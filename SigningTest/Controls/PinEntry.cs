using System;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SigningTest.Controls
{
	public class PinEntry : ContentView
	{
		private ImageSource _emtpyImageSource;
		private ImageSource _filledImageSource;
		private Image[] _pinImages;
		private Label[] _pinText;
		private Entry _pinEntry = new Entry() { HeightRequest = 0, WidthRequest = 0, Keyboard = Keyboard.Numeric, IsVisible = false };

		private bool _isChanging = false;

		public static readonly BindableProperty PinLengthProperty = BindableProperty.Create(nameof(PinLength),
																							typeof(int),
																							typeof(PinEntry),
																							6);
		public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading),
																							typeof(bool),
																							typeof(PinEntry),
																							false,
																							BindingMode.Default,
																							null,
																							IsLoadingPropertyChanged);
		public static readonly BindableProperty InputProperty = BindableProperty.Create(nameof(InputView),
																						typeof(string),
																						typeof(PinEntry),
																						string.Empty,
																						BindingMode.Default,
																						null,
																						InputPropertyChanged);
		public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
																						  typeof(ICommand),
																						  typeof(PinEntry),
																						  null);
		public static readonly BindableProperty IsPlainTextProperty = BindableProperty.Create(nameof(IsPlainText),
		                                                                                      typeof(bool),
		                                                                                      typeof(PinEntry),
		                                                                                      false);

		public int PinLength
		{
			get { return (int)GetValue(PinLengthProperty); }
			set { SetValue(PinLengthProperty, value); }
		}

		public bool IsLoading
		{
			get { return (bool)GetValue(IsLoadingProperty); }
			set { SetValue(IsLoadingProperty, value); }
		}

		public string Input
		{
			get { return (string)GetValue(InputProperty); }
			set { SetValue(InputProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public bool IsPlainText
		{
			get { return (bool)GetValue(IsPlainTextProperty); }
			set { SetValue(IsPlainTextProperty, value); }
		}

		/// <summary>
		/// Init the control during OnParentSet to be sure all XAML based property values are known
		/// </summary>
		protected override void OnParentSet()
		{
			base.OnParentSet();
			InitImageSources();
			DrawLayout();
			RegisterEventHandlers();
		}

		public void Reset()
		{
			for (int i = 0; i < PinLength; i++)
				_pinImages[i].IsVisible = _pinText[i].IsVisible = false;
		}

		public void SetFocus()
		{
			Device.BeginInvokeOnMainThread(() => _pinEntry.Focus());
		}

		private static void IsLoadingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var self = (PinEntry)bindable;
			self.IsLoading = (bool)newValue;
		}

		private static void InputPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var self = (PinEntry)bindable;
			self._pinEntry.Text = (string)newValue;
		}

		private void InitImageSources()
		{
			_filledImageSource = new FileImageSource { File = "inputbig" };
			_emtpyImageSource = new FileImageSource { File = "inputsmall" };
		}

		private void DrawLayout()
		{
			_pinImages = new Image[PinLength];
			_pinText = new Label[PinLength];

			var pinGrid = new Grid()
			{
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Center,
				RowDefinitions = new RowDefinitionCollection()
				{
					new RowDefinition() { Height = GridLength.Auto }
				},
				ColumnDefinitions = new ColumnDefinitionCollection()
			};

			pinGrid.Children.Add(_pinEntry, 0, 0);

			for (int i = 0; i < PinLength; i++)
			{
				_pinImages[i] = new Image()
				{
					Source = _filledImageSource,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					IsVisible = false
				};

				_pinText[i] = new Label()
				{
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					//TODO: Glenn - We need to be able to bind this!
					BackgroundColor = Color.White,
					IsVisible = false
				};

				Image emptyImage = new Image()
				{
					Source = _emtpyImageSource,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				};

				pinGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

				pinGrid.Children.Add(emptyImage, i, 0);
				pinGrid.Children.Add(_pinImages[i], i, 0);
				pinGrid.Children.Add(_pinText[i], i, 0);
			}

			Content = pinGrid;
		}

		private void RegisterEventHandlers()
		{
			_pinEntry.TextChanged += OnPinEntryTextChanged;
		}

		private void OnPinEntryTextChanged(object sender, TextChangedEventArgs e)
		{
			if (!_isChanging && !IsLoading)
				OnInputChanged();
		}

		private void OnInputChanged()
		{
			_isChanging = true;

			//Get the current pin entry
			string currentEntry = GetEntryText();
			_pinEntry.Text = Input = currentEntry;

			if (!string.IsNullOrEmpty(currentEntry))
			{
				if (IsPlainText)
				{
					_pinText[currentEntry.Length - 1].Text = currentEntry.ToCharArray().Last().ToString();
					_pinText[currentEntry.Length - 1].IsVisible = true;
					if (currentEntry.Length < PinLength)
					{
						_pinText[currentEntry.Length].IsVisible = false;
						_pinText[currentEntry.Length].Text = string.Empty;
					}
				}
				else
				{
					_pinImages[currentEntry.Length - 1].IsVisible = true;
					if (currentEntry.Length < PinLength)
						_pinImages[currentEntry.Length].IsVisible = false;
				}
			}
			else
			{
				if (IsPlainText)
				{
					_pinText[0].IsVisible = false;
					_pinText[0].Text = string.Empty;
				}
				else
					_pinImages[0].IsVisible = false;
			}

			if (!string.IsNullOrEmpty(currentEntry) && currentEntry.Length == PinLength && Command != null)
				Command.Execute(null);

			_isChanging = false;
		}

		private string GetEntryText()
		{
			if (!string.IsNullOrEmpty(_pinEntry.Text))
				return new string(_pinEntry.Text.ToCharArray().Where(char.IsDigit).Take(PinLength).ToArray());

			return string.Empty;
		}
	}
}
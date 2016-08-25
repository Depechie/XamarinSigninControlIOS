using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using System.Dynamic;
namespace SigningTest.ViewModels
{
	public class SigningViewModel : INotifyPropertyChanged
	{
		private string _pinEntry;
		public string PinEntry
		{
			get { return _pinEntry; }
			set
			{
				_pinEntry = value;
				NotifyPropertyChanged();
			}
		}

		private ICommand _validatePinCommand;
		public ICommand ValidatePinCommand => _validatePinCommand ?? (_validatePinCommand = new Command(ValidatePin));

		public SigningViewModel()
		{
		}

		private void ValidatePin()
		{
			var t = PinEntry;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}


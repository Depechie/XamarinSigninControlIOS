using Xamarin.Forms;
using SigningTest.Controls;
using SigningTest.ViewModels;
using System.Threading.Tasks;

namespace SigningTest
{
	public partial class SigningTestPage : ContentPage
	{
		public SigningTestPage()
		{
			InitializeComponent();

			SigningViewModel viewModel = new SigningViewModel();
			BindingContext = viewModel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Pin.SetFocus();
		}
	}
}


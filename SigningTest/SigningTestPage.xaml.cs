using System;
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
            PinSetfocus();
        }

	    public void OnPinEntryTapped(object sender, EventArgs args)
        {
	        PinSetfocus();
	    }

	    private void PinSetfocus()
	    {
            Pin.SetFocus();
        }
	}
}


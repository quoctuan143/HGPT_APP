using HGPT_APP.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        AboutViewModel viewModel;
        public AboutPage()
        {
            InitializeComponent();
            viewModel = new AboutViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
    }
}
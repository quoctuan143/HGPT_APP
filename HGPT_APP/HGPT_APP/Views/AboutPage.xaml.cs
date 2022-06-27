using HGPT_APP.Global;
using HGPT_APP.Popup;
using HGPT_APP.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            if (Preferences.Get(Config.User, "tuannq") == "tuannq")
            {
                thongbao.IsVisible = false;
                boxthongbao.IsVisible = false;
            }
            viewModel = new AboutViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            BackButtonPressed();
            return true;
        }
        public async Task BackButtonPressed()
        {
            var ok = await new MessageYesNo("Thông báo", "Bạn có muốn thoát chương trình không?").Show();
            if (ok == DialogReturn.OK)
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
    }
}
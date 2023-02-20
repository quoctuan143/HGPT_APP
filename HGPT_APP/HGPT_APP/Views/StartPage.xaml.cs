using HGPT_APP.Global;
using HGPT_APP.Popup;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
		public StartPage ()
		{
			InitializeComponent ();
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChangedAsync;
            NavigationPage.SetHasNavigationBar(this, false);
        }
        private async void Current_ConnectivityChangedAsync(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await new MessageBox("Thông báo", "Vui lòng kiểm tra lại Internet!").Show();
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!CrossConnectivity.Current.IsConnected)
            {
                await ShowMessage("Thông Báo", "Vui Lòng kiểm tra lại kết nối mạng", "OK", () =>
                { App.Current.MainPage = new Login(); });
            }
            await Task.Delay(2000);
            try
            {

                var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Login?username=" + Preferences.Get(Config.User, "1") + "&password=" + Preferences.Get(Config.Password, "1")).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    App.Current.MainPage = new AppShell();

                }
                else
                {
                    App.Current.MainPage = new Login();
                }

            }
            catch (Exception ex)
            {

                await ShowMessage("Thông Báo", ex.Message, "OK", () =>
                { App.Current.MainPage = new Login(); });
            }

        }

        public async Task ShowMessage(string title, string message, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);

            afterHideCallback?.Invoke();
        }
    }
}
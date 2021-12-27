using HGPT_APP.Global;
using HGPT_APP.Popup;
using HGPT_APP.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HGPT_APP.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
      public   INavigation navigation;

        #region "Contructor"

        public AboutViewModel()
        {
            Title = "Thông tin ứng dụng";

            LogoutCommand = new Command(OnLogoutClicked);
            ChangePasswordCommand = new Command(OnChangePassClicked);
            InformationCommand = new Command(OnShowInformationClicked);
            SettingNotification = new Command(OnSettingNotificationClicked);
        }

        private async void OnSettingNotificationClicked(object obj)
        {
            await navigation.PushAsync(new Setting());
        }
        #endregion

        #region "Method"
        private void OnLogoutClicked(object obj)
        {
            App.Current.MainPage = new Login();
        }
        private async void OnChangePassClicked(object obj)
        {
            try
            {
                var ok = await new MessageChangePassword().Show();
                if (ok == DialogReturn.OK)
                {
                    App.Current.MainPage = new Login();
                }
            }
            catch (Exception)
            {


            }
        }

        private async void OnShowInformationClicked(object obj)
        {
            await new AppInformation().Show();
        }

        #endregion

        #region "Command"
        public Command InformationCommand { get; }
        public Command LogoutCommand { get; }
        public Command ChangePasswordCommand { get; }
        public Command SettingNotification { get; } 
        #endregion

    }
}
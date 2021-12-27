using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HGPT_APP.Services;
using HGPT_APP.Views;
using HGPT_APP.Global;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Collections.Generic;
using Plugin.FirebasePushNotification;

namespace HGPT_APP
{
    public partial class App : Application
    {
          
        public App(Dictionary<string, string> dict) 
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            DependencyService.Register<MockDataStore>();
            new Config();
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                string token = CrossFirebasePushNotification.Current.Token;
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };
            MainPage = new NavigationPage(new SplashPage());
            if (dict.Count > 0)
            {
                MainPage.Navigation.PushAsync(new NotificationPage());
               
            }
            
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=2998244f-393f-43fe-aeda-fe30629e1fe5;" +
                  "ios=13ae1038-e27c-4cde-afc6-4bb80c627d67;",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

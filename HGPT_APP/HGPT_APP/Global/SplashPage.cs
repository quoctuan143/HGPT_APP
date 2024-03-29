﻿using HGPT_APP.Popup;
using HGPT_APP.Views;
using Plugin.Connectivity;
using Plugin.LatestVersion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HGPT_APP.Global
{
    public class SplashPage : ContentPage
    {

        // IMqttClient client;
        Image image;
        public SplashPage()
        {
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChangedAsync;


            NavigationPage.SetHasNavigationBar(this, false);
            var sub = new AbsoluteLayout();
            image = new Image
            {
                Source = "logo_hgpt.png",
                WidthRequest = 250,
                HeightRequest = 250

            };
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            var lable = new Label
            {
                Text = "App tiện ích HGPT",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Red,
                FontSize = 20 
            };
            AbsoluteLayout.SetLayoutFlags(lable, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lable, new Rectangle(0.5, 0.95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            sub.Children.Add(image);
            sub.Children.Add(lable);
            this.BackgroundColor = Color.White;
            this.Content = sub;
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
            await image.ScaleTo(1, 2000);//thời gian khởi tạo
                                         //await image.ScaleTo(0.9, 1500, Easing.Linear);
                                         // await image.ScaleTo(150, 500, Easing.Linear);

            //kiêm tra xem user có thay đổi k
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

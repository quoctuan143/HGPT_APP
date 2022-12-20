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
using HGPT_APP.Views.GiamSat;
using HGPT_APP.Views.SinhNhatKhachHang;
using HGPT_APP.Popup;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Net.Http;
using HGPT_APP.Models;
using Device = Xamarin.Forms.Device;

namespace HGPT_APP
{
    public partial class App : Application
    {
          
        public App(bool isNotification, object dataNotification = null)
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            DependencyService.Register<MockDataStore>();
            new Config();
            Xamarin.Forms.Device.SetFlags(new string[] { "CollectionView_Experimental", "Brush_Experimental", "SwipeView_Experimental", "CarouseView_Experimental", "IndicatorView_Experimental" });
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                string token = CrossFirebasePushNotification.Current.Token;
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };            
            if (CrossFirebasePushNotification.Current.Token != Preferences.Get(Config.Token, "1"))
            {
                try
                {
                    Preferences.Set(Config.Token, CrossFirebasePushNotification.Current.Token);
                    using (HttpClient client1 = new HttpClient())
                    {
                        Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "sinhnhatkhachhang", UserName = Preferences.Get(Config.User, "1"), Device = Device.RuntimePlatform };
                        client1.BaseAddress = new Uri(Config.URL);
                        var ok = client1.PostAsJsonAsync("api/qltb/InsertToken", token);
                        var d = ok.Result.Content.ReadAsStringAsync();
                        client1.Dispose();
                    }
                }
                catch { }               
                
            }
            if (isNotification == false)
            {
                MainPage = new NavigationPage(new SplashPage());
                if (dataNotification != null )
                {
                    var json = JsonConvert.SerializeObject(dataNotification);
                    var Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);   
                    string loaiphieu;
                    try
                    {
                        if (Data["sochungtu"].ToString() != "")
                        {
                            
                            string soChungTu = Data["sochungtu"].ToString();
                            loaiphieu = Data["loaiphieu"].ToString();                            
                            switch (loaiphieu)
                            {
                                case "ThongBaoBaoTri":
                                    MainPage.Navigation.PushAsync(new KeHoachBaoTriPage());
                                    break;
                                case "ThongBaoPhanViec":
                                    MainPage.Navigation.PushAsync(new Phan_Chia_Cong_Viec());
                                    break;
                                case "LenhSanXuat":
                                    MainPage.Navigation.PushAsync(new DanhSachLenhSanXuat());
                                    break;
                                case "ThongBaoCongTy":
                                    MainPage.Navigation.PushAsync(new NotificationPage());
                                    break;
                                case "ThongBaoGiamSat":
                                    MainPage.Navigation.PushAsync(new XemBaoCaoGiamSat_Page(soChungTu, Convert.ToDateTime(Data["date"])));
                                    break;
                                case "sinhnhatkhachhang":
                                    MainPage.Navigation.PushAsync(new SinhNhatKhachHang_ChuaXuLy());
                                    break;
                            }
                        }
                    }

                    catch
                    {
                    }
                };
            }    
            
               
           // MainPage = new NavigationPage(new SplashPage());            
            
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

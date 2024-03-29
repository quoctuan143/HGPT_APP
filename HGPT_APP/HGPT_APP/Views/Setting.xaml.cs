﻿using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Setting : ContentPage
    {
        public Setting()
        {
            InitializeComponent();
            chkcanhbaokhongkethuccongviec.IsToggled = Preferences.Get(Config.ThongBaoPhanViec, false );
            chknhanthongbaocongty.IsToggled = Preferences.Get(Config.ThongBaoCongTy, false);
            chkNhanBaoTri.IsToggled = Preferences.Get(Config.ThongBaoBaoTri, false);
            chknhanthongbaolenhsanxuat.IsToggled = Preferences.Get(Config.LenhSanXuat , false);
            chknhanthongbaogiamsat.IsToggled = Preferences.Get(Config.ThongBaoGiamSat, false);
            chkcanhbaokhongkethuccongviec.Toggled += chkcanhbaokhongkethuccongviec_Toggled;
            chknhanthongbaocongty.Toggled += Chknhanthongbaocongty_Toggled;
            chkNhanBaoTri.Toggled += ChkNhanBaoTri_Toggled;
            chknhanthongbaolenhsanxuat.Toggled += Chknhanthongbaolenhsanxuat_Toggled;
            chknhanthongbaogiamsat.Toggled += Chknhanthongbaogiamsat_Toggled;
        }

        private async void Chknhanthongbaogiamsat_Toggled(object sender, ToggledEventArgs e)
        {
            if (chknhanthongbaogiamsat.IsToggled == true)
            {
                Preferences.Set(Config.ThongBaoGiamSat, true);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "ThongBaoGiamSat", UserName = Preferences.Get(Config.User, ""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);

                    var ok = client.PostAsJsonAsync("api/qltb/InsertToken", token);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Đã đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }
            else
            {
                Preferences.Set(Config.ThongBaoGiamSat, false);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "ThongBaoGiamSat", UserName = Preferences.Get(Config.User, ""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);
                    var ok = client.PostAsJsonAsync("api/qltb/DeleteToken", token);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Hủy đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }
        }

        private async void Chknhanthongbaolenhsanxuat_Toggled(object sender, ToggledEventArgs e)
        {
            if (chkNhanBaoTri.IsToggled == true)
            {
                Preferences.Set(Config.LenhSanXuat, true);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "LenhSanXuat" ,UserName = Preferences.Get(Config.User,""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);

                    var ok = client.PostAsJsonAsync("api/qltb/InsertToken", token);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Đã đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }
            else
            {
                Preferences.Set(Config.LenhSanXuat, false);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "LenhSanXuat", UserName = Preferences.Get(Config.User, ""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);
                    var ok = client.PostAsJsonAsync("api/qltb/DeleteToken", token);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Hủy đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }
        }

        private async void ChkNhanBaoTri_Toggled(object sender, ToggledEventArgs e)
        {
            if (chkNhanBaoTri.IsToggled == true)
            {
                Preferences.Set(Config.ThongBaoBaoTri, true);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "ThongBaoBaoTri", UserName = Preferences.Get(Config.User, ""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);

                    var ok = client.PostAsJsonAsync("api/qltb/InsertToken", token);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Đã đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }
            else
            {
                Preferences.Set(Config.ThongBaoBaoTri, false);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "ThongBaoBaoTri", UserName = Preferences.Get(Config.User, ""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);
                    var ok = client.PostAsJsonAsync("api/qltb/DeleteToken", token);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Hủy đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }
        }

        private async  void Chknhanthongbaocongty_Toggled(object sender, ToggledEventArgs e)
        {
            if (chknhanthongbaocongty.IsToggled == true)
            {
                Preferences.Set(Config.ThongBaoCongTy, true);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "NhanThongBaoCongTy", UserName = Preferences.Get(Config.User, ""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);

                    var ok = client.PostAsJsonAsync("api/qltb/InsertToken", token);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Đã đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }
            else
            {
                Preferences.Set(Config.ThongBaoCongTy, false);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "NhanThongBaoCongTy", UserName = Preferences.Get(Config.User, ""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);
                    var ok = client.PostAsJsonAsync("api/qltb/DeleteToken", token);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Hủy đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }
        }

      
        private async void chkcanhbaokhongkethuccongviec_Toggled(object sender, ToggledEventArgs e)
        {
            if (chkcanhbaokhongkethuccongviec.IsToggled == true )
            {
                Preferences.Set(Config.ThongBaoPhanViec, true);
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "ThongBaoPhanViec", UserName = Preferences.Get(Config.User, ""), Device=Device.RuntimePlatform  };
                    client.BaseAddress = new Uri(Config.URL);
                   
                    var ok = client.PostAsJsonAsync("api/qltb/InsertToken", token );
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {                        
                        DependencyService.Get<IMessage>().ShortAlert("Đã đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }   
            else
            {
                Preferences.Set(Config.ThongBaoPhanViec, false );
                using (HttpClient client = new HttpClient())
                {
                    Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "ThongBaoPhanViec", UserName = Preferences.Get(Config.User, ""), Device = Device.RuntimePlatform };
                    client.BaseAddress = new Uri(Config.URL);
                    var ok = client.PostAsJsonAsync("api/qltb/DeleteToken", token   );
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Hủy đăng ký thành công");
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result).Show();
                    }
                    client.Dispose();
                }
            }    
        }

        
    }
}
using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using Newtonsoft.Json;
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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(btnusername.Text) || string.IsNullOrEmpty(btnpassword.Text))
                {
                    await DisplayAlert("Thông Báo", "Vui lòng điền đẩy đủ username và password", "Ok");
                    return;
                }
                await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi");
                using (HttpClient client = new HttpClient())
                {

                    var _json = client.GetStringAsync(Config.URL + "api/hgpt/get_Login?username=" + btnusername.Text + "&password=" + btnpassword.Text).Result;
                    await Task.Delay(1000);

                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        await DependencyService.Get<IProcessLoader>().Hide();
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        List<Account> user = JsonConvert.DeserializeObject<List<Account>>(result);
                        if (swRememer.IsChecked == true)
                        {
                            Preferences.Set(Config.Password, btnpassword.Text);
                        }
                        Preferences.Set(Config.User, btnusername.Text);
                        Preferences.Set(Config.Role, user[0].Role.ToString());
                        Preferences.Set(Config.PhoneNumber, user[0].PhoneNumber);
                        Preferences.Set(Config.NhaMay, user[0].WorkCenterCode);
                        Preferences.Set(Config.FullName, user[0].FullName);
                        Preferences.Set(Config.AnhDaiDien, user[0].ImageURL);
                        Preferences.Set(Config.IsThietBi, user[0].IsThietBi.ToString());
                        Preferences.Set(Config.IsPhanViec, user[0].IsPhanViec.ToString());
                        Preferences.Set(Config.IsChamSocKhachHang, user[0].ChamSocKhachHang.ToString());
                        Preferences.Set(Config.ThongBaoCongTy, true);
                        Preferences.Set(Config.ThongBaoBaoTri, true);
                        Preferences.Set(Config.LenhSanXuat, true);
                        Preferences.Set(Config.ThongBaoPhanViec, true);
                        Preferences.Set(Config.Token, CrossFirebasePushNotification.Current.Token);
                        //App.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        await DependencyService.Get<IProcessLoader>().Hide();
                        await DisplayAlert("Thông Báo", "Thông tin đăng nhập không chính xác", "Ok");
                        return;
                    }

                    await DependencyService.Get<IProcessLoader>().Hide();
                    if (swRememer.IsChecked == true)
                    {
                        Preferences.Set(Config.User, btnusername.Text);
                        Preferences.Set(Config.Password, btnpassword.Text);
                    }
                    //đăng kí token quản lý sinh nhật
                    using (HttpClient client1 = new HttpClient())
                    {
                        Token token = new Token { TokenKey = CrossFirebasePushNotification.Current.Token, Topic = "sinhnhatkhachhang", UserName = btnusername.Text };
                        client1.BaseAddress = new Uri(Config.URL);
                        var ok = client1.PostAsJsonAsync("api/qltb/InsertToken", token);
                        var d = ok.Result.Content.ReadAsStringAsync();
                        client1.Dispose();
                    }
                    App.Current.MainPage = new AppShell();
                    client.Dispose();
                }    

            }
            catch (Exception ex)
            {

                await DependencyService.Get<IProcessLoader>().Hide();
                await DisplayAlert("Lỗi", ex.Message, "Ok");
            }
        }
    }
}
using HGPT_APP.Global;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.SinhNhatKhachHang
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoiDungChamSocKhachHang : ContentPage
    {
        public ChamSocKhachHang CSKH;
        public NoiDungChamSocKhachHang(ChamSocKhachHang cskh )
        {
            InitializeComponent();
            CSKH = cskh;
            BindingContext = this;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn lưu không").Show();
                if (ok == Global.DialogReturn.OK )
                {
                    using (HttpClient client  = new HttpClient ())
                    {
                        CSKH.NoiDungChamSoc = rte.Text;
                        CSKH.NguoiChamSoc = Preferences.Get(Config.User, "");
                        CSKH.NgayChamSoc  = DateTime.Now.Date ;
                        client.BaseAddress = new Uri(Config.URL);
                        var post = client.PostAsJsonAsync("api/qltb/PostNoiDungChamSocKhachHang", CSKH);
                        if (post.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                        {
                            await new MessageBox("Thông báo", "Đã lưu thành công").Show();
                            MessagingCenter.Send(this, "xulysinhnhatkhachhang", CSKH);
                            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());

                        }    
                    }    
                }    
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message ).Show();
            }
        }
    }
}
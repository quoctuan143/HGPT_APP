using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Nhap_Phu_Tung : PopupPage
    {
        string MaThietBi;
        string flag;
        int rowid;
        public Nhap_Phu_Tung(string mathietbi, string ghichu, string _flag,int _rowid)
        {
            InitializeComponent();
            MaThietBi = mathietbi;
            rowid = _rowid;
            flag = _flag;
            txtPhuTung.Text = ghichu;
        }
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhuTung.Text))
                   
            {
                await new MessageBox("Thông báo", "Vui lòng nhập chi tiết").Show();
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                
                if (flag =="new")
                {
                    Danh_Muc_Phu_Tung_Model ptung = new Danh_Muc_Phu_Tung_Model { Description = txtPhuTung.Text, Ma_Thiet_Bi = MaThietBi };
                    client.BaseAddress = new Uri(Config.URL);
                    var ok = client.PostAsJsonAsync("api/qltb/PostChiTietPhuTung", ptung);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("sucess"))
                    {
                        MessagingCenter.Send(this, "UpdatePhuTung", ptung);
                        await Navigation.PopPopupAsync();
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                    }
                }    
                else
                {
                    Danh_Muc_Phu_Tung_Model ptung = new Danh_Muc_Phu_Tung_Model { Description = txtPhuTung.Text, Ma_Thiet_Bi = MaThietBi,RowID =rowid  };
                    client.BaseAddress = new Uri(Config.URL);
                    var ok = client.PostAsJsonAsync("api/qltb/UpdateGhiChuThietBi", ptung);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("sucess"))
                    {
                        MessagingCenter.Send(this, "UpdatePhuTung", ptung);
                        await Navigation.PopPopupAsync();
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                    }
                }
                
                client.Dispose();

            }
            
            
        }
        public async Task Show()
        {
           
            await Navigation.PushPopupAsync(this);
           
        }

        private async  void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}
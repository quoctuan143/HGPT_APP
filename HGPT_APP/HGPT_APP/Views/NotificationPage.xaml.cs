using HGPT_APP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HGPT_APP.Global;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using HGPT_APP.Interface;
using HGPT_APP.Popup;
using HGPT_APP.Views.SinhNhatKhachHang;
using Plugin.FirebasePushNotification;
using Xamarin.Essentials;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage , INotifyPropertyChanged
    {
        ObservableCollection<NotifycationModel> _listthongbao;
        public ObservableCollection <NotifycationModel> ListThongBao { get => _listthongbao ; 
            set { _listthongbao = value; OnPropertyChanged(nameof(ListThongBao)); } 
        }
      
        bool _isRunning;
        public bool IsRunning { get => _isRunning; set { _isRunning = value; OnPropertyChanged(nameof(IsRunning)); } }
        public NotificationPage()
        {
            InitializeComponent();
            ListThongBao = new ObservableCollection<NotifycationModel>();
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           using (HttpClient client = new HttpClient())
            {
                IsRunning = true;
                try
                {
                    var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getNotification?token=" + Preferences.Get(Config.Token, "1")).Result;

                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        ListThongBao = JsonConvert.DeserializeObject<ObservableCollection<NotifycationModel>>(result);
                        listThongBao.ItemsSource = ListThongBao;
                    }
                }
                catch
                {

                   
                }
               
                IsRunning = false;
            }    
        }
        

        private async  void listThongBao_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                NotifycationModel item = listThongBao.SelectedItem as NotifycationModel;
                if (item != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var ok = client.PostAsJsonAsync("api/qltb/UpdateNotification?RowID=" + item.RowID, item);
                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                        {

                            if (item.Code == "LenhSanXuat")
                                await Navigation.PushAsync(new DanhSachLenhSanXuat());
                            else if (item.Code == "ThongBaoBaoTri")
                                await Navigation.PushAsync(new KeHoachBaoTriPage());
                            else if (item.Code == "ThongBaoPhanViec")
                                await Navigation.PushAsync(new Phan_Chia_Cong_Viec());
                            else if (item.Code == "sinhnhatkhachhang")
                                await Navigation.PushAsync(new SinhNhatKhachHang_ChuaXuLy());
                        }
                        else
                        {
                            await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                        }
                        client.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {


            }
        }
    }
}
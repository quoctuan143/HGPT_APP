using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinThietBi_Edit : ContentPage, INotifyPropertyChanged
    {
        public DanhMuc_ThietBi Item { get; set; }
        public ThongTinThietBi_Edit(DanhMuc_ThietBi item)
        {
            InitializeComponent();
            Item = item;
            ListNhaMay = new ObservableCollection<DM_NHA_MAY>();
            ListPhongBan = new ObservableCollection<DM_TO_SAN_XUAT>();
            BindingContext = this;
        }
        ObservableCollection<DM_NHA_MAY> _nhaMays;
        public ObservableCollection<DM_NHA_MAY> ListNhaMay { get => _nhaMays;set { _nhaMays = value; OnPropertyChanged("ListNhaMay"); } }

        ObservableCollection<DM_TO_SAN_XUAT> _phongBan;
        public ObservableCollection<DM_TO_SAN_XUAT> ListPhongBan { get => _phongBan; set { _phongBan = value; OnPropertyChanged("ListPhongBan"); } }

        DM_NHA_MAY _selectNhaMay;
        public DM_NHA_MAY SelectNhaMay { get => _selectNhaMay; 
            set {
                
                if (_selectNhaMay != value)
                {
                    _selectNhaMay = value;
                    try
                    {
                        var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getXuong?nhamay=" + SelectNhaMay.Code).Result;
                        _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                        if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                        {
                            Int32 from = _json.IndexOf("[");
                            Int32 to = _json.IndexOf("]");
                            string result = _json.Substring(from, to - from + 1);
                            ListPhongBan.Clear();
                            ListPhongBan = JsonConvert.DeserializeObject<ObservableCollection<DM_TO_SAN_XUAT>>(result);
                            foreach (DM_TO_SAN_XUAT  pb in ListPhongBan)
                            {
                                if (pb.Code == Item.Ma_Phong_Ban )
                                {
                                    SelectPhongBan  = pb;
                                }
                            }
                            OnPropertyChanged("ListPhongBan");
                        }

                        OnPropertyChanged("SelectNhaMay");
                     }
                    catch (Exception)
                    {


                    }
                }    
                  
                }}

        DM_TO_SAN_XUAT _selectPhongBan;
        public DM_TO_SAN_XUAT SelectPhongBan 
        {
            get => _selectPhongBan;
            set
            {

                if (_selectPhongBan != value)
                {
                    _selectPhongBan = value;
                    OnPropertyChanged("SelectPhongBan");
                }

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (SelectPhongBan == null )
                {
                    await new MessageBox("Thông báo", "Chọn phòng ban sử dụng").Show();
                    return;
                }

                if (SelectNhaMay == null)
                {
                    await new MessageBox("Thông báo", "Chọn nhà máy sử dụng").Show();
                    return;
                }
                var ok1 = await new MessageYesNo("Thông báo", "Bạn có muốn cập nhật không?").Show();
                if (ok1 == Global.DialogReturn.OK)
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Config.URL);
                    Item.Ma_Nha_May = SelectNhaMay.Code;
                    Item.Ma_Phong_Ban = SelectPhongBan.Code;
                    var ok = client.PostAsJsonAsync("api/qltb/UpdateThongTinThietBi", Item);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("cập nhật thành công"))
                    {

                        DependencyService.Get<IMessage>().ShortAlert("Cập nhật thành công");
                        MessagingCenter.Send(this, "UpdateThietBi", Item);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                    }
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message ).Show();
            }
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Nha_May").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListNhaMay= JsonConvert.DeserializeObject<ObservableCollection<DM_NHA_MAY>>(result);
                    foreach (DM_NHA_MAY lsx in ListNhaMay)
                    {
                        if (lsx.Code == Item.Ma_Nha_May)
                        {
                            SelectNhaMay = lsx;
                        }
                    }
                }

                 
            }
            catch (Exception)
            {

               
            }
           

             
        }
    }
}
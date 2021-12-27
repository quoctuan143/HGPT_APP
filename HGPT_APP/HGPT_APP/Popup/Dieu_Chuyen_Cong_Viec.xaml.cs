using HGPT_APP.Global;
using HGPT_APP.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dieu_Chuyen_Cong_Viec : PopupPage , INotifyPropertyChanged
    {
        TaskCompletionSource<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> _tsk = null;
        public LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN Item { get; set; }
        ObservableCollection<Work_Type_Header> _listCongDoan;
        public ObservableCollection<Work_Type_Header> ListCongDoan
        {
            get => _listCongDoan;
            set
            {
                if (value != null)
                {
                    _listCongDoan = ListCongDoan;
                    //OnPropertyChanged("ListCongDoan");
                }    
            }
        }
        public Dieu_Chuyen_Cong_Viec(LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN _item)
        {
            InitializeComponent();
            Item = _item;
            this.ListCongDoan = new ObservableCollection<Work_Type_Header>();
            BindingContext = this;
        }
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            if (cbNhaMay.SelectedItem == null)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "vui lòng chọn nhà máy", "OK");
                return;
            }
            if (cbLenhSanXuat.SelectedItem == null)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "vui lòng chọn lệnh sản xuất", "OK");
                return;
            }
            if (cbBoxCongDoan.SelectedItem  == null)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "vui lòng chọn công đoạn", "OK");
                return;
            }
            try
            {
                DANH_MUC_LENH_SAN_XUAT lsx = cbLenhSanXuat.SelectedItem as DANH_MUC_LENH_SAN_XUAT;
                DM_NHA_MAY nhamay = cbNhaMay.SelectedItem as DM_NHA_MAY;
                LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN result = new LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN();
                result.GIO_BAT_DAU = DateTime.Now;
                result.GIO_KET_THUC = null;
                result.MA_CONG_DOAN = cbBoxCongDoan.Text;
                result.MA_NHAN_VIEN = Item.MA_NHAN_VIEN;
                result.TRANG_THAI = 1;
                result.NGAY_TAO = DateTime.Now;
                result.NOI_DUNG = txtGhiChu.Text;
                result.DINH_MUC  = txtDinhMuc.Text;
                result.MA_NHA_MAY = nhamay.Code;
                result.LENH_SAN_XUAT = lsx.LENH_SAN_XUAT;
                result.TEN_NHAN_VIEN = Item.TEN_NHAN_VIEN;
                result.TO_SAN_XUAT = Item.TO_SAN_XUAT;
                result.NGUOI_TAO = Preferences.Get(Config.User, "");
                result.ExternalDocumentNo_ = lsx.Ten_Day_Du;
                await Navigation.PopAllPopupAsync(true);
                _tsk.SetResult(result);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "vui lòng chọn công đoạn", "OK");
                return;

            }
            
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync(true);
            _tsk.SetResult(null );
        }
        public async Task<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> Show()
        {
            _tsk = new TaskCompletionSource<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //danh mục công đoạn
            var _json =  Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Cong_Doan").Result;           
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);
                cbBoxCongDoan.DataSource = JsonConvert.DeserializeObject<ObservableCollection<Work_Type_Header>>(result);
                
            }

            //danh mục lệnh san xuất
            _json  = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Lenh_San_Xuat").Result;            
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);                
                ObservableCollection<DANH_MUC_LENH_SAN_XUAT> LstLSX = JsonConvert.DeserializeObject<ObservableCollection<DANH_MUC_LENH_SAN_XUAT>>(result);
                cbLenhSanXuat.DataSource = LstLSX;
                foreach (DANH_MUC_LENH_SAN_XUAT lsx in LstLSX)
                {
                    if (lsx.LENH_SAN_XUAT == Item.LENH_SAN_XUAT)
                    {
                        cbLenhSanXuat.SelectedItem = lsx;
                    }
                }
            }

            //danh muc nhà máy
            _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Nha_May").Result;
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);
                ObservableCollection<DM_NHA_MAY> lstNhaMay = JsonConvert.DeserializeObject<ObservableCollection<DM_NHA_MAY>>(result);
                cbNhaMay.DataSource = lstNhaMay;
                foreach (DM_NHA_MAY lsx in lstNhaMay)
                {
                    if (lsx.Code == Preferences.Get (Config.NhaMay ,""))
                    {
                        cbNhaMay.SelectedItem = lsx;
                    }
                }
            }
        }
    }
}
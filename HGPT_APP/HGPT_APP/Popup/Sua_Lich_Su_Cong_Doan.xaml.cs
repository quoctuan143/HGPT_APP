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
    public partial class Sua_Lich_Su_Cong_Doan : PopupPage , INotifyPropertyChanged
    {
        TaskCompletionSource<LICH_SU_PHAN_CONG_VIEC> _tsk = null;
        public LICH_SU_PHAN_CONG_VIEC Item { get; set; }  
        ObservableCollection<Work_Type_Header> _listCongDoan;
        public ObservableCollection<Work_Type_Header> ListCongDoan
        {
            get => _listCongDoan;
            set
            { 
                if (value != null)
                {
                    _listCongDoan = value ;
                    //OnPropertyChanged("ListCongDoan"); 
                }    
            }
        }

        ObservableCollection<DM_TANG_CA> _listTangCa;
        public ObservableCollection<DM_TANG_CA> ListTangCa
        {
            get => _listTangCa;
            set
            {
                if (value != null)
                {
                    _listTangCa = value ;
                    //OnPropertyChanged("ListCongDoan"); 
                }
            }
        }

        public Sua_Lich_Su_Cong_Doan(LICH_SU_PHAN_CONG_VIEC _item)
        {
            InitializeComponent();
            Item = _item;
            this.ListCongDoan = new ObservableCollection<Work_Type_Header>();
            BindingContext = this;
        }
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            if (cbLenhSanXuat.SelectedItem == null)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "vui lòng chọn lệnh sản xuất", "OK");              
                return;
            }
            if (cbBoxCongDoan.SelectedItem ==  null)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "vui lòng chọn công đoạn", "OK");
                return;
            }
            try
            {
                DANH_MUC_LENH_SAN_XUAT lsx = cbLenhSanXuat.SelectedItem as DANH_MUC_LENH_SAN_XUAT;
                Work_Type_Header cd = cbBoxCongDoan.SelectedItem as Work_Type_Header;
                DM_TANG_CA tc = cbTangCa.SelectedItem as DM_TANG_CA;
                Item.LENH_SAN_XUAT = lsx.LENH_SAN_XUAT;
                Item.ExternalDocumentNo_ = lsx.Ten_Day_Du;
                if (tc != null) Item.MA_TANG_CA = tc.Code;
                Item.MA_CONG_DOAN = cd.MA_CONG_DOAN;
                await Navigation.PopAllPopupAsync(true);
                _tsk.SetResult(Item);
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message , "OK");
                return;
            }
            
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync(true);
            _tsk.SetResult(null );
        }
        public async Task<LICH_SU_PHAN_CONG_VIEC> Show()
        {
            _tsk = new TaskCompletionSource<LICH_SU_PHAN_CONG_VIEC>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Cong_Doan").Result;           
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);               
                ObservableCollection<Work_Type_Header> LstLSX = JsonConvert.DeserializeObject<ObservableCollection<Work_Type_Header>>(result);
                cbBoxCongDoan.DataSource = LstLSX;
                foreach (Work_Type_Header cd in LstLSX)
                {
                    if (cd.MA_CONG_DOAN  == Item.MA_CONG_DOAN)
                    {
                        cbBoxCongDoan.SelectedItem = cd;                         
                    }
                }
            }

            _json  = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Lenh_San_Xuat").Result;            
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);
                ObservableCollection<DANH_MUC_LENH_SAN_XUAT > LstLSX = JsonConvert.DeserializeObject<ObservableCollection<DANH_MUC_LENH_SAN_XUAT>>(result);
                cbLenhSanXuat.DataSource = LstLSX;
                foreach (DANH_MUC_LENH_SAN_XUAT lsx in LstLSX)
                {
                    if (lsx.LENH_SAN_XUAT  == Item.LENH_SAN_XUAT  )
                    {
                        cbLenhSanXuat.SelectedItem = lsx;
                    }
                }
            }

            _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Tang_Ca").Result;
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);
                ObservableCollection<DM_TANG_CA> LstLSX = JsonConvert.DeserializeObject<ObservableCollection<DM_TANG_CA>>(result);
                cbTangCa.DataSource = LstLSX;
                foreach (DM_TANG_CA lsx in LstLSX)
                {
                    if (lsx.Code == Item.MA_TANG_CA)
                    {
                        cbTangCa.SelectedItem = lsx;
                    }
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}
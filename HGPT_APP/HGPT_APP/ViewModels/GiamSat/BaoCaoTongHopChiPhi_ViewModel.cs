using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HGPT_APP.Models;
using Xamarin.Forms;
using HGPT_APP.Global;
using Xamarin.Essentials;
using Newtonsoft.Json;
using HGPT_APP.Popup;
using System.Net.Http;
using HGPT_APP.Models.GiamSat;

namespace HGPT_APP.ViewModels.GiamSat
{
  public class BaoCaoTongHopChiPhi_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"      
        
        public INavigation navigation { get; set; }       
        DANH_MUC_LENH_SAN_XUAT _selectLenhSanXuat;
        public DANH_MUC_LENH_SAN_XUAT SelectLenhSanXuat { get => _selectLenhSanXuat; 
            set 
            {
                SetProperty(ref _selectLenhSanXuat, value);
                LoadData(null);
            } 
        }
        ObservableCollection<DANH_MUC_LENH_SAN_XUAT> _lenhsanxuatlist;
        public ObservableCollection<DANH_MUC_LENH_SAN_XUAT> ListLenhSanXuat { get => _lenhsanxuatlist; set => SetProperty(ref _lenhsanxuatlist, value); }

        ObservableCollection<TongHopChiPhi> _tonghopchiphi;
        public ObservableCollection<TongHopChiPhi> ListTongHopChiPhi { get => _tonghopchiphi; set => SetProperty(ref _tonghopchiphi, value); }
        #endregion

        #region "Commnad"
        public Command LoadCongDoanBaoCao { get; set; }       

        #endregion

        public BaoCaoTongHopChiPhi_ViewModel()
        {
            try
            {                   
                Title = "Tổng hợp chi phí theo LSX";              
                LoadCongDoanBaoCao = new Command(async () => await LoadCongDoanBaoCaoExcute());
                ListLenhSanXuat = new ObservableCollection<DANH_MUC_LENH_SAN_XUAT>();
                ListTongHopChiPhi = new ObservableCollection<TongHopChiPhi>();
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }          
        }
        public override async  Task  LoadData(object ojb)
        {
           if (_selectLenhSanXuat == null )
            {
                await new MessageBox("Thông báo", "Vui lòng chòn lệnh sản xuất").Show();
                return;
            }    
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình

                var _json = Config.client.GetStringAsync(Config.URL + $"TongHopChiPhiTheoLenhSanXuat?lenhsanxuat={SelectLenhSanXuat.LENH_SAN_XUAT}").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {
                    string result = _json.Substring(1, _json.Length - 2);
                    ListTongHopChiPhi = JsonConvert.DeserializeObject<ObservableCollection<TongHopChiPhi>>(result);
                }

                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        private async Task LoadCongDoanBaoCaoExcute()
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình
               
                var _json = Config.client.GetStringAsync(Config.URL + $"DanhSachLenhSanXuat").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {              
                    string result = _json.Substring(1, _json.Length - 2);
                    ListLenhSanXuat = JsonConvert.DeserializeObject<ObservableCollection< DANH_MUC_LENH_SAN_XUAT  >>(result);                    
                }
                
                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }
    }
}

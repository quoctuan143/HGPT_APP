using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using HGPT_APP.Global;
using Newtonsoft.Json;
using HGPT_APP.Popup;
using HGPT_APP.Models.GiamSat;


namespace HGPT_APP.ViewModels.GiamSat
{
  public class TongHopBaoCaoTienDoLapDat_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"  
       ObservableCollection < TongHopBaoCaoTienDoLapDat> _tiendolapdat;
        public ObservableCollection<TongHopBaoCaoTienDoLapDat> ListTienDoLapDat
        {
            get => _tiendolapdat;
            set
            {
                if (value != null)
                {
                    _tiendolapdat = value;
                    OnPropertyChanged("ListTienDoLapDat");
                }
            }
        }               
        #endregion

        

        public TongHopBaoCaoTienDoLapDat_ViewModel(string CongTrinh)
        {
            try
            {                         
                Title = "Chi tiết tiến độ lắp đặt";
                ListTienDoLapDat = new ObservableCollection<TongHopBaoCaoTienDoLapDat >();
                HienThiThongTin(CongTrinh);
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }        

        public async void HienThiThongTin (string CongTrinh)
        {
            try
            {            
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình
                var _json = Config.client.GetStringAsync(Config.URL + $"BaoCaoTatCaLapDatHienTaiCuaCongTrinh?macongtrinh={(CongTrinh == null ? "abc" : CongTrinh)}").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {
                    string result = _json.Substring(1, _json.Length - 2);
                    ListTienDoLapDat = JsonConvert.DeserializeObject<ObservableCollection<TongHopBaoCaoTienDoLapDat>>(result);
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

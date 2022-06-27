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
        public INavigation navigation { get; set; }
        DateTime _ngaylamviec;
        public DateTime NgayLamViec { get => _ngaylamviec; set => SetProperty(ref _ngaylamviec, value); }             
        DanhSachCongTrinh _selectCongTrinh;
        public DanhSachCongTrinh SelectCongTrinh { get => _selectCongTrinh; 
            set 
            { 
                SetProperty(ref _selectCongTrinh, value);
                HienThiThongTin();
            } 
        }      
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

        

        public TongHopBaoCaoTienDoLapDat_ViewModel()
        {
            try
            {
                NgayLamViec = DateTime.Now.Date;                
                Title = "Tổng hợp báo cáo tiến độ lắp đặt";
                ListTienDoLapDat = new ObservableCollection<TongHopBaoCaoTienDoLapDat >();
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }

        public override async Task LoadData(object ojb)
        {
           
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình
                GetCongTrinh("1");                
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

        public async void HienThiThongTin ()
        {
            try
            {
                if (_selectCongTrinh == null  )
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn công trình báo cáo").Show();
                    return;
                }
               
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình
                var _json = Config.client.GetStringAsync(Config.URL + $"BaoCaoTatCaLapDatHienTaiCuaCongTrinh?macongtrinh={(_selectCongTrinh == null ? "abc" : _selectCongTrinh.Code)}").Result;
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

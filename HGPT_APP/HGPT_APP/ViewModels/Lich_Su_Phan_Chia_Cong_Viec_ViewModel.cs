using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HGPT_APP.ViewModels
{
    class Lich_Su_Phan_Chia_Cong_Viec_ViewModel : BaseViewModel
    {
        #region "Khai báo biến"
        public DateTime NgayLamViec { get; set; }
        ObservableCollection<LICH_SU_PHAN_CONG_VIEC> Cong_Doan_Cong_Nhans;
        public ObservableCollection<LICH_SU_PHAN_CONG_VIEC> ListCong_Doan_Cong_Nhans
        {
            get => Cong_Doan_Cong_Nhans;
            set
            {
                if (value != null)
                {
                    Cong_Doan_Cong_Nhans = value;
                    OnPropertyChanged("ListCong_Doan_Cong_Nhans");
                }
            }
        }

        LICH_SU_PHAN_CONG_VIEC _selectCongDoan;
        public LICH_SU_PHAN_CONG_VIEC SelectCongDoan
        {
            get => _selectCongDoan;
            set
            {
                if (value != null)
                {
                    _selectCongDoan = value;
                    OnPropertyChanged("SelectCongDoan");
                }
            }
        }
        #endregion

        #region "Commnad"
        public Command<DateTime> LoadCongDoanCongNhan { get; set; }      
        #endregion

        public Lich_Su_Phan_Chia_Cong_Viec_ViewModel()
        {
            try
            {
                NgayLamViec = DateTime.Now.Date;
                ListCong_Doan_Cong_Nhans = new ObservableCollection<LICH_SU_PHAN_CONG_VIEC>();
                Title = "Lịch sử công việc";
                LoadCongDoanCongNhan = new Command<DateTime>(async (p) => await LoadCongDoanCongNhanExcute(p));
            }
            catch (Exception ex)
            {

                 new MessageBox("Thông báo", ex.ToString()).Show();
            }            
        }            

        private async Task LoadCongDoanCongNhanExcute(DateTime ngaytao)
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ListCong_Doan_Cong_Nhans.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Lich_Su?ngaytao=" + string.Format("{0:yyyy-MM-dd}",ngaytao ) + "&nguoitao=" + Preferences.Get(Config.User, "")).Result;
                await Task.Delay(1000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListCong_Doan_Cong_Nhans = JsonConvert.DeserializeObject<ObservableCollection<LICH_SU_PHAN_CONG_VIEC>>(result);
                }
            }
            catch (Exception ex)
            {

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

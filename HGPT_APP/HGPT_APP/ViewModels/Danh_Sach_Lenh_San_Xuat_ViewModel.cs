using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGPT_APP.Global;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using HGPT_APP.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HGPT_APP.ViewModels
{
   public  class Danh_Sach_Lenh_San_Xuat_ViewModel : BaseViewModel
    {

        #region "Khai Báo Biến"
        ObservableCollection<DANH_MUC_LENH_SAN_XUAT> _listLenhSanXuat;
        public ObservableCollection<DANH_MUC_LENH_SAN_XUAT> List_LENH_SAN_XUATs 
        { get => _listLenhSanXuat; 
            set { if (value != null) 
                {
                    _listLenhSanXuat = value;
                    OnPropertyChanged(nameof(List_LENH_SAN_XUATs));
                } 
            } 
        }

        ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN> _listCongDoanLSX;
        public ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN> ListCongDoanLSX
        {            
            get => _listCongDoanLSX;
            set
            {
                if (value != null)
                {
                    _listCongDoanLSX = value;
                    OnPropertyChanged(nameof(ListCongDoanLSX));
                }
            }
        }

        DANH_MUC_LENH_SAN_XUAT _selectLSX;
        public DANH_MUC_LENH_SAN_XUAT SelectLSX
        {
            get => _selectLSX;
            set
            {
                if (value != null)
                {
                    _selectLSX = value;                   
                    OnPropertyChanged(nameof(SelectLSX));
                }
            }
        }

        #endregion
        #region "ICommand"
        public Command LoadLenhSanXuat { get; set; }
        public Command<string> LoadCongDoanLSX { get; set; }
        public Command StopLenhSanXuat { get; set; } 

        public Command KhoiTaoCongViecCommand { get; set; }
        #endregion
        public Danh_Sach_Lenh_San_Xuat_ViewModel()
        {
            try
            {
                Title = "Danh mục lệnh sản xuất";
                List_LENH_SAN_XUATs = new ObservableCollection<DANH_MUC_LENH_SAN_XUAT>();
                ListCongDoanLSX = new ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN>();
                LoadLenhSanXuat = new Command(async () => await ExcuteLenhSanXuat());
                LoadCongDoanLSX = new Command<string>(async (p) => await ExcuteLoadCongDoanLSX(p));
                StopLenhSanXuat = new Command(async () => await ExcuteStopLenhSanXuat());
                KhoiTaoCongViecCommand = new Command(async () => await KhoiTaoCongViecExcute());
            }
            catch (Exception ex)
            {

                 new MessageBox("Thông báo", ex.ToString()).Show();
            }
            
        }



        #region "Funtionn"
        private async Task KhoiTaoCongViecExcute()
        {
            try
            {
                if (SelectLSX != null)
                    await Application.Current.MainPage.Navigation.PushAsync(new Tao_Cong_Viec_Cho_Cong_Nhan(SelectLSX));
            }
            catch (Exception ex)
            {

              await  new MessageBox("Thông báo", ex.ToString()).Show();
            }
            
        }

        private async Task   ExcuteLenhSanXuat()
        {
            try
            {
                IsBusy = true ;
                IsRunning = true ;
                List_LENH_SAN_XUATs.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Lenh_San_Xuat").Result;
                await Task.Delay(1000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    List_LENH_SAN_XUATs = JsonConvert.DeserializeObject<ObservableCollection<DANH_MUC_LENH_SAN_XUAT>>(result);
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
        
        private async Task ExcuteStopLenhSanXuat() 
        {
            try
            {
                if (SelectLSX == null )
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn hạng mục cần đóng").Show();
                    return;
                }    
                var ask = await new MessageYesNo("Thông báo", "Bạn có muốn đóng hạng mục này không?").Show();
                if (ask == DialogReturn.OK )
                {
                    var result = await RunHttpClientPost("api/hgpt/Dong_Lenh_San_Xuat", SelectLSX.LENH_SAN_XUAT );

                    if (result.IsSuccessStatusCode)
                    {
                        List_LENH_SAN_XUATs.Remove(List_LENH_SAN_XUATs.FirstOrDefault(x => x.LENH_SAN_XUAT == SelectLSX.LENH_SAN_XUAT));
                    }
                }  
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }            
        }

        private async Task  ExcuteLoadCongDoanLSX(string lsx) 
        {
            try
            {
                IsBusy = true;
                ListCongDoanLSX.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Cong_Doan_Theo_LSX?lsx=" + lsx).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListCongDoanLSX = JsonConvert.DeserializeObject<ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN>>(result);
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            finally
            {
                IsBusy = false;

            }
        }
        #endregion
    }
}

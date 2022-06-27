using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using HGPT_APP.Models;
using Xamarin.Forms;
using HGPT_APP.Global;
using Xamarin.Essentials;
using Newtonsoft.Json;
using HGPT_APP.Popup;
using HGPT_APP.Models.GiamSat;
using System.Net.Http;
using System.Threading;
using NativeMedia;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Bson;
using System.Net.Http.Headers;

namespace HGPT_APP.ViewModels.GiamSat
{
  public class ChamCongNhanSu_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"      
        
        public INavigation navigation { get; set; }
        DateTime _ngaylamviec;
        public DateTime NgayLamViec { get => _ngaylamviec; set => SetProperty(ref _ngaylamviec, value); }             
        DanhSachCongTrinh _selectCongTrinh;
        public DanhSachCongTrinh SelectCongTrinh { get => _selectCongTrinh; set => SetProperty(ref _selectCongTrinh, value); }       
       ObservableCollection < CapNhatGioCong> _capnhatgiocong;
        public ObservableCollection<CapNhatGioCong> ListCapNhatGioCong
        {
            get => _capnhatgiocong;
            set
            {
                if (value != null)
                {
                    _capnhatgiocong = value;
                    OnPropertyChanged("ListCapNhatGioCong");
                }
            }
        }
        ObservableCollection<DanhSachCongTrinh> _nhomchamcong;
        public ObservableCollection<DanhSachCongTrinh> ListNhomChamCong { get => _nhomchamcong; set => SetProperty(ref _nhomchamcong , value); }
        
        #endregion

        #region "Commnad"

        public Command GuiBaoCao { get; set; }
        public Command RemoveCommand { get; set; }        
        #endregion

        public ChamCongNhanSu_ViewModel()
        {
            try
            {
                NgayLamViec = DateTime.Now.Date;                
                Title = "Chấm công nhân viên";
                ListCapNhatGioCong = new ObservableCollection<CapNhatGioCong>();
                RemoveCommand = new  Command(DeleteNhanSuClick);
                GuiBaoCao = new Command(async () => await GuiBaoCaoClick());   
                
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }
        private async void DeleteNhanSuClick(object obj)
        {
            try
            {
                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn xóa nhân sự này không?").Show();
                if (ok == DialogReturn.OK)
                {
                    CapNhatGioCong e = obj as CapNhatGioCong;
                    ListCapNhatGioCong.Remove(e);
                }

            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
                return;
            }
        }
        private async Task GuiBaoCaoClick()
        {
            try
            {
                if (_selectCongTrinh == null )
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn công trình").Show();
                    return;
                }
                foreach (CapNhatGioCong  _item in ListCapNhatGioCong)
                {
                    if (_item.NgayCong  > 0 )
                    {
                        _item.MaCongTrinh = _selectCongTrinh.Code;
                        _item.NguoiCham = Preferences.Get(Config.User, "");
                    }    
                }            

                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn gửi chấm công đến nhân sự không?").Show();
                if (ok == DialogReturn.OK )
                {                  

                    ShowLoading("Đang xử lý vui lòng đợi");
                    await Task.Delay(1000);
                    HttpResponseMessage ok1 = null;
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);   
                        ok1 = await client.PostAsJsonAsync("CaphatGioCong?NgayCham=" + string.Format("{0:yyyy-MM-dd}",NgayLamViec ), ListCapNhatGioCong);
                        if (ok1.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            HideLoading();
                            ShortAlert("Đã gửi công nhân sự thành công");
                            await navigation.PopAsync();
                        }

                        else
                        {
                            HideLoading();
                            await new MessageBox("Thông báo", ok1.Content.ReadAsStringAsync().Result).Show();
                            return;
                        }

                    }

                }
            }
            catch (Exception ex)            
            {
                HideLoading();
                await new MessageBox("Thông báo",ex.Message ).Show();
                return;
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
                var _json = Config.client.GetStringAsync(Config.URL + $"XemNhanSuTheoGiamSatDeChamCong?giamsat={Preferences.Get(Config.User ,"")}").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {              
                    string result = _json.Substring(1, _json.Length - 2);
                    ListCapNhatGioCong = JsonConvert.DeserializeObject<ObservableCollection< CapNhatGioCong >>(result);                    
                }
                _json = Config.client.GetStringAsync(Config.URL + $"GetNhomChamCong").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {
                    string result = _json.Substring(1, _json.Length - 2);
                    ListNhomChamCong = JsonConvert.DeserializeObject<ObservableCollection<DanhSachCongTrinh>>(result);
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

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
  public class XemChamCongNhanSu_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"      
        
        public INavigation navigation { get; set; }
        DateTime _ngaylamviec;
        public DateTime NgayLamViec 
        { 
            get => _ngaylamviec; 
            set 
            {
                SetProperty(ref _ngaylamviec, value);
                LoadData(null);
            }
        }
        bool theongay = true ;
        public bool TheoNgay
        {
            get => theongay; set
            {
                SetProperty(ref theongay, value);
                LoadData(null );
            }
        }
        bool theothang = false ;
        public bool TheoThang { get => theothang; set { 
                SetProperty(ref theothang, value);
                LoadData(null );
            } }
          
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
        #endregion

        #region "Commnad"
        //public Command LoadCongDoanBaoCao { get; set; }
        public Command XoaChamCongCommand { get; set; }        
       
        #endregion

        public XemChamCongNhanSu_ViewModel()
        {
            try
            {
                NgayLamViec = DateTime.Now.Date;                
                Title = "Bảng chấm công nhân viên";
                ListCapNhatGioCong = new ObservableCollection<CapNhatGioCong>();
                XoaChamCongCommand = new Command(XoaChamCongClick);
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }

        private async void XoaChamCongClick(object obj)
        {
            try
            {
                if (theongay == false  )
                {
                    await new MessageBox("Thông báo", "Bạn chỉ xóa được khi báo cáo theo ngày").Show();
                    return;
                }
                CapNhatGioCong item = obj as CapNhatGioCong;
                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn xóa ngày công này không?").Show();
                if (ok == DialogReturn.OK)
                {
                    ShowLoading("Đang xử lý vui lòng đợi");
                    await Task.Delay(1000);
                    HttpResponseMessage ok1 = null;
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        ok1 = await client.PostAsJsonAsync("XoaGioCongSai?NgayCham=" + string.Format("{0:yyyy-MM-dd}", NgayLamViec), item);
                        if (ok1.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            HideLoading();
                            ListCapNhatGioCong.Remove(item);
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
                await new MessageBox("Thông báo", ex.Message).Show();
                return;
            }
        }

        public override async Task LoadData(object ojb)
        {
            try
            {
                if (IsBusy == true) return ;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình               
                var _json = Config.client.GetStringAsync(Config.URL + $"XemBaoCaoGioCong?ngayxem={string.Format("{0:yyyy-MM-dd}",NgayLamViec )}&userid={Preferences.Get(Config.User, "")}&loaixem={(theongay == true ? 0 : 1)}").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {
                    string result = _json.Substring(1, _json.Length - 2);
                    ListCapNhatGioCong = JsonConvert.DeserializeObject<ObservableCollection<CapNhatGioCong>>(result);
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

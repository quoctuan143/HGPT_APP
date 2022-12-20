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
    public class XemBaoCaoGiamSat_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"        
        int _heigthmaymoc = 0;
        public int HeightMayMoc { get => _heigthmaymoc; set => SetProperty(ref _heigthmaymoc, value); }
        int _heightchiphi = 0;
        public int HeightChiPhi { get => _heightchiphi; set => SetProperty(ref _heightchiphi, value); }
        int _heigthnhansu = 0;
        public int HeightNhanSu { get => _heigthnhansu; set => SetProperty(ref _heigthnhansu, value); }
        bool _isthoitiet = false;
        public bool IsThoiTiet { get => _isthoitiet  ; set => SetProperty(ref _isthoitiet ,value); }
        bool _isnhansu = false;
        public bool IsNhanSu { get => _isnhansu; set => SetProperty(ref _isnhansu, value); }
        bool _ismaymoc = false;
        public bool IsMayMoc { get => _ismaymoc; set => SetProperty(ref _ismaymoc, value); }
        bool _ischiphi = false;
        public bool IsChiPhi { get => _ischiphi; set => SetProperty(ref _ischiphi, value); }
        bool _istomtat = false;
        public bool IsTomTat { get => _istomtat; set => SetProperty(ref _istomtat, value); }
        bool _ishinhanh = false;
        public bool IsHinhAnh { get => _ishinhanh; set => SetProperty(ref _ishinhanh, value); }
        public INavigation navigation { get; set; }
        string _noidungbaocao;
        public string NoiDungBaoCao { get => _noidungbaocao; set => SetProperty(ref _noidungbaocao, value); }
        string _congviecngaymai;
        public string CongViecNgayMai { get => _congviecngaymai; set => SetProperty(ref _congviecngaymai, value); }  
        DateTime _ngaylamviec;
        public DateTime NgayLamViec
        {
            get => _ngaylamviec;
            set
            {
                SetProperty(ref _ngaylamviec, value);
                XemBaoCao(MaCongTrinh, value);
            }
        }

         string   MaCongTrinh { get; set; }
        BaoCaoGiamSat_Model _bao_cao_giam_sat;
        public BaoCaoGiamSat_Model ListBaoCaoGiamSat
        {
            get => _bao_cao_giam_sat;
            set
            {
                if (value != null)
                {
                    _bao_cao_giam_sat = value;
                    OnPropertyChanged("ListBaoCaoGiamSat");
                }
            }
        }        
        #endregion

        #region "Commnad"              
       
        #endregion

        public XemBaoCaoGiamSat_ViewModel(string maCongTrinh,DateTime ngay)
        {
            try
            {
                MaCongTrinh = maCongTrinh;
                NgayLamViec = ngay;                
                Title = "Chi tiết báo cáo công việc";                
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }          
        }
      async  void XemBaoCao(string macongtrinh, DateTime ngaybaocao)
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình
                var _json = Config.client.GetStringAsync(Config.URL + "XemBaoCaoGiamSat?macongtrinh=" + macongtrinh + "&ngayxem=" + string.Format("{0:yyyy-MM-dd}",ngaybaocao )).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "")
                {
                    string result = _json.Substring(1, _json.Length - 2);
                    ListBaoCaoGiamSat = JsonConvert.DeserializeObject<BaoCaoGiamSat_Model>(result);
                    OnPropertyChanged("ListBaoCaoGiamSat");
                    if (ListBaoCaoGiamSat.ListTomTatCongViec.Count > 0)
                    {
                        IsTomTat = true;
                        NoiDungBaoCao  = ListBaoCaoGiamSat.ListTomTatCongViec[0].Description.Replace("&#10;",Environment.NewLine);
                        CongViecNgayMai = ListBaoCaoGiamSat.ListTomTatCongViec[0].CongViecNgayMai.Replace("&#10;", Environment.NewLine); 
                    }
                    else
                    {
                        IsTomTat = false ;
                        NoiDungBaoCao = "";
                        CongViecNgayMai = "";
                    }
                    if (ListBaoCaoGiamSat.ListImage.Count > 0)                   
                        IsHinhAnh = true;
                    else IsHinhAnh = false ;

                    if (ListBaoCaoGiamSat.ListMayMocThietBi.Count > 0)
                    {
                        IsMayMoc = true;
                        HeightMayMoc = 45 * ListBaoCaoGiamSat.ListMayMocThietBi.Count + 45;
                    }                        
                    else IsMayMoc = false;

                    if (ListBaoCaoGiamSat.ListNhanLuc.Count > 0)
                    {
                        IsNhanSu = true;
                        HeightNhanSu = 45 * ListBaoCaoGiamSat.ListNhanLuc.Count + 45;
                    }
                       
                    else IsNhanSu = false;
                    if (ListBaoCaoGiamSat.ListChiPhiKhac.Count > 0)
                    {
                        IsChiPhi = true;
                        HeightChiPhi = 45 * ListBaoCaoGiamSat.ListChiPhiKhac.Count + 45;
                    }

                    else IsChiPhi = false;

                    if (ListBaoCaoGiamSat.ListThoiTiet.Count > 0)
                        IsThoiTiet = true;
                    else IsThoiTiet = false;

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

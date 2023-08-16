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
  public class BaoCaoGiamSat_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"      
        int _heigthmaymoc = 0;
        public int HeightMayMoc { get => _heigthmaymoc; set => SetProperty(ref _heigthmaymoc, value); }
        int _heigthnhansu = 0;
        public int HeightNhanSu { get => _heigthnhansu; set => SetProperty(ref _heigthnhansu, value); }
        int _heigthchiphi = 0;
        public int HeightChiPhi { get => _heigthchiphi; set => SetProperty(ref _heigthchiphi, value); }
        public INavigation navigation { get; set; }
        DateTime _ngaylamviec;
        public DateTime NgayLamViec { get => _ngaylamviec ; set => SetProperty (ref _ngaylamviec ,value ); }
        string _noidungbaocao ="";
        public string NoiDungBaoCao { get => _noidungbaocao ; set => SetProperty (ref _noidungbaocao ,value); }
        string _congviecngaymai ="";
        public string CongViecNgayMai { get => _congviecngaymai ; set => SetProperty(ref _congviecngaymai, value); }        
        DanhSachCongTrinh _selectCongTrinh;
        public DanhSachCongTrinh SelectCongTrinh { get => _selectCongTrinh; set => SetProperty(ref _selectCongTrinh, value); }
        ObservableCollection<FileAttach> _imageList;
        public ObservableCollection<FileAttach > ImageList { get => _imageList; set => SetProperty(ref _imageList, value); }
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

        ObservableCollection < ChiPhiKhac> _chiphikhac;
        public ObservableCollection<ChiPhiKhac> ListChiPhiKhac
        {
            get => _chiphikhac; 
            set
            {
                SetProperty(ref _chiphikhac, value);                
            }
        }
        ObservableCollection<MayMocThietBi> _maymocthietbi;
        public ObservableCollection<MayMocThietBi> ListMayMocThietBi
        {
            get => _maymocthietbi;
            set
            {
                SetProperty(ref _maymocthietbi, value);
            }
        }

        #endregion

        #region "Commnad"
        public Command LoadCongDoanBaoCao { get; set; }
        public Command GuiBaoCao { get; set; }
        public Command SelectImage { get; }
        public Command RemoveCommand { get; }
        public Command AddCommand { get; } 
        public Command AddThietBiCommand { get; }
        public Command DeleteThietBiCommand { get; }
        #endregion

        public BaoCaoGiamSat_ViewModel()
        {
            try
            {
                NgayLamViec = DateTime.Now.Date;                
                Title = "Báo cáo công việc";               
                LoadCongDoanBaoCao = new Command(async () => await LoadCongDoanBaoCaoExcute());
                ListMayMocThietBi = new ObservableCollection<MayMocThietBi>();
                ListChiPhiKhac = new ObservableCollection<ChiPhiKhac>();
                GuiBaoCao = new Command(async () => await GuiBaoCaoClick());
                SelectImage = new Command(async () => await SelectImageAsync());
                RemoveCommand = new Command(DeleteNhanSuClick);
                AddCommand = new Command(AddChiPhi);
                AddThietBiCommand = new Command(AddThietBi);
                DeleteThietBiCommand = new Command(DeleteThietBi);
                ImageList = new ObservableCollection<FileAttach>();               
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }

        private async void DeleteThietBi(object obj)
        {
            try
            {
                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn xóa thiết bị này không?").Show();
                if (ok == DialogReturn.OK)
                {
                    MayMocThietBi e = obj as MayMocThietBi;
                    ListMayMocThietBi.Remove(e);
                    HeightMayMoc = 45 + ListMayMocThietBi.Count * 45;
                }

            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
                return;
            }
        }

        private async void AddThietBi(object obj)
        {
            try
            {
                ListMayMocThietBi.Add(new MayMocThietBi());
                HeightMayMoc = 45 + ListMayMocThietBi.Count * 45;
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
                return;
            }

            
        }

        private async void AddChiPhi(object obj)
        {
            try
            {
                ListChiPhiKhac.Add(new ChiPhiKhac());
                HeightChiPhi = 45 + ListChiPhiKhac.Count * 45;
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
                return;
            }
            
        }

        private async void DeleteNhanSuClick(object obj)
        {
            try
            {
                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn xóa chi phí này không?").Show();
                if (ok == DialogReturn.OK)
                {
                    ChiPhiKhac  e = obj as ChiPhiKhac;
                    ListChiPhiKhac .Remove(e);
                    HeightChiPhi = 45 + ListChiPhiKhac.Count * 45;
                }

            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
                return;
            }
        }
        private async Task SelectImageAsync()
        {
            var cts = new CancellationTokenSource();
            IMediaFile[] files = null;

            try
            {
                var request = new MediaPickRequest(3, MediaFileType.Image)
                {
                    PresentationSourceBounds = System.Drawing.Rectangle.Empty,
                    UseCreateChooser = true,
                    Title = "Select"
                };                
                cts.CancelAfter(TimeSpan.FromMinutes(5));
                var results = await MediaGallery.PickAsync(request, cts.Token);
                if (results.Files  == null) return;
                ImageList.Clear();
                foreach (var file in results.Files)
                {
                    byte[] buf;  // byte array
                    Stream stream = await file.OpenReadAsync();  //initialise new stream                  
                    buf = new byte[stream.Length];  //declare arraysize
                    stream.Read(buf, 0, buf.Length); // read from stream to byte array
                    ImageList.Add(new FileAttach { FileName = file.NameWithoutExtension, data =  ImageSource.FromStream( () =>  new MemoryStream(buf)), dataByte = buf  }); //new FileAttach { , data = buf }
                    file.Dispose();
                }
                OnPropertyChanged("ImageList");
            }
            catch (OperationCanceledException)
            {
                // handling a cancellation request
            }
            catch (Exception)
            {
                // handling other exceptions
            }
            finally
            {
                cts.Dispose();
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

                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn gửi báo cáo không").Show();
                if (ok == DialogReturn.OK )
                {
                    List<BaoCaoGiamSat_Insert> Insert = new List<BaoCaoGiamSat_Insert>();
                    foreach (ThoiTiet _thoitiet  in ListBaoCaoGiamSat.ListThoiTiet)
                    {
                        Insert.Add(new BaoCaoGiamSat_Insert
                        {
                            Code = _thoitiet.Code,
                            Weather = _thoitiet.Sunny == 1 ? 1 : 0,
                            PostingDate = NgayLamViec,
                            UserID = Preferences.Get(Config.User, ""),
                            CongTrinh = _selectCongTrinh.Code ,
                            Amount = 0,
                            CongViecNgayMai ="",
                            Description = "",                        
                            Quantity =0,UnitPrice=0
                        });
                    }
                    foreach (NhanLuc _nhanluc in ListBaoCaoGiamSat.ListNhanLuc)
                    {
                        if (_nhanluc.Quantity > 0)
                        {
                            Insert.Add(new BaoCaoGiamSat_Insert
                            {
                                Code = _nhanluc.Code,
                                Quantity = _nhanluc.Quantity,
                                PostingDate = NgayLamViec,
                                UnitPrice = _nhanluc.UnitPrice,
                                UserID = Preferences.Get(Config.User, ""),
                                Amount = _nhanluc.Quantity * _nhanluc.UnitPrice,
                                CongTrinh = _selectCongTrinh.Code,                            
                                Description="",
                                CongViecNgayMai ="",
                                Weather = 0
                            });
                        }    
                        
                    }
                    foreach (MayMocThietBi _nhanluc in ListMayMocThietBi )
                    {
                        if (_nhanluc.Quantity > 0)
                        {
                            Insert.Add(new BaoCaoGiamSat_Insert
                            {
                                Code = "11",
                                Quantity = _nhanluc.Quantity,
                                PostingDate = NgayLamViec,
                                UnitPrice = _nhanluc.UnitPrice,
                                UserID = Preferences.Get(Config.User, ""),
                                Amount = _nhanluc.Quantity * _nhanluc.UnitPrice,
                                CongTrinh = _selectCongTrinh.Code,
                                Weather = 0, CongViecNgayMai ="",
                                Description = _nhanluc.Description 
                              
                            });
                        }

                    }
                    foreach (ChiPhiKhac _nhanluc in ListChiPhiKhac)
                    {
                        if (_nhanluc.Amount > 0)
                        {
                            if (_nhanluc.NhomChiPhi == null )
                            {
                                await new MessageBox("Thông báo", "Vui lòng chọn nhóm chi phí").Show();
                                return;
                            }    
                            Insert.Add(new BaoCaoGiamSat_Insert
                            {
                                Code = "18",
                                Quantity = 0,
                                PostingDate = NgayLamViec,
                                UnitPrice = 0,
                                UserID = Preferences.Get(Config.User, ""),
                                Amount = _nhanluc.Amount ,
                                CongTrinh = _selectCongTrinh.Code,
                                Weather = 0,
                                CongViecNgayMai = "",
                                Description = _nhanluc.Description ,
                                NhomChiPhi = _nhanluc.NhomChiPhi 

                            });
                        }

                    }

                    Insert.Add(new BaoCaoGiamSat_Insert
                    {
                        Code = "15",
                        Quantity =0,
                        PostingDate = NgayLamViec,
                        UnitPrice = 0,
                        UserID = Preferences.Get(Config.User, ""),
                        Amount = 0,
                        Description = NoiDungBaoCao.Contains(Environment.NewLine) ? NoiDungBaoCao.Replace(Environment.NewLine, "&#10;") : NoiDungBaoCao,                   
                        CongViecNgayMai = CongViecNgayMai.Contains(Environment.NewLine ) ? CongViecNgayMai.Replace(Environment.NewLine, "&#10;") : CongViecNgayMai,                                            
                        CongTrinh = _selectCongTrinh.Code,
                        Weather = 0
                    });

                    ShowLoading("Đang xử lý vui lòng đợi");
                    await Task.Delay(1000);
                    HttpResponseMessage ok1 = null;
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        //ok1 = await client.PostAsJsonAsync("PostBaoCaoGiamSat", content );
                        //string a = JsonConvert.SerializeObject(Insert);
                        bool checkUploadFile = true;
                        foreach (FileAttach file in ImageList)
                        {
                            var content = new MultipartFormDataContent();                                                                                   
                            content.Add(  CreateFileContent(new MemoryStream(file.dataByte), file.FileName + ".png", "application/octet-stream"));
                                                    
                            var response = client.PostAsync("PostFileGiamSat?macongtrinh=" + _selectCongTrinh.Code + "&userid=" + Preferences.Get(Config.User, "") + "&ngaybaocao=" + string.Format("{0:yyyy-MM-dd}", NgayLamViec), content ).Result;
                            response.EnsureSuccessStatusCode();
                            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                            
                            {
                                HideLoading();
                                checkUploadFile = false;
                                await new MessageBox("Thông báo", response.Content.ReadAsStringAsync().Result).Show();
                                return;
                            }
                        }

                        if (checkUploadFile == true )
                        {
                            //gửi thông tin còn lại
                            ok1 = await client.PostAsJsonAsync("PostBaoCaoGiamSat", Insert);
                            if (ok1.StatusCode == System.Net.HttpStatusCode.OK)
                            {                                
                                HideLoading();
                                ShortAlert("Đã gửi báo cáo thành công");
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
            }
            catch (Exception ex)            
            {
                HideLoading();
                await new MessageBox("Thông báo",ex.Message ).Show();
                return;
            }
           
        }
        private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"file\"",
                FileName = "\"" + fileName + "\""
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
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
                GetCongTrinh("1");
                var _json = Config.client.GetStringAsync(Config.URL + "getCongDoanBaoBao").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "")
                {              
                    string result = _json.Substring(1, _json.Length - 2);
                    ListBaoCaoGiamSat = JsonConvert.DeserializeObject<BaoCaoGiamSat_Model>(result);                    
                    if (ListBaoCaoGiamSat.ListNhanLuc.Count > 0)
                    {                       
                        HeightNhanSu = 45 * ListBaoCaoGiamSat.ListNhanLuc.Count + 45;
                    }
                    
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

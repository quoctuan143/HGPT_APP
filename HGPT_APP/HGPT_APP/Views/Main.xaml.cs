using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Models.GiamSat;
using HGPT_APP.Popup;
using HGPT_APP.ViewModels;
using HGPT_APP.Views.GiamSat;
using HGPT_APP.Views.SinhNhatKhachHang;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using Plugin.LatestVersion;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        public ObservableCollection<FileAttach> ImageList { get; set; }
        BaseViewModel viewModel;
        Timer timer;
        int position = 0;
       public Command SelectDateCommand { get; set; }
        public Main()
        {
            InitializeComponent();
            try
            {
                txtName.Text = Preferences.Get(Config.FullName, "");
                viewModel = new BaseViewModel();
                ImageList = new ObservableCollection<FileAttach>
                {
                    new FileAttach { FileName = "https://hrm.hgpt.vn/image/hinh1.jpg" },
                    new FileAttach { FileName = "https://hrm.hgpt.vn/image/hinh2.jpg" },
                    new FileAttach { FileName = "https://hrm.hgpt.vn/image/hinh3.jpg" },
                    new FileAttach { FileName = "https://hrm.hgpt.vn/image/hinh4.jpg" }
                };
                SelectDateCommand = new Command(async () => 
                {
                    try
                    {
                        var asked = await new MessageYesNo("Thông báo", "Bạn có muốn chuyển kế hoạch sang công việc hiện tại không?").Show();
                        if (asked == DialogReturn.OK )
                        {
                            string url = "api/hgpt/TaoCongViecTuKeHoach?ngaytao=" + String.Format("{0:yyyy-MM-dd}", datepicker.Date) + "&nguoitao=" + Preferences.Get(Config.User, "");
                            var ok = await viewModel.RunHttpClientGet<object>(url);
                            if (ok.Status.IsSuccessStatusCode)
                                DependencyService.Get<IMessage>().LongAlert("Đã chuyển kế hoạch sang công việc cho ngày bạn chọn");
                            else
                                await new MessageBox("Thông báo", ok.Status.Content.ReadAsStringAsync().Result).Show();
                        }    
                    }
                    catch
                    {

                       
                    }
                   
                });
                if (Preferences.Get(Config.AnhDaiDien, "") != "")
                    AnhDaiDien.Source = ImageSource.FromUri(new Uri(Preferences.Get(Config.AnhDaiDien, "")));

                if (Preferences.Get(Config.IsPhanViec, "0") == "1")
                {
                    frmCongViec.IsVisible = true;
                }
                if (Preferences.Get(Config.IsThietBi, "0") == "1")
                {
                    frmThietBi.IsVisible = true;
                }
                if (Preferences.Get(Config.IsChamSocKhachHang, "0") == "1")
                {
                    frmchamsockhachhang.IsVisible = true;
                }
                if (Preferences.Get(Config.IsGiamSat, "0") == "1")
                {
                    frmGiamSat.IsVisible = true;
                }
                
                BindingContext = this;
                Task.Run( async () => 
                {
                    
                    var a = await viewModel.RunHttpClientGet<object>("api/qltb/getChamSocKhachHangChuaHoanThanh");
                    bagCSKH.BadgeText = a.Lists.Count.ToString();
                });
                timer = new Timer();
                timer.Interval = 2000;
                timer.Enabled = true;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
                CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened;
                Task.Run( ()=>  NewVersion());
            }
            catch (Exception ex)
            {

                new MessageBox("Thông báo", ex.Message).Show();
            }


        }
        private async void Current_OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs p)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                string loaiphieu;
                try
                {                    
                    if (p.Data["sochungtu"].ToString() != "")
                    {
                        string soChungTu = p.Data["sochungtu"].ToString();
                        loaiphieu = p.Data["loaiphieu"].ToString();
                        switch (loaiphieu)
                        {   
                            case "ThongBaoBaoTri":
                        await Navigation.PushAsync(new KeHoachBaoTriPage());
                                break;
                            case "ThongBaoPhanViec":
                        await Navigation.PushAsync(new Phan_Chia_Cong_Viec());
                                break;
                            case "LenhSanXuat":
                             await Navigation.PushAsync(new DanhSachLenhSanXuat());
                                break;
                            case "ThongBaoCongTy":
                                await Navigation.PushAsync(new NotificationPage());
                                break;
                            case "ThongBaoGiamSat":
                        await Navigation.PushAsync(new XemBaoCaoGiamSat_Page(soChungTu, Convert.ToDateTime( p.Data["date"])));
                                break;
                            case "sinhnhatkhachhang" :
                        await Navigation.PushAsync(new SinhNhatKhachHang_ChuaXuLy());
                                break;
                        }                        
                    }
                }
                catch
                {
                }
            });
        }

        private async void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs p)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                string loaiphieu;
                try
                {
                    if (p.Data["sochungtu"].ToString() != "")
                    {
                        string soChungTu = p.Data["sochungtu"].ToString();
                        loaiphieu = p.Data["loaiphieu"].ToString();
                        switch (loaiphieu)
                        {
                            case "ThongBaoBaoTri":
                                await Navigation.PushAsync(new KeHoachBaoTriPage());
                                break;
                            case "ThongBaoPhanViec":
                                await Navigation.PushAsync(new Phan_Chia_Cong_Viec());
                                break;
                            case "LenhSanXuat":
                                await Navigation.PushAsync(new DanhSachLenhSanXuat());
                  
                                break;
                            case "ThongBaoCongTy":
                                await new MessageBox("Thông báo", p.Data["body"].ToString()).Show();
                                break;
                            case "ThongBaoGiamSat":
                                await Navigation.PushAsync(new XemBaoCaoGiamSat_Page(soChungTu, Convert.ToDateTime(p.Data ["date"])));
                                break;
                            case "sinhnhatkhachhang":
                                await Navigation.PushAsync(new SinhNhatKhachHang_ChuaXuLy());
                                break;
                        }
                    }
                }
                catch
                {
                }
            });

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                position += 1;
                viewImage.Position = position;
                if (viewImage.Position > 3)
                {
                    position = 0;
                    viewImage.Position = position;
                }
            });

        }
        MediaFile media;
        //check version
        async void NewVersion()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var version = Config.client.GetStringAsync("https://play.google.com/store/apps/details?id=com.companyname.hgpt_app").Result;
                if (version.Contains(AppInfo.VersionString) == false)
                {
                    var update = await new MessageYesNo("New Version", "Có phiên bản mới trên app store. Bạn có muốn cập nhật không").Show();
                    if (update == DialogReturn.OK)
                    {
                        await CrossLatestVersion.Current.OpenAppInStore();
                    }
                }
            }
            else
            {
                bool isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();
                if (!isLatest)
                {
                    var update = await new MessageYesNo("New Version", "Có phiên bản mới trên app store. Bạn có muốn cập nhật không").Show();
                    if (update == DialogReturn.OK)
                    {
                        await CrossLatestVersion.Current.OpenAppInStore();
                    }
                }
            }
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new DanhSachLenhSanXuat());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }

        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Lich_Su_Phan_Chia_Cong_Viec());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }

        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Phan_Chia_Cong_Viec());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }

        }

        private async void hinhdaidien_Tapped(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await new MessageBox("thông báo", "Điện thoại không hỗ trợ chức năng chụp hình").Show();
                    return;
                }
                media = await CrossMedia.Current.PickPhotoAsync();
                if (media == null) return;
                AnhDaiDien.Source = ImageSource.FromStream(() =>
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var response = client.PostAsync("api/hgpt/PostFileUpload?username=" + Preferences.Get(Config.User, ""), content).Result;
                        string filename = media.Path.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                        Preferences.Set(Config.AnhDaiDien, "http://hrm.hgpt.vn/image/" + filename);
                        client.Dispose();
                    }

                    return media.GetStream();
                });
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
            }

        }



        private async void btnThoat_Clicked(object sender, EventArgs e)
        {
            var ok = await new MessageYesNo("Thông báo", "Bạn có muốn thoát không?").Show();
            if (ok == DialogReturn.OK)
                Application.Current.MainPage = new Login();
        }

        private async void btndanhsachthietbi_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Danh_Muc_Thiet_Bi());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnLichXich_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new KeHoachBaoTriPage());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnLichSuThietBi_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new LichSuBaoTri());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnCaiDat_Clicked(object sender, EventArgs e)
        {
            try
            {
                await new MessageChangePassword().Show();
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnthongbao_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getNotification?token=" + Preferences.Get(Config.Token, "1")).Result;

                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        ObservableCollection<NotifycationModel> ListThongBao = JsonConvert.DeserializeObject<ObservableCollection<NotifycationModel>>(result);
                        txtThongBao.BadgeText = ListThongBao.Where(p => p.Viewed == 0).ToList().Count().ToString();                        
                    }

                    var a = await viewModel.RunHttpClientGet<DanhSachCongTrinhHoatDongTrongNgay_Model>("DanhSachCongTrinhHoatDongTrongNgay?ngay=" + string.Format("{0:yyyy-MM-dd}",  DateTime.Now.Date));
                    int XembaoCaoHangNgay = a.Lists.Count;
                    bagXembaoCaoHangNgay.BadgeText = XembaoCaoHangNgay > 0 ? XembaoCaoHangNgay.ToString() : "";
                    //ImgXembaoCaoHangNgay.WidthRequest = XembaoCaoHangNgay > 0 ? 60 : 65;
                    //ImgXembaoCaoHangNgay.HeightRequest = XembaoCaoHangNgay > 0 ? 60 : 65;
                }
                catch
                {

                }


            }
            using (HttpClient client = new HttpClient())
            {
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getChamSocKhachHangChuaHoanThanh").Result;

                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    //bagesinhnhatkhachhang.BadgeText  = JsonConvert.DeserializeObject<ObservableCollection<ChamSocKhachHang>>(result).Count.ToString();
                }

            }
        }

        private void btnsinhnhatkhachhangchuachamsoc_Tapped(object sender, EventArgs e)
        {

            try
            {
                //await Navigation.PushAsync(new KeHoachBaoTriPage());
                Navigation.PushAsync(new SinhNhatKhachHang_ChuaXuLy());
            }
            catch (Exception ex)
            {

                new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnHoanThanhChamSocKhachHang_Tapped(object sender, EventArgs e)
        {

            try
            {
                await Navigation.PushAsync(new SinhNhatKhachHang_DaChamSoc());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            BackButtonPressed();
            return true;
        }
        public async Task BackButtonPressed()
        {
            var ok = await new MessageYesNo("Thông báo", "Bạn có muốn thoát chương trình không?").Show();
            if (ok == DialogReturn.OK)
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }

        private async void barcode_Tapped(object sender, EventArgs e)
        {
            await new Barcode("dfsfsdfdf").Show();
        }

        private async void btnBaoCaoGiamSat_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new BaoCaoGiamSat_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnChamCongNhanSu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ChamCongNhanSu_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnXembaoCaoHangNgay_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new DanhSachCongTrinhHoatDongTrongNgay_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private void btnThietLapNhanSu_Tapped(object sender, EventArgs e)
        {

        }

        private async void btnThietLapNhanSu_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ThemNhanSuChoGiamSat_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnXemCongHangNgay_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new XemChamCongNhanSu_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnBaoCaoQuanLy_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new BaoCaoTongHopChiPhi_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnTienDoLapDat_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new TienDoLapDat_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnTongHopBaoCaoTienDo_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new DanhSachCongTrinhDuocLapDatTrongNgay_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnDanhSachCongTrinh_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new DanhSachCongTrinh_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private async void btnTheoDoiGiamSat_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new TheoDoiGiamSat_Page());
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show(); ;
            }
        }

        private void btnPhanViecTuKeHoach_Tapped(object sender, EventArgs e)
        {
            datepicker.IsOpen = true;
        }
    }
}
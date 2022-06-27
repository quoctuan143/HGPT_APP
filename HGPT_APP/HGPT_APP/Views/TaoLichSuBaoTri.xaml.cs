using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaoLichSuBaoTri : ContentPage
    {
        bool _isrunning;
        public Boolean IsRunning { get => _isrunning; set { _isrunning = value; OnPropertyChanged("IsRunning"); } }
        ObservableCollection<QUY_TRINH_BAO_TRI> _quytrinh;
        public ObservableCollection<QUY_TRINH_BAO_TRI> QUY_TRINH_BAO_TRIs { get => _quytrinh; set { _quytrinh = value; OnPropertyChanged("QUY_TRINH_BAO_TRIs"); } }
        public KeHoachBaoTri Item { get; set; }
        LICH_SU_BAO_TRI lsu;
        MediaFile media;
        public TaoLichSuBaoTri(KeHoachBaoTri item)
        {
            InitializeComponent();

            Item = item;
            QUY_TRINH_BAO_TRIs = new ObservableCollection<QUY_TRINH_BAO_TRI>();
            BindingContext = this;
            //listquytrinh.QueryRowHeight += Listquytrinh_QueryRowHeight;
        }

        private void Listquytrinh_QueryRowHeight(object sender, Syncfusion.SfDataGrid.XForms.QueryRowHeightEventArgs e)
        {
            try
            {
                if (e.RowIndex != 0)
                {
                    //Calculates and sets height of the row based on its content 
                    e.Height = listquytrinh.GetRowHeight(e.RowIndex);
                    e.Handled = true;
                }
            }
            catch (Exception)
            {


            }
        }

        async void Save_Clicked(object sender, EventArgs e)

        {
            try
            {
                if (string.IsNullOrEmpty(cbNhaMay.Text))

                {
                    await new MessageBox("Thông báo", "Chọn nhà máy").Show();
                    return;
                }

                if (string.IsNullOrEmpty(entryTinhTrang.Text))

                {
                    await new MessageBox("Thông báo", "Bạn nhập tình trạng sau khi sửa chữa").Show();
                    return;
                }

                string noidung = "";
                foreach (QUY_TRINH_BAO_TRI btri in QUY_TRINH_BAO_TRIs)
                {
                    if (btri.IsCheck == true)
                    {
                        if (noidung == "")
                        {
                            noidung = btri.Description;
                        }
                        else
                        {
                            noidung += "||" + btri.Description;
                        }
                    }


                }
                if (!string.IsNullOrEmpty(entryNoiDungKhac.Text))
                {
                    noidung += "||" + entryNoiDungKhac.Text;
                }

                var result = await new MessageYesNo("Thông báo", "Bạn có muốn cập nhật lần bảo trì này không?").Show();
                if (result == Global.DialogReturn.OK)
                {


                    lsu = new LICH_SU_BAO_TRI();
                    lsu.NHOM_BAO_TRI = Item.Code;
                    lsu.MA_THIET_BI = Item.No_;
                    lsu.MA_PHU_TUNG = entryPhuTung.Text == string.Empty ? "" : entryPhuTung.Text;
                    lsu.BAO_DUONG_DINH_KY = Convert.ToInt32(chkBaoDuong.IsChecked);
                    if (media != null)
                    {
                        var filename = media.Path.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                        lsu.IMAGE_LINK = "http://hrm.hgpt.vn/image/" + filename;
                    }
                    else
                    {
                        lsu.IMAGE_LINK = "";
                    }

                    lsu.MA_NHA_MAY = cbNhaMay.Text;
                    lsu.MA_XUONG = Preferences.Get(Config.MaXuong, "");
                    lsu.NGAY_GIO = DateTime.Now;
                    lsu.NGUOI_THUC_HIEN = Preferences.Get(Config.User, "");
                    lsu.NOI_DUNG = noidung;
                    lsu.TINH_TRANG_HIEN_TAI = entryTinhTrang.Text;
                    lsu.STATUS = 1;
                    lsu.NGUOI_XAC_NHAN = Preferences.Get(Config.User, ""); ;
                    lsu.THANG = Item.Thang;




                    //nếu up hình thành công thì up lich sử lên luôn
                    if (media != null)
                    {
                        var content = new MultipartFormDataContent();
                        content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(Config.URL);
                        var response = client.PostAsync("api/qltb/PostLichSuBaoTri_Picture", content).Result;
                        if (response.Content.ReadAsStringAsync().Result.ToLower().Contains("upload thành công"))
                        {
                            var ok = client.PostAsJsonAsync("api/qltb/postLichSuBaoTri", lsu);
                            if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("cập nhật thành công"))
                            {

                                DependencyService.Get<IMessage>().ShortAlert("Cập nhật thành công");
                                MessagingCenter.Send(this, "AddLichSuBaoTri", lsu);
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                            }
                        }
                        else
                        {
                            await new MessageBox("Thông báo", response.Content.ReadAsStringAsync().Result).Show();
                            return;
                        }
                    }
                    else
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(Config.URL);
                        var ok = client.PostAsJsonAsync("api/qltb/postLichSuBaoTri", lsu);
                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("cập nhật thành công"))
                        {

                            DependencyService.Get<IMessage>().ShortAlert("Cập nhật thành công");
                            MessagingCenter.Send(this, "AddLichSuBaoTri", lsu);
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
            }



        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
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
                imagePicture.Source = ImageSource.FromStream(() =>
                {
                    return media.GetStream();
                });
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông Báo", ex.Message).Show();
            }

        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getQuyTrinhBaoTri_ChiTiet?mathietbi=" + Item.No_ + "&nhom=" + Item.ItemCategoryCode + "&mabaotri=" + Item.Code).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    QUY_TRINH_BAO_TRIs = JsonConvert.DeserializeObject<ObservableCollection<QUY_TRINH_BAO_TRI>>(result);
                }
            }
            catch (Exception ex)
            {

                new MessageBox("Thông Báo", ex.Message).Show();
            }

        }
    }
}
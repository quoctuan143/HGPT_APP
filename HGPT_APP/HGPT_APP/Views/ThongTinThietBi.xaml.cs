using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Popup;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HGPT_APP.ViewModels;
using HGPT_APP.Models;
using Xamarin.Essentials;
using Plugin.Media.Abstractions;
using Syncfusion.SfDataGrid.XForms;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinThietBi : ContentPage
    {
        Thong_Tin_Thiet_Bi_ViewModel viewModel;
        MediaFile media;
        public ThongTinThietBi(DanhMuc_ThietBi item)
        {
            InitializeComponent();
            listThietBi.QueryRowHeight += ListThietBi_QueryRowHeight;
            BindingContext = viewModel = new Thong_Tin_Thiet_Bi_ViewModel(item);

        }

        private void ListThietBi_QueryRowHeight(object sender, Syncfusion.SfDataGrid.XForms.QueryRowHeightEventArgs e)
        {
            if (e.RowIndex != 0)
            {
                //Calculates and sets height of the row based on its content 
                e.Height = listThietBi.GetRowHeight(e.RowIndex);
                e.Handled = true;
            }
        }

        private void SfTabView_TabItemTapped(object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {
            try
            {
                if (e.TabItem.Title == "Lịch Sử")
                {
                    if (viewModel.lICH_SU_BAO_TRIs.Count == 0)
                        viewModel.LoadLichSuBaoTri.Execute(viewModel.Item.No_);
                }
                if (e.TabItem.Title == "Quy Trình")
                {
                    if (viewModel.QUY_TRINH_BAO_TRIs.Count == 0)
                        viewModel.LoadQuyTrinhBaoTri.Execute(viewModel.Item.No_);
                }
                if (e.TabItem.Title == "Ghi Chú")
                {
                    if (viewModel.ListPhuTung.Count == 0)
                        viewModel.LoadPhuTUng.Execute(viewModel.Item.No_);
                }
                if (e.TabItem.Title == "KH Bảo Trì")
                {
                    if (viewModel.KE_HOACH_BAO_TRIs.Count == 0)
                        viewModel.LoadKeHoachBaoTri.Execute(DateTime.Now.Year.ToString());
                }
            }
            catch (Exception)
            {


            }

        }

        //private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    if (viewModel.Item.LinkVideo == "" || viewModel.Item.LinkVideo == null)
        //    {
        //        DependencyService.Get<IMessage>().ShortAlert("Không có link video");

        //        return;
        //    }

        //    await Browser.OpenAsync("https://www.youtube.com/embed/" + viewModel.Item.LinkVideo);
        //}

        private async void addSuCo_Clicked(object sender, EventArgs e)
        {
            try
            {
                KeHoachBaoTri item = listKeHoach.SelectedItem as KeHoachBaoTri;
                if (item != null)
                {
                    await Navigation.PushAsync(new TaoLichSuBaoTri(item));
                }
                else
                {
                    KeHoachBaoTri item1 = new Models.KeHoachBaoTri
                    {
                        Code = "HN",
                        LoaiBaoTri = "HN",
                        Nam = DateTime.Now.Year,
                        Thang = DateTime.Now.Month,
                        NameVN = viewModel.Item.NameVN,
                        No_ = viewModel.Item.No_,
                        No_2 = viewModel.Item.No_2,
                        No_3 = viewModel.Item.No_3,
                        Da_Bao_Tri = false,
                        ItemCategoryCode = viewModel.Item.ItemCategory

                    };
                    await Navigation.PushAsync(new TaoLichSuBaoTri(item1));
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
            }

        }

        private async void uploadImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (media != null)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Đang upload hình vui lòng đợi");
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(media.GetStream()), "\"file\"", $"\"{media.Path}\"");
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Config.URL);
                    var response = client.PostAsync("api/qltb/PostFileUpload?mathietbi=" + viewModel.Item.No_, content).Result;

                    await new MessageBox("Thông báo", response.Content.ReadAsStringAsync().Result).Show();
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("chưa có hình nào để upload");

                }


            }
            catch (Exception ex)
            {


            }


        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                DependencyService.Get<IMessage>().ShortAlert("Điện thoại không hỗ trợ chức năng chụp hình");

                return;
            }
            media = await CrossMedia.Current.PickPhotoAsync();           
            //media = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { SaveToAlbum = true });
            if (media == null) return;
            imagePicture.Source = ImageSource.FromStream(() => 
            {
                return media.GetStream();
            });
        }

        private void Edit_Clicked(object sender, EventArgs e)
        {

        }



        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            if (viewModel.Item != null)
            {
                Navigation.PushAsync(new ThongTinThietBi_Edit(viewModel.Item));
            }
        }

        private async  void Button_Clicked(object sender, EventArgs e)
        {
            await new Nhap_Phu_Tung(viewModel.Item.No_,"","new",0).Show();
        }

        private async void btnSuaGhiChu_Clicked(object sender, EventArgs e)
        {
            if (viewModel.SelectPhuTung  != null)
            {
                await new Nhap_Phu_Tung(viewModel.Item.No_, viewModel.SelectPhuTung.Description , "edit", viewModel.SelectPhuTung.RowID ).Show();
            }
           
        }
    }
}
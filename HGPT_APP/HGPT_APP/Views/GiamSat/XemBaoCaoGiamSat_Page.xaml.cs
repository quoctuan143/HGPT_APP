using HGPT_APP.Interface;
using HGPT_APP.ViewModels.GiamSat;
using ImageFromXamarinUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.GiamSat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XemBaoCaoGiamSat_Page : ContentPage
    {
        XemBaoCaoGiamSat_ViewModel viewModel;
        string MaCongTrinh;
        DateTime Ngay;
        public XemBaoCaoGiamSat_Page(string maCongTrinh , DateTime ngay)
        {
            InitializeComponent();
            MaCongTrinh = maCongTrinh;
            Ngay = ngay;
            viewModel = new XemBaoCaoGiamSat_ViewModel(maCongTrinh,ngay);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();              
        }

        private async  void btnShareZalo_Clicked(object sender, EventArgs e)
        {
            //try
            //{
            //    await Browser.OpenAsync($"https://hrm.hgpt.vn/Home/BaoCaoCongTrinh?CongTrinh={MaCongTrinh}&Ngay={string.Format("{0:yyyy - MM - dd}", Ngay)}", BrowserLaunchMode.SystemPreferred);
            //}
            //catch (Exception ex)
            //{
            //    // An unexpected error occured. No browser may be installed on the device.
            //}
            
            await Share.RequestAsync(new ShareTextRequest
            {
                Title = "Báo cáo công việc",
                Text = "Báo cáo công việc",
                Uri = $"https://hrm.hgpt.vn/Home/BaoCaoCongTrinh?CongTrinh={MaCongTrinh}&Ngay={string.Format("{0:yyyy-MM-dd}", Ngay)}"
            });
        }
    }
}
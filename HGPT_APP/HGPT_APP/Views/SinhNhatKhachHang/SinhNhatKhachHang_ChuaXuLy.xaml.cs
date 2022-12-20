using HGPT_APP.Models;
using HGPT_APP.ViewModels.SinhNhatKhachHang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.SinhNhatKhachHang
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SinhNhatKhachHang_ChuaXuLy : ContentPage
    {
        SinhNhatKhachHang_ChuaXuLyViewModel viewModel;
        public SinhNhatKhachHang_ChuaXuLy()
        {
            InitializeComponent();
            BindingContext = viewModel = new SinhNhatKhachHang_ChuaXuLyViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListDanhSach.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ChamSocKhachHang cskh = listChuaXuLy.SelectedItem as ChamSocKhachHang;
            Navigation.PushAsync(new NoiDungChamSocKhachHang(cskh));
        }
    }
}
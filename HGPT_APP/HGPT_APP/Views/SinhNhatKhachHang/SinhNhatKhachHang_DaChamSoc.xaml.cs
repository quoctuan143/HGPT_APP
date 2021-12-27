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
    public partial class SinhNhatKhachHang_DaChamSoc : ContentPage
    {
        SinhNhatKhachHang_DaXuLyViewModel viewModel;
        public SinhNhatKhachHang_DaChamSoc()
        {
            InitializeComponent();
            BindingContext = viewModel = new SinhNhatKhachHang_DaXuLyViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListDanhSach.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
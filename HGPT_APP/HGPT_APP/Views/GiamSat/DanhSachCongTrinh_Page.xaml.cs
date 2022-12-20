using HGPT_APP.Global;
using HGPT_APP.Models.GiamSat;
using HGPT_APP.ViewModels.GiamSat;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.GiamSat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DanhSachCongTrinh_Page : ContentPage
    {
        DanhSachCongTrinh_ViewModel viewModel;        
        public DanhSachCongTrinh_Page()    
        {
            InitializeComponent();           
            viewModel = new DanhSachCongTrinh_ViewModel();           
            BindingContext = viewModel;
            listCongDoanCongNhan.QueryRowHeight += ListCongDoanCongNhan_QueryRowHeight;
        }

        private void ListCongDoanCongNhan_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (e.RowIndex != 0)
            {
                //Calculates and sets height of the row based on its content 
                e.Height = listCongDoanCongNhan.GetRowHeight(e.RowIndex);
                e.Handled = true;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if ("linhtc,dunghd,tuannq1,hiennk".Contains(Preferences.Get(Config.User, "")))
            {
                btnStopCongTrinh.IsVisible = true;
            }
            if (viewModel.IsBusy == true) return;            
            
        }
    }    
}
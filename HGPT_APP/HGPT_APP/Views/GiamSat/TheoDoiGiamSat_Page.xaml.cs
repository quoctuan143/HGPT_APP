using HGPT_APP.Models.GiamSat;
using HGPT_APP.ViewModels.GiamSat;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.GiamSat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TheoDoiGiamSat_Page : ContentPage
    {
        TheoDoiGiamSat_ViewModel viewModel;
        public TheoDoiGiamSat_Page()
        {
            InitializeComponent();
            viewModel = new TheoDoiGiamSat_ViewModel();
            viewModel.navigation = Navigation;
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

        private void pickerButton_Clicked(object sender, EventArgs e)
        {
            datepicker.IsOpen = true;
        }
    }
}
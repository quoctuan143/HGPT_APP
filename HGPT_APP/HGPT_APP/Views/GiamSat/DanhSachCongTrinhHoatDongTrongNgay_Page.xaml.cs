using HGPT_APP.Models.GiamSat;
using HGPT_APP.ViewModels.GiamSat;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.GiamSat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DanhSachCongTrinhHoatDongTrongNgay_Page : ContentPage
    {
        DanhSachCongTrinhHoatDongTrongNgay_ViewModel viewModel;        
        public DanhSachCongTrinhHoatDongTrongNgay_Page()   
        {
            InitializeComponent();           
            viewModel = new DanhSachCongTrinhHoatDongTrongNgay_ViewModel();
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

        private void listCongDoanCongNhan_SelectionChanging(object sender, GridSelectionChangingEventArgs e)
        {
            DanhSachCongTrinhHoatDongTrongNgay_Model item = listCongDoanCongNhan.SelectedItem as DanhSachCongTrinhHoatDongTrongNgay_Model;
            if (item != null)
            Navigation.PushAsync(new XemBaoCaoGiamSat_Page(item.CongTrinh, item.PostingDate));
        }

        private void listCongDoanCongNhan_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            DanhSachCongTrinhHoatDongTrongNgay_Model item = listCongDoanCongNhan.SelectedItem as DanhSachCongTrinhHoatDongTrongNgay_Model;
            if (item != null)
                Navigation.PushAsync(new XemBaoCaoGiamSat_Page(item.CongTrinh, item.PostingDate));

        }
    }    
}
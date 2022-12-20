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
    public partial class DanhSachCongTrinhDuocLapDatTrongNgay_Page : ContentPage
    {
        DanhSachCongTrinhDuocLapDatTrongNgay_ViewModel viewModel;        
        public DanhSachCongTrinhDuocLapDatTrongNgay_Page()    
        {
            InitializeComponent();           
            viewModel = new DanhSachCongTrinhDuocLapDatTrongNgay_ViewModel();
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
        

        private void listCongDoanCongNhan_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            DanhSachCongTrinhDuocLapDatTrongNgay_Model item = listCongDoanCongNhan.SelectedItem as DanhSachCongTrinhDuocLapDatTrongNgay_Model;
            if (item != null)
                Navigation.PushAsync(new TongHopBaoCaoTienDoLapDat_Page(item.Code));

        }
    }    
}
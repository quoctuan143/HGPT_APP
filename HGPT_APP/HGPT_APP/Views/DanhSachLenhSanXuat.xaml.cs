using HGPT_APP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGPT_APP.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HGPT_APP.Popup;
using Syncfusion.SfDataGrid.XForms;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DanhSachLenhSanXuat : ContentPage
    {
        Danh_Sach_Lenh_San_Xuat_ViewModel viewModel;
        string filterText;
        public DanhSachLenhSanXuat()
        {
            InitializeComponent();
            listLenhSX.QueryRowHeight += DataGrid_QueryRowHeight;
            viewModel = new Danh_Sach_Lenh_San_Xuat_ViewModel();
            BindingContext = viewModel;
        }
        private void DataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (e.RowIndex != 0)
            {
                //Calculates and sets height of the row based on its content 
                e.Height = listLenhSX.GetRowHeight(e.RowIndex);
                e.Handled = true;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.IsBusy == true) return;
            if (viewModel.List_LENH_SAN_XUATs.Count == 0)
                viewModel.LoadLenhSanXuat.Execute(null);
        }
        public bool FilterRecords(object o)
        {
            try
            {
                var item = o as DANH_MUC_LENH_SAN_XUAT;

                if (item != null)
                {

                    if (item.External_Document_No_.ToLower().Contains(filterText))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                filterText = e.NewTextValue;
                listLenhSX.View.Filter = FilterRecords;
                listLenhSX.View.RefreshFilter();
            }
            catch (Exception)
            {

               
            }
           
        }

       

        private async void SfTabView_TabItemTapped_1(object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {
            try
            {
                if (e.TabItem.Title == "Lịch Sử")
                {

                }
                if (e.TabItem.Title == "Công Đoạn")
                {
                    if (viewModel.SelectLSX != null)
                    {
                        viewModel.LoadCongDoanLSX.Execute(viewModel.SelectLSX.LENH_SAN_XUAT);
                    }

                }
                if (e.TabItem.Title == "KH Bảo Trì")
                {

                }
            }
            catch (Exception ex)
            {
                await new MessageBox("Thông báo", ex.ToString()).Show();

            }
            
        }
    }
}
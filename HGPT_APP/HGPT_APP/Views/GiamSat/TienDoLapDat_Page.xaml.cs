using HGPT_APP.Models.GiamSat;
using HGPT_APP.ViewModels.GiamSat;
using Syncfusion.SfDataGrid.XForms;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.GiamSat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TienDoLapDat_Page : ContentPage
    {
        TienDoLapDat_ViewModel viewModel;
        List<NgayCong> NgayCong = new List<NgayCong>();
        public TienDoLapDat_Page() 
        {
            InitializeComponent();           
            viewModel = new TienDoLapDat_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel; 
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

        protected  override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListTienDoLapDat.Count  == 0 )
            {
                viewModel.LoadCommand.Execute(null);                
            }    
        }
        string filterText = "";
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listCongDoanCongNhan.View.Filter = FilterRecords;
            listCongDoanCongNhan.View.RefreshFilter();
        }
        public bool FilterRecords(object o)
        {
            try
            {
                var item = o as TienDoLapDat;

                if (item != null)
                {

                    if (item.Description.ToLower().Contains(filterText) || item.QuyCach.ToLower().Contains(filterText))
                        return true;
                }
                return false;
            }
            catch (System.Exception)
            {

                return false; 
            }

           
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var cb = (CheckBox)sender;
            var item = (TienDoLapDat)cb.BindingContext;
            if (item == null) return;
            foreach (TienDoLapDat td in viewModel.ListTienDoLapDat)
            {
                if (td.OrderCode.StartsWith(item.OrderCode) && td.LenhSanXuat == item.LenhSanXuat)
                    if (cb.IsChecked  == true)
                    {
                        td.SoLuongCanLap = td.Quantity - td.SoLuongLapDat;
                        td.ValueCha = true;
                    }
                    else
                    {
                        td.SoLuongCanLap = 0;
                        td.ValueCha = false;
                    }
            }            
        }
    }    
}
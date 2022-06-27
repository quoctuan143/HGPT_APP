using HGPT_APP.Models.GiamSat;
using HGPT_APP.ViewModels.GiamSat;
using Syncfusion.SfDataGrid.XForms;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.GiamSat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TongHopBaoCaoTienDoLapDat_Page : ContentPage
    {
        TongHopBaoCaoTienDoLapDat_ViewModel viewModel;
        List<NgayCong> NgayCong = new List<NgayCong>();
        public TongHopBaoCaoTienDoLapDat_Page()  
        {
            InitializeComponent();
            listCongDoanCongNhan.QueryRowHeight += ListCongDoanCongNhan_QueryRowHeight;
            viewModel = new TongHopBaoCaoTienDoLapDat_ViewModel();
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListTienDoLapDat.Count  == 0 )
            {
                viewModel.LoadCommand.Execute(null);
            }    
        }     
       
    }    
}
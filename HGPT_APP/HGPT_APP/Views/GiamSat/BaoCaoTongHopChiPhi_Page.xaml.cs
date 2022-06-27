using HGPT_APP.ViewModels.GiamSat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.GiamSat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaoCaoTongHopChiPhi_Page : ContentPage
    {
        BaoCaoTongHopChiPhi_ViewModel viewModel;        
        public BaoCaoTongHopChiPhi_Page()  
        {
            InitializeComponent();
            viewModel = new BaoCaoTongHopChiPhi_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;            
        }

       

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListLenhSanXuat.Count  == 0 )
            {
                viewModel.LoadCongDoanBaoCao.Execute(null);
            }    
        }

        private void addgroup_Clicked(object sender, EventArgs e)
        {
            var group = listCongDoanCongNhan.GroupColumnDescriptions.Where(x => x.ColumnName == "NoiDung").FirstOrDefault();
            if (group == null )
            {
                listCongDoanCongNhan.GroupColumnDescriptions.Add(new Syncfusion.SfDataGrid.XForms.GroupColumnDescription { ColumnName = "NoiDung" });
            }    
            
        }

        private void removegroup_Clicked(object sender, EventArgs e)
        {
            var group = listCongDoanCongNhan.GroupColumnDescriptions.Where(x => x.ColumnName == "NoiDung").FirstOrDefault();
            if (group != null )
            listCongDoanCongNhan.GroupColumnDescriptions.Remove(group);
        }
    }
}
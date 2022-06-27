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
    public partial class XemBaoCaoGiamSat_Page : ContentPage
    {
        XemBaoCaoGiamSat_ViewModel viewModel;       
        public XemBaoCaoGiamSat_Page()
        {
            InitializeComponent();
            viewModel = new XemBaoCaoGiamSat_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListBaoCaoGiamSat == null )
            {
                viewModel.LoadCongDoanBaoCao.Execute(null);
            }    
        }
    }
}
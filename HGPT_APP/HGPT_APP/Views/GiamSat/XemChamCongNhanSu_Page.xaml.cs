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
    public partial class XemChamCongNhanSu_Page : ContentPage
    {
        XemChamCongNhanSu_ViewModel viewModel;        
        public XemChamCongNhanSu_Page() 
        {
            InitializeComponent();
            viewModel = new XemChamCongNhanSu_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;            
        }

       

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListCapNhatGioCong.Count  == 0 )
            {
                viewModel.LoadCommand.Execute(null);
            }    
        }
    }
}
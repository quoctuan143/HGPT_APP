using HGPT_APP.Models;
using HGPT_APP.ViewModels.GiamSat;
using Syncfusion.SfDataGrid.XForms;
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
    public partial class BaoCaoGiamSat_Page : ContentPage    {
        
        BaoCaoGiamSat_ViewModel viewModel;
        Timer timer;
        int position = 0;
        public BaoCaoGiamSat_Page()
        {
            InitializeComponent();
            viewModel = new BaoCaoGiamSat_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;            
            timer = new Timer();
            timer.Interval = 2000;
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();            
        }       

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                position += 1;
                viewImage.Position = position;
                if (viewImage.Position > viewModel.ImageList.Count )
                {
                    position = 0;
                    viewImage.Position = position;
                }
            });
        }

        protected override  void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListBaoCaoGiamSat == null )
            {
                viewModel.LoadCongDoanBaoCao.Execute(null);
                
            }
            
        }
    }
}
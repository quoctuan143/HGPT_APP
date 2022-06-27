﻿using HGPT_APP.ViewModels.GiamSat;
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
    public partial class ThemNhanSuChoGiamSat_Page : ContentPage
    {
        ThemNhanSuChoGiamSat_ViewModel viewModel;        
        public ThemNhanSuChoGiamSat_Page()  
        {
            InitializeComponent();
            viewModel = new ThemNhanSuChoGiamSat_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;            
        }

       

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListEmployee.Count  == 0 )
            {
                viewModel.LoadCongDoanBaoCao.Execute(null);
            }    
        }
    }
}
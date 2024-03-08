using HGPT_APP.Global;
using HGPT_APP.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HGPT_APP
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            if (Preferences.Get(Config.User, "tuannq") == "tuannq")
            {
                homeTest.IsVisible = true;
                thongbao.IsVisible = true;
                tracuu.IsVisible = true;
                caidat.IsVisible = true;
            }
            else
            {
                home.IsVisible = true;
                caidat.IsVisible = true;
            }    
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
    }
}

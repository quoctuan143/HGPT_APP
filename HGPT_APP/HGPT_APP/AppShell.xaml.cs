using HGPT_APP.Views;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HGPT_APP
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.Test
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{
		public SearchPage ()
		{
			InitializeComponent ();
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
			Navigation.PushAsync(new TraCuuDiemThuTienDienPage());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
			Navigation.PushAsync(new TraCuuDienNangTieuThuPage());
		}
    }
}
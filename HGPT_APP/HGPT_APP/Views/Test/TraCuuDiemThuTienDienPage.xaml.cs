using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.Test
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TraCuuDiemThuTienDienPage : ContentPage
	{
		public ObservableCollection<TraCuuDiemThuTienDienModel> ListTraCuu { get; set; }
		public TraCuuDiemThuTienDienPage ()
		{
			InitializeComponent ();
			ListTraCuu = new ObservableCollection<TraCuuDiemThuTienDienModel>();
			ListTraCuu.Add(new TraCuuDiemThuTienDienModel { DIEMTHU = "Ngân hàng VCB", DCHI_THU = "121 Kinh Dương Vương", NGAY_THU = "20 hàng tháng", TGIAN_LVIEC = "08:00 - 17:00" });
			BindingContext = this;
		}
	}
	public class TraCuuDiemThuTienDienModel
	{
		public string DIEMTHU { get; set; }
		public string DCHI_THU { get; set; }
		public string NGAY_THU { get; set; }
		public string TGIAN_LVIEC { get; set; }

	}
}
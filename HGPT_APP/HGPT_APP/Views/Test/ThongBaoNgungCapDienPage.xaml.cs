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
	public partial class ThongBaoNgungCapDienPage : ContentPage	{
		
		ObservableCollection<THONG_BAO_KHAC_MODEL> _listTraCuu;
		public ObservableCollection<THONG_BAO_KHAC_MODEL> ListTraCuu { get => _listTraCuu; set { _listTraCuu = value; } }
		public ThongBaoNgungCapDienPage()
		{
			InitializeComponent();
			ListTraCuu = new ObservableCollection<THONG_BAO_KHAC_MODEL>();
			ListTraCuu.Add(new THONG_BAO_KHAC_MODEL { NGAY_TBAO = new DateTime(2021, 12, 14), DA_XEM = "Chưa xem", TIEU_DE = "THÔNG BÁO CẤP ĐIỆN", NOI_DUNG = "HGPT xin thông báo lịch tạm ngưng cấp điện theo kế hoạch của trạm biến áp KCD Hòa Khánh -HCV12334 từ 06:30 đến 13:00 ngày 15/12/2021. Kính mong quý khách thông cảm." });
			BindingContext = this;
		}
	}
}
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
	public partial class TraCuuDienNangTieuThuPage : ContentPage
	{
		public ObservableCollection<TraCuuDienNangTieuThuModel> ListTraCuu { get; set; }
		public TraCuuDienNangTieuThuPage ()
		{
			InitializeComponent ();
            ListTraCuu = new ObservableCollection<TraCuuDienNangTieuThuModel>();
            ListTraCuu.Add(new TraCuuDienNangTieuThuModel
            {
                CHISO_CU = 1000,
                CHISO_MOI = 1100,
                DIEN_TTHU = 100,
                HSNHAN = 1,
                MA_CTO = "1000002323",
                MA_KHANG = "KH0000101",
                NAM = "2021",
                NGAY_CKY = DateTime.Today,
                NGAY_DKY = DateTime.Today,
                THANG = "12",
                TEN_KHANG = "NGUYỄN QUỐC TUẤN"
            }
            );
            BindingContext = this;
        }
	}
    public class TraCuuDienNangTieuThuModel
    {
        public string MA_KHANG { get; set; }
        public string TEN_KHANG { get; set; }
        public string NAM { get; set; }
        public string THANG { get; set; }
        public string MA_CTO { get; set; }
        public decimal? CHISO_CU { get; set; }
        public decimal? HSNHAN { get; set; }
        public decimal? CHISO_MOI { get; set; }
        public decimal? SLUONG_THAO { get; set; }
        public decimal? DIEN_TTHU { get; set; }
        public DateTime? NGAY_DKY { get; set; }
        public DateTime? NGAY_CKY { get; set; }
    }
}
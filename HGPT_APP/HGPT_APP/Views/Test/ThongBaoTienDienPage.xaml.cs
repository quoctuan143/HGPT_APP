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
	public partial class ThongBaoTienDienPage : ContentPage
	{
		public ObservableCollection<THONG_BAO_TIEN_DIEN_MODEL> ListTraCuu { get; set; }
		public ThongBaoTienDienPage ()
		{
			InitializeComponent ();
            ListTraCuu = new ObservableCollection<THONG_BAO_TIEN_DIEN_MODEL>();
            ListTraCuu.Add(new THONG_BAO_TIEN_DIEN_MODEL { NGAY_TBAO = new DateTime(2021, 12, 30), DA_XEM = "Chưa xem", TIEU_DE = "THÔNG BÁO TIỀN ĐIỆN", NOI_DUNG = "Quý khách có hóa đơn tiền điện từ ngày 1/12/2021 - 31/12/2021 có chỉ số cuối là 8024, SL 195KW, số tiền là 500.000. thu tại các quầy giao dịch." , TEN_KHANG ="NGUYỄN QUỐC TUẤN"});
            ListTraCuu.Add(new THONG_BAO_TIEN_DIEN_MODEL { NGAY_TBAO = new DateTime(2021, 12, 25), DA_XEM = "Chưa xem", TIEU_DE = "THÔNG BÁO THANH TOÁN", NOI_DUNG = "Quý khách hàng đã thanh toán tiền điện kì 1 tháng 12 năm 2021 của mã khách hàng PP030000940650",TEN_KHANG="NGUYỄN QUỐC TUẤN" });
            BindingContext = this;
        }
	}
    public class THONG_BAO_TIEN_DIEN_MODEL
    {
        public string HOADON_ID { get; set; }
        public string MA_KHANG { get; set; }
        public string TEN_KHANG { get; set; }
        public string TIEU_DE { get; set; }
        public string GHI_CHU { get; set; }
        public string NOI_DUNG { get; set; }
        public string KIHIEU_HDON { get; set; }
        public string DCHI_KHANG { get; set; }
        public string LOAI_THONG_BAO { get; set; }
        public string THANG { get; set; }
        public string NAM { get; set; }
        public string TIEN_BANG_CHU { get; set; }
        public DateTime? NGAY_TBAO { get; set; }
        public DateTime? NGAY_NOP { get; set; }
        int _trangthaixem;
        public int TRANG_THAI_XEM
        {
            get => _trangthaixem; set
            {
                _trangthaixem = value;
                if (_trangthaixem == 0)
                    DA_XEM = "chưa xem";
                else
                    DA_XEM = "đã xem";
            }
        }

        public string DA_XEM { get; set; }

        public DateTime? NGAY_DKY { get; set; }
        public DateTime? NGAY_CKY { get; set; }
        public Decimal? TONG_TIEN { get; set; }
        public Decimal? TIEN_THUE { get; set; }
        public Decimal? TIEN_DIEN { get; set; }
        public Decimal? DIEN_TTHU { get; set; }
    }
}
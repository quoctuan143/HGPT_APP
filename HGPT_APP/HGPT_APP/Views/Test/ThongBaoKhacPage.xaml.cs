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
	public partial class ThongBaoKhacPage : ContentPage
	{
        ObservableCollection<THONG_BAO_KHAC_MODEL> _listTraCuu;
        public ObservableCollection<THONG_BAO_KHAC_MODEL> ListTraCuu { get => _listTraCuu; set { _listTraCuu = value; } }
        public ThongBaoKhacPage ()
		{
            InitializeComponent();
            ListTraCuu = new ObservableCollection<THONG_BAO_KHAC_MODEL>();
            ListTraCuu.Add(new THONG_BAO_KHAC_MODEL { NGAY_TBAO = new DateTime(2021, 12, 30) , DA_XEM ="Chưa xem", TIEU_DE ="THÔNG BÁO HÓA ĐƠN ĐIỆN TỬ" ,NOI_DUNG= "Mẫu hóa đơn điện tử (HĐĐT), thông báo tiền điện mới được EVN thiết kế dựa trên kết quả bình chọn, góp ý của khách hàng từ chương trình Bình chọn trực tuyến mẫu Hóa đơn tiền điện mới, đã được Tập đoàn tổ chức trong năm 2019. Chương trình đã thu hút 76.182 lượt bình chọn trên toàn quốc. Kết quả, mẫu HĐĐT và thông báo tiền điện số 4 có số lượt tham gia chọn cao nhất (24.167 phiếu bình chọn, chiếm tỷ lệ 31,7%)." });
            ListTraCuu.Add(new THONG_BAO_KHAC_MODEL { NGAY_TBAO = new DateTime(2021, 12, 25), DA_XEM = "Chưa xem", TIEU_DE = "MẪU HÓA ĐƠN MỚI",NOI_DUNG= "Mẫu HĐĐT, mẫu thông báo tiền điện mới có tính kế thừa mẫu hóa đơn hiện hành để tạo sự gần gũi, đồng thời đảm bảo đầy đủ thông tin theo quy định về hóa đơn của Bộ Tài chính. Thông tin trên hóa đơn, thông báo tiền điện đơn giản, dễ hiểu và bổ sung đẩy đủ các kênh thông tin liên hệ, cung cấp dịch vụ điện." });
            BindingContext = this;
        }
	}
    public class THONG_BAO_KHAC_MODEL
    {
        public string HOADON_ID { get; set; }
        public string MA_KHANG { get; set; }
        public string TEN_KHANG { get; set; }
        public string TIEU_DE { get; set; }
        public string GHI_CHU { get; set; }
        public string NOI_DUNG { get; set; }
        public string DCHI_KHANG { get; set; }
        public string LOAI_THONG_BAO { get; set; }
        public string THANG { get; set; }
        public string NAM { get; set; }
        public DateTime? NGAY_TBAO { get; set; }
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

    }
}
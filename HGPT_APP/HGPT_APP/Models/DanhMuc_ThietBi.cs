using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models
{
   public class DanhMuc_ThietBi
    {
        public string No_ { get; set; }
        public string No_2 { get; set; }
        public string No_3 { get; set; }  
        public string NameVN { get; set; }
        public string Description { get; set; }
        public string MaTaiSan { get; set; }
        public string TenChungLoai { get; set; } 
        public string Picture { get; set; } // link ảnh
        public string NhaSanXuat { get; set; } //hãng sản xuất
        public string XuatXu { get; set; } //xuất xứ   
        public string CongSuat { get; set; } //xuất xứ   
        public string Ma_Nha_May { get; set; }
        public string Nha_May { get; set; }
        public string Phong_Ban { get; set; }
        public string Ma_Phong_Ban { get; set; } 
        public string ItemCategory { get; set; }
        public string TinhTrangThietBi { get; set; } // công suất 
        public DateTime? NgaySuDung { get; set; }
        public string NguoiSuDung { get; set; }
        public string NamSanXuat { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models
{
   public class LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN
    {
        public Nullable<int> RowID { get; set; }
        public string LENH_SAN_XUAT { get; set; }
        public string MA_CONG_DOAN { get; set; }
        public string MA_NHAN_VIEN { get; set; }
        public Nullable<decimal> SO_NGAY_CONG { get; set; }
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public string NGUOI_TAO { get; set; }
        public Nullable<int> TRANG_THAI { get; set; }
        public Nullable<System.DateTime> GIO_BAT_DAU { get; set; }
        public Nullable<System.DateTime> GIO_KET_THUC { get; set; }
        public string NOI_DUNG { get; set; }
        public string MA_NHA_MAY { get; set; }
        public string MA_XUONG { get; set; }
        public string TEN_NHAN_VIEN { get; set; }
        public string TO_SAN_XUAT { get; set; }
        public string ExternalDocumentNo_ { get; set; }
        public Nullable<int> UnCheck { get; set; }
        public string DINH_MUC { get; set; } 
    }
}

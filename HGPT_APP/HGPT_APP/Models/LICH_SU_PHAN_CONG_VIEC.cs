using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models
{
  public  class LICH_SU_PHAN_CONG_VIEC
    {
        public Nullable<int> RowID { get; set; }
        public string LENH_SAN_XUAT { get; set; }
        public string MA_CONG_DOAN { get; set; }
        public string MA_NHAN_VIEN { get; set; }
        public string MA_TANG_CA { get; set; }
        public Nullable<decimal> NGAY_CONG { get; set; }
        public Nullable<decimal> GIO_CONG { get; set; }
        public Nullable<decimal> TANG_CA { get; set; } 
        public Nullable<System.DateTime> NGAY_TAO { get; set; }
        public string NGUOI_TAO { get; set; }        
        public string NOI_DUNG { get; set; }   
        public string TEN_NHAN_VIEN { get; set; }
        public string TO_SAN_XUAT { get; set; }
        public string ExternalDocumentNo_ { get; set; }
        public string FullName { get; set; }
        public string DINH_MUC { get; set; }
    }
}

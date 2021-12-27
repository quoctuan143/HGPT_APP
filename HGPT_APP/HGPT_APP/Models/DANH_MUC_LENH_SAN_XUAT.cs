using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models
{
   public class DANH_MUC_LENH_SAN_XUAT : Bindable
    {
       
         public string LENH_SAN_XUAT { get; set; }
        public string External_Document_No_ { get; set; }
        public string Description { get; set; }
        public DateTime Ending_Date { get; set; }
        public int IsRunning { get; set; }
        public string Ten_Khach_Hang { get; set; }
        public string Ten_San_Pham { get; set; }
        public int NgayConLai { get; set; }
        public string Ten_Day_Du { get; set; }
    }
}

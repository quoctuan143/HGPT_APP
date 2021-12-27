using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models
{
   public  class NotifycationModel 
    {
        public Int32? RowID { get; set; }
        public string Description { get; set; }
        public DateTime?  CreateDate { get; set; }
        public string  Code { get; set; }
        Int32 _viewed;
        public Int32 Viewed { get => _viewed ; set { _viewed = value;
                if (_viewed == 1) TRANG_THAI = "Đã xem";
                else TRANG_THAI = "Chưa xem";
            } }
        public string TRANG_THAI { get; set; }
    }
}

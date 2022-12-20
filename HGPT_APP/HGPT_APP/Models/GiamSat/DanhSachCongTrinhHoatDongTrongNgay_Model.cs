using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models.GiamSat
{
   public class DanhSachCongTrinhHoatDongTrongNgay_Model
    {
        public string CongTrinh { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }

        public DateTime PostingDate { get; set; }
    }

    public class DanhSachCongTrinhDuocLapDatTrongNgay_Model
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
    }
}

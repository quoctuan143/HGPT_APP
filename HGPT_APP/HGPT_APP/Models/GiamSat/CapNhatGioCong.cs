using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models.GiamSat
{
    public class CapNhatGioCong
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string MaCongTrinh { get; set; }
        public string TenCongTrinh { get; set; }
        public double NgayCong { get; set; } 
        public double TangCa { get; set; }
        public double TangCaDem { get; set; }
        public string NguoiCham { get; set; }
        public int Check { get; set; }
        public string NhomChamCong { get; set; }
    }
    
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models.GiamSat
{
  public  class TongHopBaoCaoTienDoLapDat
    {
        public string Description { get; set; }
        public string QuyCach { get; set; }
        public double SoLuong { get; set; }
        public double KhoiLuong { get; set; }
        public double KhoiLuongDaLap { get; set; }
        public double PhanTramDaLap { get; set; }
        public int Type { get; set; } 
        public string TenBaoCao { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models.GiamSat
{
  public  class NhanLuc
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double  Quantity { get; set; }
        public double HomQua { get; set; }
        public double UnitPrice { get; set; }
        public bool IsEdit { get; set; }
    }
}

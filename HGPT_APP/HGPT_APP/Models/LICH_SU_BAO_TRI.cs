using HGPT_APP.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HGPT_APP.Models
{
  public  class LICH_SU_BAO_TRI : BaseViewModel 
    {
        public Int32 RowID { get; set; }
        public string TEN_THIET_BI { get; set; }
        public string SERIAL { get; set; }
        public string MODEL { get; set; }
        public string MA_THIET_BI { get; set; }
        public string MA_PHU_TUNG { get; set; }

        string _noidung;
        public string NOI_DUNG
        {

            get { return _noidung; }
            set { _noidung = value; OnPropertyChanged("NOI_DUNG_BAO_TRI"); }
        }
        string _noidungbaotri;

       

        public string NOI_DUNG_BAO_TRI
        {

            get
            {
                _noidungbaotri = _noidung;
                _noidungbaotri = _noidungbaotri.Replace("||", System.Environment.NewLine);
                return _noidungbaotri;
            }
            set
            {
                _noidungbaotri = value;
            }
        }

        public string NGUOI_THUC_HIEN { get; set; }
        public int? BAO_DUONG_DINH_KY { get; set; }
        public int? STATUS { get; set; }
        public string TINH_TRANG_HIEN_TAI { get; set; }
        public string NGUOI_XAC_NHAN { get; set; }
        public DateTime? NGAY_GIO { get; set; }
        public string IMAGE_LINK { get; set; }
        public string MA_NHA_MAY { get; set; }
        public string MA_XUONG { get; set; }
        public int THANG { get; set; }
        public string NHOM_BAO_TRI { get; set; }
    }
}

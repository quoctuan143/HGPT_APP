using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models
{
   public class ChamSocKhachHang
    {
        public int RowID { get; set; } 
        public string CongTy { get; set; }
        public string NguoiLienHe { get; set; }
        public string PhoneNo_ { get; set; }
        public string ChucVu { get; set; }
        public string Email { get; set; }
        public DateTime NgaySinh { get; set; }
        public string FullName { get; set; }
        public string NguoiChamSoc { get; set; }
        string _noidungchamsoc;
        public string NoiDungChamSoc { get => _noidungchamsoc; set { _noidungchamsoc = value; _noidungchamsoc  = _noidungchamsoc.Replace("|", Environment.NewLine); } }
        public int? HoanThanh { get; set; }
        public DateTime? NgayChamSoc { get; set; }
    }
}

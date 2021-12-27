using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models
{
   public class Account
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public int Active { get; set; }
        public string FullName { get; set; }
        public string Function { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WorkCenterCode { get; set; }
        public string ImageURL { get; set; }
        public int IsThietBi { get; set; }
        public int IsPhanViec { get; set; }
        public int ChamSocKhachHang { get; set; }
    }
}

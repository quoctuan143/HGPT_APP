using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Global
{
    public class Config
    {
        public static string URL = "http://hrm.hgpt.vn/";
        public static string User = "User";
        public static string Password = "Password";
        public static string Role = "Role";
        public static string FullName = "FullName";
        public static string PhoneNumber = "PhoneNumber";
        public static string AnhDaiDien = "AnhDaiDien";
        public static string IsThietBi = "IsThietBi";
        public static string IsPhanViec = "IsPhanViec";
        public static string IsChamSocKhachHang = "IsChamSocKhachHang";
        public static string Token { get; set; }
        public static string NhaMay = "NhaMay";
        public static string MaXuong = "MaXuong";
        public static string ThongBaoBaoTri = "ThongBaoBaoTri";
        public static string ThongBaoPhanViec = "ThongBaoPhanViec";
        public static string ThongBaoCongTy = "ThongBaoCongTy";
        public static string LenhSanXuat = "LenhSanXuat";
        
        public static System.Net.Http.HttpClient client;       
        public Config()
        {
            if (client == null)
            {
                client = new System.Net.Http.HttpClient();
            }          
        }

    }

    public enum DialogReturn
    {
        OK = 0,
        Cancel = 1,
        Repeat = 2,
        Stop = 3
    }
}

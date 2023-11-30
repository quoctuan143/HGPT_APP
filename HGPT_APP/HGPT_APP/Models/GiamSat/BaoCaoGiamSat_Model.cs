using HGPT_APP.Popup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace HGPT_APP.Models.GiamSat
{
   public  class BaoCaoGiamSat_Model // lấy danh sách từ db ra chia ra nhiều table để list lên giao diện
    {
       
        public ObservableCollection<ThoiTiet > ListThoiTiet { get; set; }
        public ObservableCollection<NhanLuc> ListNhanLuc { get; set; }
        public ObservableCollection<MayMocThietBi > ListMayMocThietBi { get; set; }
        public ObservableCollection<ChiPhiKhac > ListChiPhiKhac { get; set; } 
        public ObservableCollection<TomTatCongViec> ListTomTatCongViec { get; set; }
        public ObservableCollection<ImageList> ListImage { get; set; } 
    }
    public class TomTatCongViec 
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string  CongViecNgayMai { get; set; }
       
    }
    public class ChiPhiKhac :Bindable
    {
        public string Code { get; set; }      
        public string Description { get; set; }
        public double _amount;
        public double Amount { get => _amount ; set => SetProperty (ref _amount , value ); }
        public string FormatAmount
        {
            get => string.Format("{0:#,##0}", Amount);
            set
            {
                if (!CheckThapPhan(value))
                {
                    FormatNumberString(ref _amount, value);
                    OnPropertyChanged("FormatAmount");
                    Amount = _amount;
                }

            }
        }
        public string NhomChiPhi { get; set; }
    }
    public class ImageList
    {
        public string ImageUrl { get; set; }     

    }
    public class BaoCaoGiamSat_Insert //cái này dùng để lưu vào database
    {
        public string Code { get; set; }
        public int Weather { get; set; }
        public DateTime PostingDate { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public string CongViecNgayMai { get; set; }       
        public string CongTrinh { get; set; }
        public string NhomChiPhi { get; set; }
        public int KhongTinhChoGiamSat { get; set; }
    }
}

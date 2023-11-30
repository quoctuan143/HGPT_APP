using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models.GiamSat
{
    public class CapNhatGioCong :Bindable
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string MaCongTrinh { get; set; }
        public string TenCongTrinh { get; set; }
        public double NgayCong { get; set; }
        double _tangca;
        public double TangCa { get => _tangca; set => SetProperty(ref _tangca,value ); }
        public string FormatTangCa { get=> string.Format("{0:#,##0.#}", TangCa);
            set 
            { 
                if (!CheckThapPhan(value))
                {
                    FormatNumberString(ref _tangca, value);
                    OnPropertyChanged("FormatTangCa");
                    TangCa = _tangca;
                }    
            }
        }
        double _tangcadem;
        public double TangCaDem { get => _tangcadem; set => SetProperty(ref _tangcadem,value); }
        public string FormatTangCaDem 
        {
            get => string.Format("{0:#,##0.#}", TangCaDem);
            set
            {
                if (!CheckThapPhan(value))
                {
                    FormatNumberString(ref _tangcadem, value);
                    OnPropertyChanged("FormatTangCaDem");
                    TangCaDem = _tangcadem;
                }
            }
        }
        public string NguoiCham { get; set; }
        public int Check { get; set; }
        public string NhomChamCong { get; set; }

        public bool KhongTinhAn { get; set; }
    }
    
}

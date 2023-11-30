using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models.GiamSat
{
 public class MayMocThietBi : Bindable
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double _quantity;
        public double Quantity { get => _quantity; set => SetProperty(ref _quantity ,value); }
        public string FormatQuantity
        {
            get => string.Format("{0:#,##0.#}",Quantity);
            set
            {
                if (!CheckThapPhan(value))
                {
                    FormatNumberString(ref _quantity, value);
                    OnPropertyChanged("FormatQuantity");
                    Quantity = _quantity;
                }

            }
        }
        public double _unitPrice;
        public double UnitPrice { get => _unitPrice; set => SetProperty(ref _unitPrice, value); }
        public string FormatUnitPrice
        {
            get => string.Format("{0:#,##0}", UnitPrice);
            set
            {
                if (!CheckThapPhan(value))
                {
                    FormatNumberString(ref _unitPrice, value);
                    OnPropertyChanged("FormatUnitPrice");
                    UnitPrice = _unitPrice;
                }

            }
        }
        public bool IsEdit { get; set; }

       public string  KhongTinhChoGiamSat { get; set; }
    }
}

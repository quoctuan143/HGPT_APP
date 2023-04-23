using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Models.GiamSat
{
  public  class NhanLuc : Bindable
    {
        public string Code { get; set; }
        public string Description { get; set; }
        double _quantity;
        public double Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }        
        public string FormatNumberQuantity
        {
            get => string.Format("{0:#,##0}", Quantity);
            set
            {
                if (!CheckThapPhan(value))
                {
                    FormatNumberString(ref _quantity, value);
                    OnPropertyChanged("FormatNumberQuantity");
                    Quantity = _quantity;
                }
            }
        }
        public double HomQua { get; set; }
        public double UnitPrice { get; set; }
        public string FormatUnitPrice 
        {
            get => string.Format("{0:#,##0}", UnitPrice);
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    char lastChar = value[value.Length - 1];
                    if (lastChar != ',')
                    {
                        double.TryParse(value, out double number);
                        UnitPrice = number;
                        OnPropertyChanged("FormatUnitPrice");
                    }
                }
                else
                {
                    UnitPrice = 0;
                    OnPropertyChanged("FormatUnitPrice");
                }
            }
        }
        public bool IsEdit { get; set; }
    }
}

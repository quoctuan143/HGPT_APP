using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace HGPT_APP.Converter
{
    public class NgayConLaiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string loai = parameter as string;
            if (loai == "gridcell")
            {
                int _value = (int)value;
                if (_value < 5)
                    return Color.Red;
                return Color.Transparent;
            }    
            else
            {
                int _value = (int)value;
                if (_value < 5)
                    return Color.Red;
                return Color.Black ;
            }    
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

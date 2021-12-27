using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace HGPT_APP.Converter
{
    public class ChuyenTrangThaiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int _value = (int)value;
            string _param = parameter as string;
            if (_param == "textcolor") // trả về là màu như backrougd
            {
                if (_value == 0)
                    return Color.Black ;
                else if (_value == 1)
                    return Color.White;
                return Color.White  ;
            }
            else if (_param == "background") // trả về là màu như backrougd
            {
                if (_value == 0)
                    return Color.White;
                else if (_value == 1)
                    return Color.Green;
                return Color.Gray;
            }
            else
            {
                if (_value == 0)
                    return "Mới tạo";
                else if (_value == 1)
                    return "Đang thực hiện";
                return "Kết thúc";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

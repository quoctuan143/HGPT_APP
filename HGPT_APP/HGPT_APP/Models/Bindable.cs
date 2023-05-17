using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;

namespace HGPT_APP.Models
{
    public class Bindable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName] string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged     
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CheckThapPhan(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                char lastChar = value[value.Length - 1];                
                if (CultureInfo.InstalledUICulture.Name == "en-US")
                {
                    if (lastChar == '.')
                    {
                        return true;
                    }
                }   
                else
                {
                    if (lastChar == ',')
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }
        public void FormatNumberString(ref double number, string value)
        {
            double.TryParse(!string.IsNullOrEmpty(value) ? value : "0", out double quantity);
            number = quantity;
        }
        #endregion
    }
}

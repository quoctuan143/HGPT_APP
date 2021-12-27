using HGPT_APP.Global;
using HGPT_APP.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cap_Nhat_Thoi_Gian_Bat_Dau : PopupPage
    {
        TaskCompletionSource<object > _tsk = null;
        public Cap_Nhat_Thoi_Gian_Bat_Dau()
        {
            InitializeComponent();
        }
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
           
            try
            {
               
                await Navigation.PopAllPopupAsync(true);
               DateTime date = new DateTime(dateKetThuc.Date.Year, dateKetThuc.Date.Month, dateKetThuc.Date.Day, timeKetThuc.Time.Hours, timeKetThuc.Time.Minutes, timeKetThuc.Time.Seconds);
                _tsk.SetResult(date);
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return;
            }

        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync(true);
            _tsk.SetResult(null);
        }
        public async Task<object > Show()
        {
            _tsk = new TaskCompletionSource<object>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
    }
}
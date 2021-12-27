using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGPT_APP.Global;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HGPT_APP.Popup;
using Xamarin.Essentials;
using System.Net.Http;
using HGPT_APP.Models;
using HGPT_APP.Views;

namespace HGPT_APP.Popup
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageChangePassword : PopupPage
    {
        TaskCompletionSource<DialogReturn> _tsk = null;
        public MessageChangePassword()
        {
            InitializeComponent();
        }
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(oldPass.Text))
            {
                await new MessageBox("Thông báo","Nhập mật khẩu cũ").Show();
                return;
            }
            if (string.IsNullOrEmpty(newPass.Text))
            {
                await new MessageBox("Thông báo", "Nhập mật khẩu mới").Show();
                return;
            }
            if (string.IsNullOrEmpty(newPass1.Text))
            {
                await new MessageBox("Thông báo", "Nhập lại mật khẩu mới").Show();
                return;
            }
            if (newPass.Text != newPass1.Text)
            {
                await new MessageBox("Thông báo", "Mật khẩu mới nhập lại không trùng nhau").Show();
                return;
            }

            ChangePasswordModel change = new ChangePasswordModel { Username  = Preferences.Get(Config.User, ""), OldPassword = oldPass.Text, NewPassword = newPass.Text };
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URL);
                var ok = client.PostAsJsonAsync("api/hgpt/changepass", change);
                if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("true"))
                {
                    await new MessageBox("Thông báo", "Đổi mật khẩu thành công").Show();
                    await Navigation.PopPopupAsync(true);
                    _tsk.SetResult(DialogReturn.OK);
                    Application.Current.MainPage = new Login();
                }
                else
                {
                    await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.Replace("\"", "")).Show();
                }
            }

        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync(true);
            _tsk.SetResult(DialogReturn.Cancel);
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
    }
}
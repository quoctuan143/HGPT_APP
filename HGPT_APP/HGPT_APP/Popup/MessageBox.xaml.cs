using HGPT_APP.Global;
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
    public partial class MessageBox : PopupPage
    {
        TaskCompletionSource<DialogReturn> _tsk = null;
        public MessageBox(string thongbao, string noidung)
        {
            InitializeComponent();          
            lblNoiDung.Text = noidung;
        }
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
            _tsk.SetResult(DialogReturn.OK);
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}

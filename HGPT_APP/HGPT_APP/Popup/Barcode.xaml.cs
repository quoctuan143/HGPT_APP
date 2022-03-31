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
    public partial class Barcode : PopupPage
    {
        TaskCompletionSource<DialogReturn> _tsk = null;
        public Barcode(string ma_khachhang)
        {
            InitializeComponent();
            txtBarcode.BarcodeValue = ma_khachhang;
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigTed;
using Foundation;
using HGPT_APP.Interface;
using HGPT_APP.iOS.Renderer;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProcessLoading))]
namespace HGPT_APP.iOS.Renderer
{
    public class ProcessLoading : IProcessLoader
    {
        public System.Threading.Tasks.Task Hide()
        {
            BTProgressHUD.Dismiss();
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task Show(string title = "Loading")
        {
            BTProgressHUD.Show(title, maskType: ProgressHUD.MaskType.Black);
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
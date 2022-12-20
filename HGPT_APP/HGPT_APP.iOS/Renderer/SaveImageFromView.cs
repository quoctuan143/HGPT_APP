using Foundation;
using HGPT_APP.Interface;
using HGPT_APP.iOS.Renderer;
using QuickLook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(SaveImageFromView))]
namespace HGPT_APP.iOS.Renderer
{
    public class SaveImageFromView : ISaveImageFromView
    {
        public async Task<byte[]> SaveImageFromViewAsync()
        {
            var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;

            UIGraphics.BeginImageContext(view.Frame.Size);
            view.DrawViewHierarchy(view.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            using (var imageData = image.AsJPEG(100))
            {
                var bytes = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
                return bytes;
            }
        }       
    }
}
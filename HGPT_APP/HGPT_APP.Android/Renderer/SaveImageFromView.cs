using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HGPT_APP.Droid.Renderer;
using HGPT_APP.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SaveImageFromView))]
namespace HGPT_APP.Droid.Renderer
{
    public class SaveImageFromView : ISaveImageFromView
    {
        [Obsolete]
        public async Task<byte[]> SaveImageFromViewAsync()
        {
            var activity1 = Forms.Context as Activity;

            var view = activity1.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Bitmap bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }
    }
}
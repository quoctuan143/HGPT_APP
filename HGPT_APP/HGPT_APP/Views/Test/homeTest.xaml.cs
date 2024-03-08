using HGPT_APP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.Test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class homeTest : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<FileAttach> ImageList { get; set; }
        public homeTest()
        {
            InitializeComponent();
            ImageList = new ObservableCollection<FileAttach>
                {
                    new FileAttach { FileName = "https://hrm.hgpt.vn/image/hinh1.jpg" },
                    new FileAttach { FileName = "https://hrm.hgpt.vn/image/hinh2.jpg" },
                    new FileAttach { FileName = "https://hrm.hgpt.vn/image/hinh3.jpg" },
                    new FileAttach { FileName = "https://hrm.hgpt.vn/image/hinh4.jpg" }
                };
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}
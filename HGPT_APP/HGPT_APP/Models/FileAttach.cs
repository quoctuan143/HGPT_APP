using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HGPT_APP.Models
{
  public   class FileAttach
    {
        public string FileName { get; set; }
        public ImageSource data { get; set; }
        public byte[] dataByte { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HGPT_APP.Interface
{
   public interface IProcessLoader
    {
        Task Hide();
        Task Show(string title = "Loading");
    }
}

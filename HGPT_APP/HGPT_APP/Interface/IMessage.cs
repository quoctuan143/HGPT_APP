using System;
using System.Collections.Generic;
using System.Text;

namespace HGPT_APP.Interface
{
   public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}

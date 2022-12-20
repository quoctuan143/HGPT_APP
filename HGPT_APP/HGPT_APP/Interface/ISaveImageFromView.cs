using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HGPT_APP.Interface
{
    public interface ISaveImageFromView
    {
        Task<byte[]> SaveImageFromViewAsync();
    }
}

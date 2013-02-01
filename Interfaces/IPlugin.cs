using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    public interface IPlugin 
    {
        INativeHandleContract NativeHandleContract { get; }

        String SendMessage(String message);
    }
}

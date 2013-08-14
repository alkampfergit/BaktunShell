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

        ///// <summary>
        ///// This function will be used to avoid keeping the host alive if the
        ///// main program chrashes for some reason.
        ///// </summary>
        //void Ping();
    }
}

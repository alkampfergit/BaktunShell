using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    [Serializable]
    public class PluginBase : MarshalByRefObject, IPlugin
    {
        public PluginBase(INativeHandleContract nativeHandleContract)
        {
            NativeHandleContract = nativeHandleContract;
        }

        public INativeHandleContract NativeHandleContract { get; private set; }


        public string SendMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}

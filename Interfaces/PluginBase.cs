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
        public PluginBase(INativeHandleContract nativeHandleContract, Object pluginInstance)
        {
            NativeHandleContract = nativeHandleContract;
            _pluginInstance = pluginInstance;
        }

        public INativeHandleContract NativeHandleContract { get; private set; }

        private Object _pluginInstance;

        public string SendMessage(string message)
        {
            if (!(_pluginInstance is IActionableFrameworkElement)) return "Not supported";

            return ((IActionableFrameworkElement)_pluginInstance).SendMessage(message);
        }
    }
}

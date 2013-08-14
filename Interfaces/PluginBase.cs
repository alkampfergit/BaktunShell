using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Interfaces
{
    [Serializable]
    public class PluginBase : MarshalByRefObject, IPlugin
    {
        static Int32 aliveHostCount = 0;

        private Boolean isAlive;

        private DateTime lastPingTime;

        //private Timer aliveTimer;

        public PluginBase(INativeHandleContract nativeHandleContract, Object pluginInstance)
        {
            NativeHandleContract = nativeHandleContract;
            _pluginInstance = pluginInstance;
            Interlocked.Increment(ref aliveHostCount);
            isAlive = true;
            //aliveTimer = new Timer(timerCallback, null, 20000, 20000);
            lastPingTime = DateTime.Now;
        }

        //private void timerCallback(object state)
        //{
        //    if (isAlive && DateTime.Now.Subtract(lastPingTime).TotalSeconds > 60) 
        //    {
        //        isAlive = false;
        //        var actualCount = Interlocked.Decrement(ref aliveHostCount);
        //        if (actualCount == 0) 
        //        { 
        //            //no more active plugin, kill everything
        //            System.Diagnostics.Process.GetCurrentProcess().Kill();
        //        }
        //    }
        //}

        public INativeHandleContract NativeHandleContract { get; private set; }

        private Object _pluginInstance;

        public string SendMessage(string message)
        {
            if (!(_pluginInstance is IActionableFrameworkElement)) return "Not supported";

            return ((IActionableFrameworkElement)_pluginInstance).SendMessage(message);
        }

        //public void Ping()
        //{
        //    lastPingTime = DateTime.Now;
        //}
    }
}

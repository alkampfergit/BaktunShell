using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
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

        private List<EventSink> sinks = new List<EventSink>();

        //private Timer aliveTimer;

        public PluginBase(INativeHandleContract nativeHandleContract, Object pluginInstance)
        {
            NativeHandleContract = nativeHandleContract;
            _pluginInstance = pluginInstance;
            IActionableFrameworkElement afe = _pluginInstance as IActionableFrameworkElement;
            if (afe != null) 
            {
                afe.EventOccurred += OnEvent;
            }
            Interlocked.Increment(ref aliveHostCount);
            isAlive = true;
            lastPingTime = DateTime.Now;
        }

        public INativeHandleContract NativeHandleContract { get; private set; }

        private Object _pluginInstance;

        public string SendMessage(string message)
        {
            if (!(_pluginInstance is IActionableFrameworkElement)) return "Not supported";

            return ((IActionableFrameworkElement)_pluginInstance).SendMessage(message);
        }

        public void RegisterSink(EventSink sink)
        {
            this.sinks.Add(sink);
        }

        public void OnEvent(PluginEventData data) 
        {
            foreach (var sink in sinks)
            {
                try
                {
                    sink.EventToClient(data);
                }
                catch (RemotingException rex)
                {
                    //Ignore it for now.
                    Console.WriteLine(rex.ToString());
                }
            }
        }

    
    }



 
}

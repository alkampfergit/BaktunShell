using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Interfaces
{
    public delegate void PluginEventDelegate(PluginEventData data);

    public interface IPlugin 
    {
        INativeHandleContract NativeHandleContract { get; }

        String SendMessage(String message);

        void RegisterSink(EventSink sink);

        ///// <summary>
        ///// This function will be used to avoid keeping the host alive if the
        ///// main program chrashes for some reason.
        ///// </summary>
        //void Ping();
    }

    [Serializable]
    public class PluginEventData
    {
        /// <summary>
        /// The name of the event.
        /// </summary>
        public String EventName { get; set; }

        /// <summary>
        /// Payload of the event, it can be null
        /// </summary>
        public String EventPayload { get; set; }

        public PluginEventData(
            String eventName,
            String eventPayload)
        {
            EventName = eventName;
            EventPayload = eventPayload;
        }
    }

    [Serializable]
    public class EventSink : MarshalByRefObject
    {
        public event PluginEventDelegate OnHostToClient;

        public EventSink()
        {

        }

        [OneWay]
        public void EventToClient(PluginEventData info)
        {
            if (OnHostToClient != null)
                OnHostToClient(info);
        }

    }
}

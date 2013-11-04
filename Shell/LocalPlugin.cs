using System.Windows;
using System;
using System.Windows.Input;
using System.ComponentModel;
using Shell.Util;
using Interfaces;
using System.AddIn.Pipeline;
using System.Diagnostics;
using System.Runtime.Remoting;

namespace Shell
{
    /// <summary>
    /// This class keep a reference to the remote plugin. 
    /// </summary>
    public class LocalPlugin : INotifyPropertyChanged, IDisposable
    {
        
        public LocalPlugin(IPlugin plugin)
        {
            Plugin = plugin;
            eventSink = new EventSink();
            eventSink.OnHostToClient += eventSink_OnHostToClient;
            plugin.RegisterSink(eventSink);
            //Plugin.EventOccurred += Plugin_EventOccurred;
            View = FrameworkElementAdapters.ContractToViewAdapter(plugin.NativeHandleContract);
            SendMessage = new DelegateCommand(ExecuteSendMessage);
        }

        void eventSink_OnHostToClient(PluginEventData data)
        {
            RaiseMessageFromPluginEvent(data);
        }

        private EventSink eventSink;

        //void Plugin_EventOccurred(object sender, PluginEventEventArgs e)
        //{
        //    Debug.WriteLine("Received event from host: " + e.EventName + " with payload " + e.EventPayload);
        //}

        private void ExecuteSendMessage()
        {
            try
            {
                Debug.WriteLine("Send message:" + MessageToSend + " returned: " + Plugin.SendMessage(MessageToSend));
            }
            catch (RemotingException)
            {
                //We are communicating with another process, everything can occour, I do not want to make main program chrash
            }
           
        }

     


        public FrameworkElement View { get; private set; }

        public IPlugin Plugin { get; set; }

        public string Title { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing) 
        {
            if (disposing) 
            {
                if (Disposed != null)
                {

                    Disposed(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler Disposed;

        public ICommand SendMessage { get; private set; }

        public string MessageToSend
        {
            get { return _messageToSend; }
            set { 
                _messageToSend = value;
                RaisePropertyChanged("MessageToSend");
            }
        }
        private string _messageToSend;

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event EventHandler<MessageFromPluginEventArgs> MessageFromPlugin;

        private void RaiseMessageFromPluginEvent(PluginEventData eventData)
        {
            if (MessageFromPlugin != null)
            {
                MessageFromPlugin(this, new MessageFromPluginEventArgs(eventData));
            }
        }
    }

    public class MessageFromPluginEventArgs : EventArgs 
    {
        public MessageFromPluginEventArgs(PluginEventData pluginEventdata)
        {
            PluginEventData = pluginEventdata;
        }
        public PluginEventData PluginEventData { get; private set; }
    }
}

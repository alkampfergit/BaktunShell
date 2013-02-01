﻿using System.Windows;
using System;
using System.Windows.Input;
using System.ComponentModel;
using Shell.Util;
using Interfaces;
using System.AddIn.Pipeline;
using System.Diagnostics;

namespace Shell
{
    class LocalPlugin : INotifyPropertyChanged, IDisposable
    {
        public LocalPlugin(IPlugin plugin)
        {
            Plugin = plugin;
            View = FrameworkElementAdapters.ContractToViewAdapter(plugin.NativeHandleContract);
            SendMessage = new DelegateCommand(ExecuteSendMessage);
        }

        private void ExecuteSendMessage()
        {
            Debug.WriteLine("Send message:" + MessageToSend + " returned: " + Plugin.SendMessage(MessageToSend));
        }

        public FrameworkElement View { get; private set; }

        public IPlugin Plugin { get; set; }

        public string Title { get; set; }

        public void Dispose()
        {
            if (Disposed != null)
            {
                Disposed(this, EventArgs.Empty);
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
    }
}

using System.Windows;
using System;
using System.Windows.Input;
using System.ComponentModel;
using Shell.Util;

namespace Shell
{
    class Plugin : INotifyPropertyChanged, IDisposable
    {
        public Plugin(FrameworkElement view)
        {
            View = view;
            SendMessage = new DelegateCommand(ExecuteSendMessage);
        }

        private void ExecuteSendMessage()
        {
      
        }

        public FrameworkElement View { get; private set; }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interfaces;

namespace SamplePlugin
{
    /// <summary>
    /// Interaction logic for WebBrowser.xaml
    /// </summary>
    public partial class WebBrowser : UserControl, IActionableFrameworkElement
    {
        public WebBrowser()
        {
            InitializeComponent();
            theBrowser.Navigated += theBrowser_Navigated;
        }

        void theBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            OnEventOccurred("navigated", e.Uri.AbsoluteUri);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            theBrowser.Navigate(txtUrl.Text);
        }

        public string SendMessage(string message)
        {
            String[] command = message.Split('|');
            if (command[0].Equals("navigate", StringComparison.OrdinalIgnoreCase  ) )
            {
                Dispatcher.Invoke ((Action) (() => theBrowser.Navigate(command[1])));
                return "Started navigation to: " + command[1];
            }
            return "Unknown command for a Web Browser";
        }

        public event PluginEventDelegate EventOccurred;

        protected void OnEventOccurred(String eventName, String eventPayload)
        {
            var temp = EventOccurred;
            if (temp != null)
            {
                temp(new PluginEventData(eventName, eventPayload));
            }
        }
    }
}

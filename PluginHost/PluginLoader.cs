﻿using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Windows;
using Interfaces;
using System.Runtime.Remoting;

namespace PluginHost
{
    class PluginLoader : MarshalByRefObject, IPluginLoader
    {
        public IPlugin LoadPlugin(string assembly, string typeName)
        {
            if (String.IsNullOrEmpty(assembly)) throw new ArgumentNullException("assembly");
            if (String.IsNullOrEmpty(typeName)) throw new ArgumentNullException("typeName");

            Console.WriteLine("Loading plugin {0},{1}", assembly, typeName);

            try
            {
                Func<string, string, IPlugin> createOnUiThread = CreateOnUiThread;
                var plugin = (IPlugin) Program.Dispatcher.Invoke(createOnUiThread, assembly, typeName);

                return plugin;
            }
            catch (Exception ex)
            {
                var message = String.Format("Error loading type '{0}' from assembly '{1}'. {2}", 
                    assembly, typeName, ex.Message);

                throw new ApplicationException(message, ex);
            }
        }

        private static IPlugin CreateOnUiThread(string assembly, string typeName)
        {
            var handle = Activator.CreateInstance(assembly, typeName);
            if (handle == null) throw new InvalidOperationException("Activator.CreateInstance() returned null");

            var obj = handle.Unwrap();
            if (obj == null) throw new InvalidOperationException("Unwrap() returned null");

            if (!(obj is FrameworkElement))
            {
                var message = string.Format("Object of type {0} cannot be loaded as plugin " +
                    "because it does not derive from FrameworkElement", typeName);

                throw new InvalidOperationException(message);
            }

            var element = (FrameworkElement)obj;
            var contract = FrameworkElementAdapters.ViewToContractAdapter(element);

            var insulator = new NativeHandleContractInsulator(contract);
            var plugin = new PluginBase(insulator, element);

            return plugin;
        }

        public void Terminate()
        {
            System.Environment.Exit(0);
        }
    }
}

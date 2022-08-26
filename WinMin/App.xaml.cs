using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WinMin
{
    public partial class App : Application
    {
        public App() : base()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Crash occurred, report this error: \n" + e.Exception.Message, "Crash Log", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                Process.Start("https://github.com/awesomegamergame/WinMin/issues");
            Current.Shutdown();
        }
    }
}

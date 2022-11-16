using System;
using System.IO;
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
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using WinMin.Functions;

namespace WinMin
{
    public partial class App : Application
    {
        public string rootPath = @"C:\Users\Public\WinMin";
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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (File.Exists($"{rootPath}\\Program.txt"))
            {
                string txt = File.ReadAllText($"{rootPath}\\Program.txt");
                string fixedString = ShortcutFix.ResolveShortcut(txt);
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = $@"{fixedString}";
                try
                {
                    Process.Start(info);
                }
                catch(FileNotFoundException)
                {
                    File.Delete($"{rootPath}\\Program.txt");
                    MessageBox.Show("Something with finding the file failed");
                    Current.Shutdown();
                }
                File.Delete($"{rootPath}\\Program.txt");
                Current.Shutdown();
            }
            else
            {
                MainWindow window = new MainWindow();
                window.Show();
            }
        }
        
    }
}

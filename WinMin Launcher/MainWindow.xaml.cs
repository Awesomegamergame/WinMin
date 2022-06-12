using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace WinMin_Launcher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(StartThread);
            thread.Start();
        }
        private void StartThread()
        {
            Thread.Sleep(1000);
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "C:\\Users\\Public\\WinMin\\psexec.exe",
                Arguments = $@"-u {Environment.MachineName}\Administrator -p Password1 -d C:\Users\Public\WinMin\WinMin.exe",
                WindowStyle = ProcessWindowStyle.Minimized
            };
            Process.Start(startInfo);
            Dispatcher.Invoke(() => 
            {
                Application.Current.Shutdown();
            });
        }
    }
}

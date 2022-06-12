using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                App.Current.Shutdown();
            });
        }
    }
}

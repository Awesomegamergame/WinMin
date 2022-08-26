using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Security.Principal;

namespace WinMin_Launcher
{
    public partial class MainWindow : Window
    {
        public string rootPath = AppDomain.CurrentDomain.BaseDirectory;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(StartThread)
            {
                IsBackground = true
            };
            thread.Start();
        }
        private void StartThread()
        {
            if (!File.Exists($"{rootPath}\\UserID.txt"))
                File.WriteAllText($"{rootPath}\\UserID.txt", WindowsIdentity.GetCurrent().User.Value);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "C:\\Users\\Public\\WinMin\\psexec.exe",
                Arguments = $@"/accepteula -u {Environment.MachineName}\Administrator -p Password1 -d C:\Users\Public\WinMin\WinMin.exe",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            Dispatcher.Invoke(() => 
            {
                Application.Current.Shutdown();
            });
        }
    }
}

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
        public string rootPath = @"C:\Users\Public\WinMin";

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
            if (!File.Exists($"{rootPath}\\UserName.txt"))
                File.WriteAllText($"{rootPath}\\UserName.txt", Environment.UserName);
            if (!File.Exists($"{rootPath}\\UserDomain.txt"))
                File.WriteAllText($"{rootPath}\\UserDomain.txt", Environment.UserDomainName);

            if (File.Exists($"{rootPath}\\Admin"))
            {
                File.Delete($"{rootPath}\\Admin");
                try
                {
                    Process.Start("C:\\Users\\Public\\WinMin\\WinMin.exe");
                }
                catch(Exception)
                {
                    if (File.Exists($"{rootPath}\\Program.txt"))
                    {
                        File.Delete($"{rootPath}\\Program.txt");
                    }
                    MessageBox.Show("You didn't hit yes on the uac prompt WinMin loading has failed");
                }
                Dispatcher.Invoke(() =>
                {
                    Application.Current.Shutdown();
                });
            }
            else
            {
                File.WriteAllText($"{rootPath}\\Admin", "");
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "C:\\Users\\Public\\WinMin\\psexec.exe",
                    Arguments = $@"/accepteula -u {Environment.MachineName}\Administrator -p Password1 -d ""C:\Users\Public\WinMin\WinMin Launcher.exe""",
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
}

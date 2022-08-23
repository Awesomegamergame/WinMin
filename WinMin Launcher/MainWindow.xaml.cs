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
            //TODO: Get the user from WinMin Launcher because they are a different user when WinMin is launcher
            //      So we need to somehow give the users SID from this line below into the WinMin app and create
            //      a text file to pass the last logged in user to the launchers startup process
            //MessageBox.Show(System.Security.Principal.WindowsIdentity.GetCurrent().User.Value);
            Thread.Sleep(1000);
            //TODO: Need to figure out why psexec takes a long time to execute and either reduce the time
            //      the time it takes to launch WinMin or make the launcher stay open longer and remove the thread sleep
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "C:\\Users\\Public\\WinMin\\psexec.exe",
                //TODO: Might be able use pass the users id into the arguments for psexec for WinMin to use but i dont know if it will work
                Arguments = $@"/accepteula -u {Environment.MachineName}\Administrator -p Password1 -d C:\Users\Public\WinMin\WinMin.exe",
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

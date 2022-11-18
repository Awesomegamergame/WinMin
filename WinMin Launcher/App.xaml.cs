using System;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using WinMin.Functions;
using Newtonsoft.Json;

namespace WinMin_Launcher
{
    public partial class App : Application
    {
        public readonly string rootPath = @"C:\Users\Public\WinMin";
        public readonly string patchPath = "C:\\Users\\Public\\WinMin\\Patches";
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if(args.Length == 2)
            {
                if (args[1].Equals("startup"))
                {
                    
                    /*
                     * The registry loading works for every plugin that is installed but it import the keys before the group policy
                     * editer changes the value so the value get changed back to default so i either need to delay loading untill
                     * the group policy editer changes the keys are change them and detect if they get changed back
                     */

                    foreach(string directory in Directory.GetDirectories($"{patchPath}\\Installed\\"))
                    {
                        string json = File.ReadAllText($"{directory}\\manifest.json");
                        WMManifest manifest = JsonConvert.DeserializeObject<WMManifest>(json);
                        foreach (string keyPath in manifest.patchFiles)
                        {
                            Process process = new Process();
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                WindowStyle = ProcessWindowStyle.Hidden,
                                FileName = "cmd.exe",
                                Arguments = $"/C reg import \"{patchPath}\\Installed\\{manifest.name}\\{keyPath}\""
                            };
                            process.StartInfo = startInfo;
                            process.Start();
                            process.WaitForExit();
                        }
                    }

                    

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

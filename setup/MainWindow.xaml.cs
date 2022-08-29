using System;
using System.IO;
using System.Collections.Generic;
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
using System.Diagnostics;

namespace setup
{
    public partial class MainWindow : Window
    {
        public string windows;
        public bool WinMin;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Installer
        private void Install_Click(object sender, RoutedEventArgs e)
        {
            if (AgreeBox.IsChecked == true)
            {
                if (WinMin)
                {
                    AgreeBox.IsEnabled = false;
                    Install.IsEnabled = false;
                    Cancel.IsEnabled = false;
                    try
                    {
                        if (File.Exists($"{windows}:\\Users\\Public\\WinMin\\UserName.txt"))
                        {
                            string userName = File.ReadAllText($"{windows}:\\Users\\Public\\WinMin\\UserName.txt");
                            if (File.Exists($"{windows}:\\Users\\{userName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk"))
                            {
                                File.Delete($"{windows}:\\Users\\{userName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk");
                            }
                        }
                        DeleteDirectory($"{windows}:\\Users\\Public\\WinMin");
                        if (File.Exists($"{windows}:\\Windows\\System32\\Tasks_Migrated\\WinMin"))
                            File.Delete($"{windows}:\\Windows\\System32\\Tasks_Migrated\\WinMin");
                        if (File.Exists($"{windows}:\\Windows\\System32\\Tasks_Migrated\\WinMin Startup"))
                            File.Delete($"{windows}:\\Windows\\System32\\Tasks_Migrated\\WinMin Startup");
                        if (File.Exists($"{windows}:\\Windows\\System32\\Tasks\\WinMin"))
                            File.Delete($"{windows}:\\Windows\\System32\\Tasks\\WinMin");
                        if (File.Exists($"{windows}:\\Windows\\System32\\Tasks\\WinMin Startup"))
                            File.Delete($"{windows}:\\Windows\\System32\\Tasks\\WinMin Startup");
                        if (File.Exists($"{windows}:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk"))
                            File.Delete($"{windows}:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk");
                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = $"/C reg load HKLM\\soft {windows}:\\Windows\\System32\\config\\SOFTWARE"
                        };
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                        Process process2 = new Process();
                        ProcessStartInfo startInfo2 = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = $"/C reg import {AppDomain.CurrentDomain.BaseDirectory}\\WinMinFiles\\WinMinRegUninstall.reg"
                        };
                        process2.StartInfo = startInfo2;
                        process2.Start();
                        process2.WaitForExit();
                        MessageBox.Show("Uninstallation complete. Please remove the flash drive then click ok to reboot.");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Critical Error: " + ex.Message);
                        AgreeBox.IsEnabled = true;
                        Install.IsEnabled = true;
                        Cancel.IsEnabled = true;
                    }
                }
                else
                {   
                    AgreeBox.IsEnabled = false;
                    Install.IsEnabled = false;
                    Cancel.IsEnabled = false;
                    try
                    {

                        if (!Directory.Exists($"{windows}:\\Users\\Public\\WinMin"))
                            Directory.CreateDirectory($"{windows}:\\Users\\Public\\WinMin");
                        File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin.exe", $"{windows}:\\Users\\Public\\WinMin\\WinMin.exe", true);
                        File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\Newtonsoft.Json.dll", $"{windows}:\\Users\\Public\\WinMin\\Newtonsoft.Json.dll", true);
                        File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin Launcher.exe", $"{windows}:\\Users\\Public\\WinMin\\WinMin Launcher.exe", true);
                        File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\psexec.exe", $"{windows}:\\Users\\Public\\WinMin\\psexec.exe", true);
                        File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin", $"{windows}:\\Windows\\System32\\Tasks\\WinMin", true);
                        File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin Startup", $"{windows}:\\Windows\\System32\\Tasks\\WinMin Startup", true);
                        if (Directory.Exists($"{windows}:\\Windows\\System32\\Tasks_Migrated"))
                        {
                            File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin", $"{windows}:\\Windows\\System32\\Tasks_Migrated\\WinMin", true);
                            File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin Startup", $"{windows}:\\Windows\\System32\\Tasks_Migrated\\WinMin Startup", true);
                        }
                        File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin.lnk", $"{windows}:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk", true);
                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = $"/C reg load HKLM\\soft {windows}:\\Windows\\System32\\config\\SOFTWARE"
                        };
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                        Process process2 = new Process();
                        ProcessStartInfo startInfo2 = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = $"/C reg import {AppDomain.CurrentDomain.BaseDirectory}\\WinMinFiles\\WinMinReg.reg"
                        };
                        process2.StartInfo = startInfo2;
                        process2.Start();
                        process2.WaitForExit();
                        MessageBox.Show("Installation complete. Please remove the flash drive then click ok to reboot.");
                        Application.Current.Shutdown();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Critical Error: " + ex.Message);
                        AgreeBox.IsEnabled = true;
                        Install.IsEnabled = true;
                        Cancel.IsEnabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("You must agree by checking the box to continue.");
            }    
        }
        #endregion

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel?", "WinMin", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void Install_Initialized(object sender, EventArgs e)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    foreach (var folder in Directory.GetDirectories(drive.Name))
                    {
                        if (folder.Contains("Windows"))
                        {
                            foreach (var file in Directory.GetFiles(folder))
                            {
                                if (file.Contains("explorer.exe"))
                                {
                                    windows = file.Substring(0, file.Length - 22);
                                }
                            }
                        }
                    }
                }
            }
            if (Directory.Exists($@"{windows}:\Users\Public\WinMin"))
            {
                WinMin = true;
                Install.Content = "Uninstall";
            }
            else
            {
                WinMin = false;
                Install.Content = "Install";
            }
        }
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
    }
}

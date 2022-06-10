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
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Install_Click(object sender, RoutedEventArgs e)
        {
            if (AgreeBox.IsChecked == true)
            {
                AgreeBox.IsEnabled = false;
                Install.IsEnabled = false;
                Cancel.IsEnabled = false;
                try
                {
                    if (!Directory.Exists("C:\\Users\\Public\\WinMin"))
                        Directory.CreateDirectory("C:\\Users\\Public\\WinMin");
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin.exe", "C:\\Users\\Public\\WinMin\\WinMin.exe", true);
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin Launcher.exe", "C:\\Users\\Public\\WinMin\\WinMin Launcher.exe", true);
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\psexec.exe.exe", "C:\\Users\\Public\\WinMin\\psexec.exe.exe", true);
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin", "C:\\Windows\\System32\\Tasks\\WinMin", true);
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin", "C:\\Windows\\System32\\Tasks_Migrated\\WinMin", true);
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin Startup", "C:\\Windows\\System32\\Tasks\\WinMin Startup", true);
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinFiles\\WinMin Startup", "C:\\Windows\\System32\\Tasks_Migrated\\WinMin Startup", true);
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Minimized,
                        FileName = "cmd.exe",
                        Arguments = "/C reg load HKLM\\soft C:\\Windows\\System32\\config\\SOFTWARE"
                    };
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    Process process2 = new Process();
                    ProcessStartInfo startInfo2 = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Minimized,
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
            else
            {
                MessageBox.Show("You must agree by checking the box to continue.");
            }    
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel?", "WinMin", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}

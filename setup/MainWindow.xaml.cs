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
                AgreeBox.IsEnabled = false;
                Install.IsEnabled = false;
                Cancel.IsEnabled = false;
                try
                {
                    Directory.Move($"{windows}:\\Program Files\\ThreatLocker", $"{windows}:\\Program Files\\ThreatLocker2");
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

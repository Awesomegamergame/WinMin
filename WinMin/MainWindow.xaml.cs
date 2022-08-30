using Microsoft.Win32;
using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using WinMin.Functions;
using System.Windows.Controls;
using System.Linq;

namespace WinMin
{
    public partial class MainWindow : Window
    {
        public static MainWindow window;
        readonly static string userID = File.ReadAllText(@"C:\Users\Public\WinMin\UserID.txt");
        readonly static string explorerKey = $@"{userID}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        public MainWindow()
        {
            Updater.CheckInternetState();
            window = this;
            InitializeComponent();
            if (Updater.IsOnline) { Updater.Update(); }
            RegistryChanger.CreateJson();
            if (File.Exists("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk"))
            {
                string userName = File.ReadAllText($"C:\\Users\\Public\\WinMin\\UserName.txt");
                File.Move("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk", $"C:\\Users\\{userName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk");
            }
        }

        private void RegB_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("regedit.exe");
        }

        private void CmdB_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe");
        }


        #region Updates
    private void No_Click(object sender, RoutedEventArgs e)
        {
            UpdateScreen.Visibility = Visibility.Collapsed;
            Yes.Visibility = Visibility.Collapsed;
            No.Visibility = Visibility.Collapsed;
            UpdateText1.Visibility = Visibility.Collapsed;
            UpdateText2.Visibility = Visibility.Collapsed;
            LocalVersion.Visibility = Visibility.Collapsed;
            LocalVersionNumber.Visibility = Visibility.Collapsed;
            OnlineVersionNumber.Visibility = Visibility.Collapsed;
            OnlineVersion.Visibility = Visibility.Collapsed;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Yes.Visibility = Visibility.Collapsed;
            No.Visibility = Visibility.Collapsed;
            UpdateProgress.Visibility = Visibility.Visible;
            if (Updater.VersionDetector == 1)
            {
                Updater.UpdaterVersion();
                Updater.VersionDetector = 0;
            }
            else if (Updater.VersionDetector == 2)
            {
                Updater.UpdaterVersion();
                Updater.VersionDetector = 0;
            }
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryChanger.LoadUserRegistry(Settings, "SettingsPageVisibility", explorerKey);
            RegistryChanger.LoadUserRegistry(RightClick, "NoViewContextMenu", explorerKey);
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            string keyName = "SettingsPageVisibility";
            string oldValue = RegistryChanger.DefaultReadValue(keyName);
            RegistryChanger.SetUserRegistry(Settings, keyName, "", oldValue, explorerKey, RegistryValueKind.String);
        }

        private void RightClick_Click(object sender, RoutedEventArgs e)
        {
            string keyName = "NoViewContextMenu";
            string oldValue = RegistryChanger.DefaultReadValue(keyName);
            RegistryChanger.SetUserRegistry(RightClick, keyName, "0", oldValue, explorerKey, RegistryValueKind.DWord);
        }
    }
}

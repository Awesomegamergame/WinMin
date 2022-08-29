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
        readonly string userID = File.ReadAllText(@"C:\Users\Public\WinMin\UserID.txt");
        public MainWindow()
        {
            Updater.CheckInternetState();
            window = this;
            InitializeComponent();
            if (Updater.IsOnline) { Updater.Update(); }
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

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            int button = (int)(sender as Button).Tag;
            if (button == 1)
            {
                RegistryKey key = Registry.Users.OpenSubKey($@"{userID}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);

                if (key != null)
                {
                    key.SetValue("SettingsPageVisibility", "");
                    key.Close();
                }
                Settings.Tag = 0;
                Settings.Content = "Disable";
            }
            else
            {
                RegistryKey key = Registry.Users.OpenSubKey($@"{userID}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);

                if (key != null)
                {
                    key.SetValue("SettingsPageVisibility", "ShowOnly:easeofaccess-audio;easeofaccess-closedcaptioning;easeofaccess-colorfilter;easeofaccess-mousepointer;easeofaccess-cursor;easeofaccess-display;easeofaccess-eyecontrol;fonts;easeofaccess-highcontrast;easeofaccess-keyboard;easeofaccess-magnifier;easeofaccess-mouse;easeofaccess-narrator;easeofaccess-otheroptions;easeofaccess-speechrecognition;sound;typing;camera;privacy-webcam;tabletmode;bluetooth;defaultapps;regionlanguage");
                    key.Close();
                }
                Settings.Tag = 1;
                Settings.Content = "Enable";
            }
        }

        private void Settings_Initialized(object sender, EventArgs e)
        {
            RegistryKey key = Registry.Users.OpenSubKey($@"{userID}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);

            string value;
            if (key != null)
            {
                //Key location exists
                if (key.GetValueNames().Contains("SettingsPageVisibility"))
                {
                    //Key Exists
                    value = key.GetValue("SettingsPageVisibility").ToString();
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        //If Key is unblocked from WinMin
                        Settings.Tag = 0;
                        Settings.Content = "Disable";
                    }
                    else
                    {
                        //If Key is still blocked
                        Settings.Tag = 1;
                        Settings.Content = "Enable";
                    }
                }
                else
                {
                    //Key doesnt Exist
                    Settings.IsEnabled = false;
                    Settings.Content = "Not Blocked";
                }
            }
            else
            {
                //Key Location doesnt exists
                Settings.IsEnabled = false;
            }
        }
    }
}

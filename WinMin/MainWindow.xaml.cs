using Microsoft.Win32;
using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using WinMin.Functions;
using System.Windows.Controls;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using System.IO.Compression;

namespace WinMin
{
    public partial class MainWindow : Window
    {
        public static MainWindow window;
        readonly static string userID = File.ReadAllText(@"C:\Users\Public\WinMin\UserID.txt");
        readonly static string explorerKey = $@"{userID}\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        readonly string rootPath = @"C:\Users\Public\WinMin";

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        public MainWindow()
        {
            Updater.CheckInternetState();
            window = this;
            InitializeComponent();
            if (Updater.IsOnline) { Updater.Update(); }
            RegistryChanger.CreateJson();
            #region WinMin wmpatch Extension
            if (File.Exists("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk"))
            {
                string userName = File.ReadAllText($"C:\\Users\\Public\\WinMin\\UserName.txt");
                File.Move("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk", $"C:\\Users\\{userName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\WinMin.lnk");
            }
            if (!File.Exists($"{rootPath}\\WMPatch.reg"))
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WinMin.WMPatch.reg");
                FileStream fileStream = new FileStream($"{rootPath}\\WMPatch.reg", FileMode.CreateNew);
                for (int i = 0; i < stream.Length; i++)
                    fileStream.WriteByte((byte)stream.ReadByte());
                fileStream.Close();
                string text = File.ReadAllText($"{rootPath}\\WMPatch.reg");
                text = text.Replace("Name", userID);
                File.WriteAllText($"{rootPath}\\WMPatch.reg", text);

                //Load the registry key
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C reg import C:\\Users\\Public\\WinMin\\WMPatch.reg"
                };
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
            #endregion
        }

        private void RegB_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("regedit.exe");
        }

        private void CmdB_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe");
        }

        private void ProgramRun_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "All files (*.*)|*.*"
            };

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = openFileDialog1.FileName;
                Process.Start(path);
            }
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
            RegistryChanger.LoadUserRegistry(RightClickTask, "NoTrayContextMenu", explorerKey);
            PluginManager pluginManager = new PluginManager();
            //Loaded Installed Must Be Called First In The PluginManager Class
            pluginManager.LoadInstalled();
            pluginManager.WMPatchInstaller();
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

        private void RightClickTask_Click(object sender, RoutedEventArgs e)
        {
            string keyName = "NoTrayContextMenu";
            string oldValue = RegistryChanger.DefaultReadValue(keyName);
            RegistryChanger.SetUserRegistry(RightClickTask, keyName, "0", oldValue, explorerKey, RegistryValueKind.DWord);
        }

        private void RegL_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "Registry files (*.reg)|*.reg"
            };

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = openFileDialog1.FileName;
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = "cmd.exe",
                    Arguments = $"/C reg import \"{path}\""
                };
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                MessageBox.Show("Registry file loaded");
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            PatchesMenu.Visibility = Visibility.Collapsed;
            MenuButton.Visibility = Visibility.Collapsed;
            APatches.Visibility = Visibility.Collapsed;
            IPatches.Visibility = Visibility.Collapsed;
            WMAvailable.Visibility = Visibility.Collapsed;
            WMInstalled.Visibility = Visibility.Collapsed;
        }

        private void WMPatchB_Click(object sender, RoutedEventArgs e)
        {
            PatchesMenu.Visibility = Visibility.Visible;
            MenuButton.Visibility = Visibility.Visible;
            APatches.Visibility = Visibility.Visible;
            IPatches.Visibility = Visibility.Visible;
            WMAvailable.Visibility = Visibility.Visible;
            WMInstalled.Visibility = Visibility.Visible;
        }
    }
}

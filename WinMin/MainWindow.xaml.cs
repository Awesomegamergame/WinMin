using System.Diagnostics;
using System.Windows;
using WinMin.Functions;

namespace WinMin
{
    public partial class MainWindow : Window
    {
        public static MainWindow window;
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
        //TODO: Need to add an uninstaller for WinMin this will go either one of two ways:
        //      1. Have the setup program detect if WinMin is installed and remove all of the files (Requires flash drive and needs computer to be boot to drive)
        //      2. Use WinMins admin perms to remove all of the files created and the tasks and exceptions (probably using cmd or powershell) and then have WinMin delete itself (Probably going to be the one)
        //      First one will work but is slow and needs a flash drive the second way might not work but is the fastest removal way 
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
    }
}

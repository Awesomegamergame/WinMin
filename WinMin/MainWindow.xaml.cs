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

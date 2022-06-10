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
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\WinMinSetup.exe", "C:\\Windows\\WinMinSetup.exe", true);
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

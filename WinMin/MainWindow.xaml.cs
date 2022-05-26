using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinMin
{
    public partial class MainWindow : Window
    {
        public static MainWindow window;
        public MainWindow()
        {
            window = this;
            InitializeComponent();
        }

        private void RegB_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("regedit.exe");
        }

        private void CmdB_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe");
        }
    }
}

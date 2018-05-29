using AsyncWpf;
using NetworkWpf;
using System;
using System.Collections.Generic;
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

namespace ProDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Common_Click(object sender, RoutedEventArgs e)
        {
            if (sender == AsyncBtn)
            {
                AsyncWindow asyncWindow = new AsyncWindow();
                asyncWindow.Show();
                this.Close();
            }
            else if(sender == NetworkBtn)
            {
                NetworkWindow netWindow = new NetworkWindow();
                netWindow.Show();
                this.Close();
            }
            else if (sender == ParallelBtn)
            {
                //ParallelWindow parallelWindow = new ParallelWindow();
                //parallelWindow.Show();
                //this.Close();
            }
        }
    }
}

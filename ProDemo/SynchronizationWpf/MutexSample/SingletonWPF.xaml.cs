using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SynchronizationWpf.MutexSample
{
    /// <summary>
    /// SingletonWPF.xaml 的交互逻辑
    /// </summary>
    public partial class SingletonWPF : Window
    {
        public SingletonWPF()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            bool mutexCreated;
            var mutex = new Mutex(false, "SingletonWinAppMutex", out mutexCreated);
            if (mutexCreated)
            {
                MessageBox.Show("你只能开启一个应用实例");
                Application.Current.Shutdown();
            }
            base.OnInitialized(e);
        }
    }
}

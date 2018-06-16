using SynchronizationWpf.ThreadingIssues;
using SynchronizationWpf.SynchronizationSample;
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
using SynchronizationWpf.AsyncDelegateSample;

namespace SynchronizationWpf
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

        private void RaceConditionBtn_Click(object sender, RoutedEventArgs e)
        {
            // 此段代码新建了一个StateObject对象，它由所有任务共享。
            // 通过使用传递给Task的Run方法的lambda表达式调用Racecondition方法来创建Task对象。
            // 然后，主线程等待用户输入。但是，因为可能出现争用，所以程序很有可能在读取用户输入前就挂起：
            var state = new StateObject();
            for (int i = 0; i < 50000; i++)
            {
                Task.Run(() => new SampleTask().RaceCondition(state));
            }
        }


        private void DealLockBtn_Click(object sender, RoutedEventArgs e)
        {
            var state1 = new StateObject();
            var state2 = new StateObject();

            new Task(new SampleTask(state1, state2).Deallock1).Start();
            new Task(new SampleTask(state1, state2).Deallock2).Start();
        }

        private void RaceConditionSampleBtn_Click(object sender, RoutedEventArgs e)
        {
            // ShareState和Job都不锁定
            //ShowTb.Text += new SynchronizationSampleMain().DoTheJobByNoLockAll();

            // ShareState，不锁定
            // Job中方法DoTheJobByJobMethodLock，锁定
            ShowTb.Text += new SynchronizationSampleMain().DoTheJobByJobMethodLock();

            // ShareStatePropertyLock中的属性，锁定
            // Job方法DoTheJobByShareStatePropertyLock，不锁定
            //ShowTb.Text += new SynchronizationSampleMain().DoTheJobByShareStatePropertyLock();

            //  ShareStatePropertyLock中的属性，锁定
            // Job方法DoTheJobByShareStatePropertyLockAndJobMethodLock，锁定
            //ShowTb.Text += new SynchronizationSampleMain().DoTheJobByShareStatePropertyLockAndJobMethodLock();

            // ShareStateMethodLock中的方法，锁定
            // Job方法DoTheJobByShareStateMethodLock，不锁定
            //ShowTb.Text += new SynchronizationSampleMain().DoTheJobByShareStateMethodLock();

            // ShareStateMethodLock中的方法，锁定
            // Job方法DoTheJobByShareStateMethodLockAndJobMethodLock，锁定
            //ShowTb.Text += new SynchronizationSampleMain().DoTheJobByShareStateMethodLockAndJobMethodLock();
            //总共循环次数713992
            //总共循环次数1000000
            //总共循环次数247661
            //总共循环次数1000000
            //总共循环次数1000000
            //总共循环次数1000000
        }

        private void AsyncDelegateBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowTb.Text += new AsyncDelegate().StartDemo();
        }
    }
}

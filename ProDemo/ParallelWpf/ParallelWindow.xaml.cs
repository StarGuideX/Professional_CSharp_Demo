using ParallelWpf.ParallelSamples;
using ParallelWpf.TaskSamples;
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

namespace ParallelWpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ParallelWindow : Window
    {
        private ParallelSampleClass parallelSampleClass = new ParallelSampleClass();
        private TaskSamplesClass taskSamplesClass = new TaskSamplesClass();
        public ParallelWindow()
        {
            InitializeComponent();
        }

        private void ParallelForBtn_Click(object sender, RoutedEventArgs e)
        {
            parallelSampleClass.ParallelFor();
        }

        private void ParallelForWithAsyncBtn_Click(object sender, RoutedEventArgs e)
        {
            parallelSampleClass.ParallelForWithAsync();
        }

        private void StopParallelForEarlyBtn_Click(object sender, RoutedEventArgs e)
        {
            parallelSampleClass.StopParallelForEarly();
        }
        private void ParallelForWithInitBtn_Click(object sender, RoutedEventArgs e)
        {
            parallelSampleClass.ParallelForWithInit();
        }

        private void ParallForEachBtn_Click(object sender, RoutedEventArgs e)
        {
            parallelSampleClass.ParallForEach();
        }

        private void ParallelInvokeBtn_Click(object sender, RoutedEventArgs e)
        {
            parallelSampleClass.ParallelInvoke();
        }

        private void ThreadUsingThreadPoolBtn_Click(object sender, RoutedEventArgs e)
        {
            taskSamplesClass.ThreadUsingThreadPool();
        }

        private void RunSynchronousTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            taskSamplesClass.RunSynchronousTask();
        }

        private void LongRunningTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            taskSamplesClass.LongRunningTask();
        }

        private void TaskWithResultDemoBtn_Click(object sender, RoutedEventArgs e)
        {
            taskSamplesClass.TaskWithResultDemo();
        }
    }
}

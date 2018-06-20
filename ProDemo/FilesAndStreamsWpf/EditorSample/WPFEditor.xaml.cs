using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace FilesAndStreamsWpf.EditorSample
{
    /// <summary>
    /// WPFEditor.xaml 的交互逻辑
    /// </summary>
    public partial class WPFEditor : Window
    {
        public WPFEditor()
        {
            InitializeComponent();
        }

        private void OnOpen(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                Title = "Simple Editor - Open File",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "文本文档{*.txt}|*.txt|所有文件|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (dlg.ShowDialog() == true)
            {
                myText.Text = File.ReadAllText(dlg.FileName);
            }
        }

        private void OnSave(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                Title = "Simple Editor - Save As",
                DefaultExt = "txt",
                Filter = "文本文档{*.txt}|*.txt|所有文件|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllText(dlg.FileName,myText.Text);
            }
        }
    }
}

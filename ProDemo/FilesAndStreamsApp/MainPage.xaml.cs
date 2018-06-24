using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FilesAndStreamsApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnOpen句柄首先启动对话框，用户可以在其中选择文件。
        /// 要打开文件，FileOpenPicker是首选的类型。可以配置此选择器，为用户定义建议的开始位置。
        /// 将SuggestedStartLocation设置为PickerLocationId.DocumentsLibrary—打开用户的文档文件夹。
        /// PickerLocationId是定义各种特殊文件夹的枚举。
        /// 接下来，FileTypeFilter集合指定应该为用户列出的文件类型。
        /// 最后，方法PickSingleFileAsync返回用户选择的文件。
        /// 为了让用户选择多个文件，可以使用方法PickMultipleFilesAsync。
        /// 这个方法返回StorageFile。StorageFile是在Windows.Storage名称空间中定义的。
        /// 这个类相当于FileInfo类，用于打开、创建、复制、移动和删除文件
        /// </summary>
        public async void OnOpen()
        {
            try
            {
                var picker = new FileOpenPicker()
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };

                picker.FileTypeFilter.Add(".txt");

                StorageFile file = await picker.PickSingleFileAsync();

                if (file != null)
                {
                    IRandomAccessStreamWithContentType stream = await file.OpenReadAsync();
                    using (var reader = new DataReader(stream))
                    {
                        await reader.LoadAsync((uint)stream.Size);

                        myText.Text = reader.ReadString((uint)stream.Size);
                    }
                }

            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message, "Error").ShowAsync();
            }
        }

        /// <summary>
        /// 保存文档时，调用Onsave()方法。
        /// 首先，FileSavePicker用于允许用户选择文档，与FileOpenPicker类似。
        /// 接下来，使用OpenTransactedWriteAsync打开文件。
        /// NTFS文件系统支持事务；这些都不包含在.NETFramework中，但可用于Windows运行库。
        /// OpenTransactedWriteAsync返回一个实现了接口IStorageStreamTransaction的StorageStreamTransaction对象。
        /// 这个对象本身并不是流，但是它包含了一个可以用Stream属性引用的流。
        /// 这个属性返回一个IRandomAccessStream流。
        /// 与创建DataReader类似，可以创建一个DataWriter，写入原始数据类型，包括字符串，
        /// StoreAsync方法最后把缓冲区的内容写到流中。
        /// 销毁写入器之前，需要调用CommitAsync方法来提交
        /// </summary>
        public async void OnSave()
        {
            try
            {
                var picker = new FileSavePicker()
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                    SuggestedFileName = "新建文本文档"
                };

                picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });

                StorageFile file = await picker.PickSaveFileAsync();

                if (file != null)
                {
                    using (StorageStreamTransaction tx = await file.OpenTransactedWriteAsync())
                    {
                        IRandomAccessStream stream = tx.Stream;
                        stream.Seek(0);
                        using (var writer = new DataWriter(stream))
                        {
                            writer.WriteString(myText.Text);
                            tx.Stream.Size = await writer.StoreAsync();
                            await tx.CommitAsync();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message, "Error").ShowAsync();
            }
        }

        /// <summary>
        /// 下面开始读取文件。为了把Windows运行库流转换为.NET流用于读取，
        /// 可以使用扩展方法AsStreamForRead。
        /// 这个方法是在程序集System.Runtime.WindowsRuntime
        /// 的System.IO名称空间中定义（必须打开）。
        /// 这个方法创建了一个新的stream对象，来管理IInputstreamo现在，
        /// 可以使用它作为正常的.NET流。
        /// </summary>
        public async void OnOpenDotnet()
        {
            try
            {
                var picker = new FileOpenPicker()
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
                picker.FileTypeFilter.Add(".txt");

                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    IRandomAccessStreamWithContentType wrtStream = await file.OpenReadAsync();
                    Stream stream = wrtStream.AsStreamForRead();

                    using (var reader = new StreamReader(stream))
                    {
                        myText.Text = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message, "Error").ShowAsync();
            }
        }

        /// <summary>
        /// 现在将更改保存到文件中。用于写入的流通过扩展方法AsStreamForWrite转换。
        /// 现在，这个流可以使用StreamWriter类写入。
        /// 代码片段也把UFT-8编码的序言写入文件
        /// </summary>
        public async void OnSaveDotnet()
        {
            try
            {
                var picker = new FileSavePicker()
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                    SuggestedFileName = "新建文本文档"
                };
                picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });

                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    using (StorageStreamTransaction tx = await file.OpenTransactedWriteAsync())
                    {
                        Stream stream = tx.Stream.AsStreamForWrite();
                        using (var writer = new StreamWriter(stream))
                        {
                            byte[] preamble = Encoding.UTF8.GetPreamble();
                            await stream.WriteAsync(preamble, 0, preamble.Length);
                            await writer.WriteAsync(myText.Text);
                            await writer.FlushAsync();
                            tx.Stream.Size = (ulong)stream.Length;
                            await tx.CommitAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message, "Error").ShowAsync();
            }
        }
    }
}

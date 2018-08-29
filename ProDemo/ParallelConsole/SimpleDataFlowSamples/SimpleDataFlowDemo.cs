using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ParallelConsole.SimpleDataFlowSamples
{
    public class SimpleDataFlowDemo
    {
        /// <summary>
        /// ActionBlock示例
        /// </summary>
        public void SimpleDataFlowDemoUsingActionBlock()
        {
            //ActionBlock异步处理消息，把信息写入控制台。
            var processInput = new ActionBlock<string>(s =>
            {
                Console.WriteLine($"用户输入：{s}");
            });

            bool exit = false;
            while (!exit)
            {
                //读取控制台
                string input = Console.ReadLine();
                if (string.Compare(input, "exit", ignoreCase: true) == 0)
                {
                    exit = true;
                }
                else
                {
                    //使用Post()把读入的所有字符串写入ActionBlock
                    processInput.Post(input);
                }
            }
        }

        public void SimpleDataFlowDemoUsingBufferBlock()
        {
            Task t1 = Task.Run(() => Producer());
            Task t2 = Task.Run(async () => await ConsumerAsync());
            Task.WaitAll(t1, t2);
        }


        private BufferBlock<string> s_buffer = new BufferBlock<string>();

        /// <summary>
        /// 此方法从控制台读取字符串，并通过调用post方法把字符串写到BufferBlock中：
        /// </summary>
        private void Producer()
        {
            bool exit = false;
            while (!exit)
            {
                string input = Console.ReadLine();
                if (string.Compare(input, "exit", ignoreCase: true) == 0)
                {
                    exit = true;
                }
                else
                {
                    s_buffer.Post(input);
                }
            }
        }

        /// <summary>
        /// 此方法在一个循环中调用ReceiveAsync()方法来接收BufferBlock中的数据。
        /// </summary>
        /// <returns></returns>
        private async Task ConsumerAsync()
        {
            while (true)
            {
                string data = await s_buffer.ReceiveAsync();
                Console.WriteLine($"用户输入：{data}");
            }
        }

        /// <summary>
        /// 此方法只需要启动管道。调用Post()方法传递目录时，管道就会启动，并最终将单词从c#源代码写入控制台。
        /// 这里可以发出多个启动管道的请求，传递多个目录，并行执行这些任务
        /// </summary>
        public void ContectBlockDemoStart()
        {
            var target = SetupPipeline();
            target.Post(@"C:\Users\Dream\Desktop\UICalculator\CalculatorContract");
            Console.ReadLine();
        }

        /// <summary>
        /// 方法接收一个目录路径作为参数，得到以.cs为扩展名的文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<string> GetFileNames(string path)
        {
            foreach (var fileName in Directory.EnumerateFiles(path, "*.cs"))
            {
                yield return fileName;
            }
        }

        /// <summary>
        /// 方法以一个文件名列表作为参数，得到文件中的每一行
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        private IEnumerable<string> LoadLines(IEnumerable<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                using (FileStream stream = File.OpenRead(fileName))
                {
                    var reader = new StreamReader(stream);
                    string line = null;
                    while (((line = reader.ReadLine()) != null))
                    {
                        yield return line;
                    }
                }
            }

        }

        /// <summary>
        /// 方法接收一个lines集合作为参数，将其逐行分割，从而得到并返回一个单词列表
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private IEnumerable<string> GetWords(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                string[] words = line.Split(' ', ';', '(', ')', '{', '}', '.', ',');
                foreach (var word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        yield return word;
                    }
                }
            }
        }

        /// <summary>
        /// 为了创建管道，SetupPipeline()方法创建了3个TransformBlock对象。
        /// TransformBlock是一个源和目标块，通过使用委托来转换源。
        /// 第一个TransfomBlock被声明为将一个字符串转换为IEnumerable<sfring>。
        /// 这种转换是通过GetFileNames()方法完成的，GetFileNames()方法在传递给第一个块的构造函数的lambda表达式中调用。
        /// 类似地，接下来的两个TransformBlock对象用于调用LoadLines()和GetWords()方法
        /// </summary>
        /// <returns></returns>
        private ITargetBlock<string> SetupPipeline()
        {
            var fileNamesForPath = new TransformBlock<string, IEnumerable<string>>(
                path =>
                {
                    return GetFileNames(path);
                });

            var lines = new TransformBlock<IEnumerable<string>, IEnumerable<string>>(
               fileNames =>
               {
                   return LoadLines(fileNames);
               });

            var words = new TransformBlock<IEnumerable<string>, IEnumerable<string>>(
               lines2 =>
               {
                   return GetWords(lines2);
               });

            //定义的最后一个块是ActionBlock。这个块只是一个用于接收数据的目标块
            var display = new ActionBlock<IEnumerable<string>>(
                coll =>
                {
                    foreach (var s in coll)
                    {
                        Console.WriteLine(s);
                    }
                });
            //最后，将这些块彼此连接起来。最后返回用于启用管道的块。
            fileNamesForPath.LinkTo(lines);
            lines.LinkTo(words);
            words.LinkTo(display);
            return fileNamesForPath;
        }
    }

}

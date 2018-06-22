using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndStreamsConsole.FileMonitorSample
{
    public static class FileMonitorDemo
    {
        public static void WatchFiles(string path, string filter)
        {
            var watcher = new FileSystemWatcher(path, filter)
            {
                IncludeSubdirectories = true
            };

            watcher.Created += OnFileChanged;
            watcher.Changed += OnFileChanged;
            watcher.Deleted += OnFileChanged;
            watcher.Renamed += OnFileRenamed;

            watcher.EnableRaisingEvents = true;
            Console.WriteLine("观察文件变化中。。。。。。");
        }

        private static void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"文件{e.OldName}{e.ChangeType}命名为{e.Name}");
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"文件{e.Name}{e.ChangeType}");
        }
    }
}

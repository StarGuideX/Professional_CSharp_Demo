using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsole
{
    public static class DataProtectionDemo
    {
        private const string readOption = "-r";
        private const string writeOption = "-w";
        private static readonly string[] options = { readOption, writeOption };

        public static void DataProtectionStart(string[] arr)
        {
            if (arr.Length != 2 || arr.Intersect(options).Count() != 1)
            {
                ShowUsage();
                return;
            }

            string fileName = arr[1];

            MySafe safe = InitProtection();

            switch (arr[0]) {

                case writeOption:
                    Write(safe, fileName);
                    break;
                case readOption:
                    Read(safe, fileName);
                    break;
                default:
                    ShowUsage();
                    break;
            }
        }
    }
}

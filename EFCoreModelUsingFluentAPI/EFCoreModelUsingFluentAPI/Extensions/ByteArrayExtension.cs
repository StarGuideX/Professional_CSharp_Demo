using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingFluentAPI.Extensions
{
    public static class ByteArrayExtension
    {
        public static string StringOutput(this byte[] data)
        {
            var sb = new StringBuilder();
            foreach (byte b in data)
            {
                sb.Append($"{b}.");
            }
            return sb.ToString();
        }
    }
}

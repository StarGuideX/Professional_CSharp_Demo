using System;
using System.Reflection;
using System.Text;

namespace TypeView
{
    class Program
    {
        /// <summary>
        /// 输出的文本
        /// </summary>
        private static StringBuilder OutputText = new StringBuilder();
        static void Main(string[] args)
        {
            Type t = typeof(double);

            AnalyzeType(t);
            Console.WriteLine($"分析{t.Name}类型");
            Console.WriteLine(OutputText.ToString());

            Console.ReadLine();
        }

        private static void AnalyzeType(Type t)
        {
            TypeInfo typeInfo = t.GetTypeInfo();
            AddToOutput($"Type Name：{t.Name}");
            AddToOutput($"Full Name：{t.FullName}");
            AddToOutput($"NameSpace：{t.Namespace}");

            Type tBase = t.BaseType;
            if (tBase != null)
            {
                AddToOutput($"Base Type：{tBase.Name}");
            }
            AddToOutput("\n公有成员：");

            foreach (MemberInfo member in t.GetMembers())
            {
#if NET46
                AddToOutput($"{member.DeclaringType} {member.MemberType} {member.Name}");
#else
                AddToOutput($"{member.DeclaringType} {member.Name}");
#endif
            }
        }

        private static void AddToOutput(string v)
        {
            OutputText.Append("\n" + v.ToString());
        }
    }
}

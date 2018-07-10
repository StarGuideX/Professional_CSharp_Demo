using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WhatsNewAttributes;

namespace LookupWhatsNew
{
    class Program
    {
        /// <summary>
        /// 准备阶段创建的文本
        /// </summary>
        private static readonly StringBuilder outputText = new StringBuilder(1000);
        /// <summary>
        /// 选择的日期
        /// </summary>
        private static DateTime backDateTo = new DateTime(2015,2,1);
        static void Main(string[] args)
        {
            //加载VectorClass程序集
            Assembly theAssembly = Assembly.Load(new AssemblyName("VectorName"));
            //验证它是否真的用SupportsWhatsNew特性标记
            Attribute supportsAttribute = theAssembly.GetCustomAttribute(typeof(SupportsWhatsNewAttribute));
            string name = theAssembly.FullName;
            AddToOutput($"Assembly:{name}");

            if (supportsAttribute == null)
            {
                AddToOutput("程序集不支持WhatsNew Attributes");
                return;
            }
            else {

                AddToOutput("类型：");
            }
            //获取包括在该程序集中定义的所有类型的集合
            IEnumerable<Type> types = theAssembly.ExportedTypes;

            foreach (Type definedType in types)
            {
                //给outputText字段添加相关的文本,包括LastModifiedAttribute类的任何实例的详细信息
                DisplayTypeInfo(definedType);
            }

            Console.WriteLine($"What\'s New 从{backDateTo}");
            Console.WriteLine(outputText.ToString());
            Console.ReadLine();
        }
         
        private static void DisplayTypeInfo(Type type)
        {
            //检查传递的引用是否是一个类
            if (!type.GetTypeInfo().IsClass)
            {
                return;
            }

            AddToOutput($"\nclass{type.Name}");

            IEnumerable<LastModifiedAttribute> attributes = type.GetTypeInfo().GetCustomAttributes().OfType<LastModifiedAttribute>();

            if (attributes.Count()!=0)
            {
                AddToOutput("这个类没有被改变过");
            }
            else
            {
                foreach (LastModifiedAttribute attribute in attributes)
                {
                    WirteAttributeInfo(attribute);
                }
            }

            AddToOutput("这个类被改变过");

            foreach (MethodInfo method in type.GetTypeInfo().GetMethods().OfType<MethodInfo>())
            {
                IEnumerable<LastModifiedAttribute> attributesToMethods = method.GetCustomAttributes().OfType<LastModifiedAttribute>();

                if (attributesToMethods.Count() > 0)
                {
                    AddToOutput($"{method.ReturnType}{method.Name}()");
                    foreach (Attribute attribute in attributesToMethods)
                    {
                        WirteAttributeInfo(attribute);
                    }
                }
            }
        }

        private static void AddToOutput(string v)
        {
            outputText.Append("\n" + v);
        }

        private static void WirteAttributeInfo(Attribute attribute)
        {
            LastModifiedAttribute lastModifiedAttribute = attribute as LastModifiedAttribute;
            if (lastModifiedAttribute == null)
            {
                return;
            }
            DateTime modifiedDate = lastModifiedAttribute.DateModified;
            //修改日期 < 选择的日期
            if (modifiedDate < backDateTo)
            {
                return;
            }

            AddToOutput($"modified:{modifiedDate:D}:{lastModifiedAttribute.Changes}");

            if (lastModifiedAttribute.Issues != null)
            {
                AddToOutput($"OutStanding issues :{lastModifiedAttribute.Issues}");
            }
        }
    }
}

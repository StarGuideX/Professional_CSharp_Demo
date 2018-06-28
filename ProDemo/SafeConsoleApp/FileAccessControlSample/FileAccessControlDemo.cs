using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace SafeConsoleApp
{
    public static class FileAccessControlDemo
    {
        /// <summary>
        /// FileStream类定义了GetAccessControl()方法，该方法返回一个FileSecunty对象。
        /// FileSecunty是一个.NET类，它表示文件的安全描述符。
        /// FileSecurity类派生自基类ObjectSecurity、CommonObjectSecurity、NativeObjectSecurity和FileSystemSecunty。
        /// 其他表示安全描述符的类有CryptoKeySecunty、EventWaitHandleSecurity、MutexSecurrty、
        /// RegistrySecurity、SemaphoreSecurity、PipeSecurity和ActiveDirectorySecunty。
        /// 所有这些对象都可以使用访问控制列表来保护。
        /// 一般情况下，对应的.NET类定义了GetAccessControl()方法，返回相应的安全类；
        /// 例如，Mutex.GetAccessControl()方法返回一个MutexSecunty类，
        /// PipeStream.GetAccessControI()方法返回一个PipeSecunty类。
        /// 
        /// FileSecunty类定义了读取、修改DACL和SACL的方法。
        /// GetAccessRules()方法以AuthorizationRuleCollection类的形式返回DACL。
        /// 要访问SACL，可以使用GetAuditRules方法。
        /// 
        /// 在GetAccessRules()方法中，可以确定是否应使用继承的访问规则(不仅仅是用对象直接定义的访问规则)。
        /// 最后一个参数定义了应返回的安全标识符的类型。这个类型必须派生自基类IdentityReference。
        /// 可能的类型有NTAccount和Securityldentifier。这两个类都表示用户或组。
        /// NTAccount类按名称查找安全对象，Secuntyldentifier类按唯一的安全标识符查找安全对象。
        /// 
        /// 返回的AuthorizationRuIeCollection包含AuthoriztionRuIe对象。AuthorizationRuIe对象是ACE的.NET表示。
        /// 在这里的例子中，因为访问一个文件，所以AuthorizationRule对象可以强制转换为FileSystemAccessRule类型。
        /// 在其他资源的ACE中，存在不同的.NET表示，例如MutexAccessRule和PipeAccessRule。
        /// 在FileSystemAccessRuIe类中，AccessConfrolType、FileSystemRights和IdentityReference属性返回ACE的相关信息
        /// </summary>
        /// <param name="fileName"></param>
        public static void FileAccessControlStart(string fileName)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Open))
            {
                FileSecurity securityDescriptor = stream.GetAccessControl();
                AuthorizationRuleCollection rules = securityDescriptor.GetAccessRules(true, true, typeof(NTAccount));

                foreach (AuthorizationRule rule in rules)
                {
                    var fileRule = rule as FileSystemAccessRule;
                    Console.WriteLine($"Access类型：{fileRule.AccessControlType}");
                    Console.WriteLine($"Rights：{fileRule.FileSystemRights}");
                    Console.WriteLine($"Identity：{fileRule.IdentityReference.Value}");
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// 设置访问权限
        /// </summary>
        /// <param name="fileName"></param>
        private static void WriteAcl(string fileName)
        {
            var salesIdentity = new NTAccount("Sales");
            var developerIdentity = new NTAccount("Developers");
            var everyOneIdentity = new NTAccount("EveryOne");

            var salesAce = new FileSystemAccessRule(salesIdentity,FileSystemRights.Write,AccessControlType.Deny);
            var everyOneAce = new FileSystemAccessRule(everyOneIdentity, FileSystemRights.Read, AccessControlType.Allow);
            var developerAce = new FileSystemAccessRule(developerIdentity, FileSystemRights.FullControl, AccessControlType.Allow);

            var securityDescriptor = new FileSecurity();
            securityDescriptor.SetAccessRule(everyOneAce);
            securityDescriptor.SetAccessRule(developerAce);
            securityDescriptor.SetAccessRule(salesAce);

            //File.SetAccessControl(fileName, securityDescriptor);
        }
    }
}

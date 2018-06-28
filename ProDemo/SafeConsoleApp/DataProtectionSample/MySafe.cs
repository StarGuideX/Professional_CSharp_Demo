using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsoleApp
{
    /// <summary>
    /// 类MySafe有一个IDataProtector成员。Mysafe类通过依赖注入接收IDataProtectionProvider接口。
    /// 有了这个接口，传递目的字符串，返回IDataProtectoro读写这个安全时，需要使用相同的字符串
    /// </summary>
    public class MySafe
    {
        /// <summary>
        /// 这个接口定义了Protect和Unprotect,来加密和解密数据。这个接口定义了Protect和Unprotect方法，这些方法带有字节数组参数，返回字节数组。
        /// </summary>
        private IDataProtector _protector;
        public MySafe(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("MySafe.MyProtection.v1");

        }

        /// <summary>
        /// 示例代码使用NuGet包Microsoft.AspNet.DataProtection.Abstractions中定义的扩展方法，直接发送、返回来自Encrypt方法的字符串。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt(string input) => _protector.Protect(input);

        /// <summary>
        /// 示例代码使用NuGet包Microsoft.AspNet.DataProtection.Abstractions中定义的扩展方法，直接发送、返回来自Decrypt方法的字符串。
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public string Decrypt(string encrypted) => _protector.Unprotect(encrypted);
    }
}

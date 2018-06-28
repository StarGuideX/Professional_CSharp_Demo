using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsoleApp
{
    public static class WindowsPrincipalDemo
    {
        /// <summary>
        /// 调用方法ShowIdentityInformation把WindowsIdentity的信息写到控制台，
        /// 调用ShowPrincipal写入可用于principals的额外信息，
        /// 调用ShowClaims写入声明信息
        /// </summary>
        public static void WindowsPrincipalStart()
        {

            WindowsIdentity identity = ShowIdentityInformation();
            WindowsPrincipal principal = ShowPrincipal(identity);
            ShowClaims(principal.Claims);
        }

        /// <summary>
        /// ShowIdentityInformation方法通过调用WindowsIdentity的静态方法GetCurrent，
        /// 创建一个WindowsIdentity对象，并访问其属性，来显示身份类型、登录名、
        /// 是否进行了身份验证、身份验证类型、匿名用户和AccessToken等
        /// </summary>
        /// <returns></returns>
        private static WindowsIdentity ShowIdentityInformation()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (identity == null)
            {
                Console.WriteLine("没有windows 用户");
                return null;
            }

            Console.WriteLine($"身份类型：{identity}");
            Console.WriteLine($"登录名：{identity.Name}");
            Console.WriteLine($"是否进行了身份验证：{identity.IsAuthenticated}");
            Console.WriteLine($"身份验证类型：{identity.AuthenticationType}");
            Console.WriteLine($"是否为匿名用户{identity.IsAnonymous}");
            Console.WriteLine($"AccessToken：{identity.AccessToken.DangerousGetHandle()}");
            Console.WriteLine();
            return identity;
        }

        /// <summary>
        /// 验证用户是否属于内置的角色User和Administrator
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        private static WindowsPrincipal ShowPrincipal(WindowsIdentity identity)
        {
            Console.WriteLine("显示组成员信息。");
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (principal == null)
            {
                Console.WriteLine("无组成员");
                return null;
            }
            Console.WriteLine($"Users?{principal.IsInRole(WindowsBuiltInRole.User)} ");
            Console.WriteLine($"Administrator?{principal.IsInRole(WindowsBuiltInRole.Administrator)}");
            Console.WriteLine();
            return principal;
        }

        /// <summary>
        /// 访问一组声明，把主题、发行人、声明类型和更多选项写到控制台
        /// </summary>
        /// <param name="claims"></param>
        private static void ShowClaims(IEnumerable<Claim> claims)
        {
            Console.WriteLine("声明");
            foreach (var claim in claims)
            {
                Console.WriteLine($"主题：{claim.Subject}");
                Console.WriteLine($"颁发者：{claim.Issuer}");
                Console.WriteLine($"声明类型：{claim.Type}");
                Console.WriteLine($"值类型：{claim.ValueType}");
                Console.WriteLine($"值：{claim.Value}");
                foreach (var prop in claim.Properties)
                {
                    Console.WriteLine($"\tProperty：{prop.Key}{prop.Value}");
                }
                Console.WriteLine();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsole
{
    public class MySafe
    {
        private IDataProtector _protector;
        public MySafe(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("MySafe.MyProtection.v1");

        }

        public string Encrypt(string input) => _protector.Protect(input);

        public string Decrypt(string encrypted) => _protector.Unprotect(encrypted);
    }
}

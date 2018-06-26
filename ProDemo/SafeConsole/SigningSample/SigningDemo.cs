using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsole
{
    public static class SigningDemo
    {
        private static CngKey _aliceKeySignature;
        private static byte[] _alicePublicKeyBlob;
        /// <summary>
        ///  创建Alice的密钥，给字符串"Alice"签名，最后使用公钥验证该签名是否真的来自于Alice。
        /// </summary>
        public static void SigningStart()
        {
            InitAliceKeys();
            byte[] aliceData = Encoding.UTF8.GetBytes("Alice");
            byte[] aliceSignature = CreateSignature(aliceData, _aliceKeySignature);
            Console.WriteLine($"Alice创建签名：{Convert.ToBase64String(aliceSignature)}");
            if (VerifySignature(aliceData, aliceSignature, _alicePublicKeyBlob))
            {
                Console.WriteLine("Alice签名已成功验证");
            }
        }

        /// <summary>
        /// 为Alice创建新的密钥对。因为这个密钥对存储在一个静态字段中，所以可以从其他方法中访问它。
        /// 除了使用CngKey类创建密钥对之外，还可以打开存储在密钥存储器中的己有密钥。
        /// 通常Alice在其私有存储器中有一个证书，其中包含了一个密钥对，该存储器可以用CngKey.Open()方法访问。
        /// </summary>
        private static void InitAliceKeys()
        {
            // CngKey类的Creat()方法把该算法作为一个参数，为算法定义密钥对。
            _aliceKeySignature = CngKey.Create(CngAlgorithm.ECDsaP521);
            // 通过Export()方法，导出密钥对中的公钥。这个公钥可以提供给Bob,来验证签名。Alice保留其私钥。
            _alicePublicKeyBlob = _aliceKeySignature.Export(CngKeyBlobFormat.GenericPublicBlob);
        }

        /// <summary>        
        /// 有了密钥对，Alice就可以使用ECDsaCng类创建签名了。
        /// 这个类的构造函数从Alice那里接收包含公钥和私钥的CngKey类。
        /// 再使用私钥，通过SignData0方法给数据签名。SignData()方法在.NET core中略有不同。.NETcore需要如下算法
        /// </summary>
        /// <param name="aliceData"></param>
        /// <param name="_aliceKeySignature"></param>
        /// <returns></returns>
        private static byte[] CreateSignature(byte[] aliceData, CngKey _aliceKeySignature)
        {
            byte[] signature;
            using (var signingAlg = new ECDsaCng(_aliceKeySignature))
            {
#if NET46
                signature = signingAlg.SignData(aliceData);
                signingAlg.Clear();
#else
                signature = signingAlg.SignData(aliceData, HashAlgorithmName.SHA512);
#endif
            }
            return signature;
        }

        /// <summary>
        /// 要验证签名是否真的来自于Alice，Bob使用Alice的公钥检查签名。包含公钥blob的字节数组可以用静态方法Import()导入CngKey对象。
        /// 然后使用ECDsaCng类，调用VerifyData()方法来验证签名。
        /// </summary>
        /// <param name="aliceData"></param>
        /// <param name="aliceSignature"></param>
        /// <param name="_alicePublicKeyBob"></param>
        /// <returns></returns>
        private static bool VerifySignature(byte[] data, byte[] signature, byte[] pubKey)
        {
            bool retValue = false;
            using (CngKey key = CngKey.Import(pubKey, CngKeyBlobFormat.GenericPublicBlob))
            using (var signingAlg = new ECDsaCng(key))
            {
#if NET46
                retValue = signingAlg.VerifyData(data, signature);
                signingAlg.Clear();
#else
                retValue = signingAlg.VerifyData(data, signature, HashAlgorithmName.SHA512);
#endif
            }
            return retValue;
        }
    }
}

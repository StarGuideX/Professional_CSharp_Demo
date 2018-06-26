using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsole
{
    public static class RSADemo
    {
        private static CngKey _aliceKey;
        private static byte[] _alicePubKeyBlob;

        public static void RSAStart()
        {
            byte[] document;
            byte[] hash;
            byte[] signature;
            AlickTask(out document, out hash, out signature);
            BobTasks(document, hash, signature);

        }
        /// <summary>
        /// 首先创建Alice所需的密钥，将消息转换为一个字节数组，散列字节数组，并添加一个签名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hash"></param>
        /// <param name="signature"></param>
        private static void AlickTask(out byte[] data, out byte[] hash, out byte[] signature)
        {
            InitAliceKeys();
            data = Encoding.UTF8.GetBytes("致Alice");
            hash = HashDocument(data);
            signature = AddSignatureToHash(hash,_aliceKey);
        }

        /// <summary>
        /// Alice所需的密钥是使用CngKey类创建的。现在正在使用RSA算法，
        /// 把CngAlgorithm.Rsa枚举值传递到Create方法，来创建公钥和私钥。
        /// 公钥只提供给Bob，所以公钥用Export方法提取
        /// </summary>
        private static void InitAliceKeys()
        {
            _aliceKey = CngKey.Create(CngAlgorithm.Rsa);
            _alicePubKeyBlob = _aliceKey.Export(CngKeyBlobFormat.GenericPublicBlob);
        }

        /// <summary>
        /// 为文档创建一个散列码。散列码使用一个散列算法SHA384类创建。
        /// 不管文档存在多久，散列码的长度总是相同。
        /// 再次为相同的文档创建散列码，会得到相同的散列码。
        /// Bob需要在文档上使用相同的算法。
        /// 如果返回相同的散列码，就说明文档没有改变。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] HashDocument(byte[] data)
        {
            using (var hashAlg = SHA384.Create())
            {
                return hashAlg.ComputeHash(data);
            }
        }

        /// <summary>
        /// 添加签名，可以保证文档来自Alice。在这里，使用RSACng类给散列签名。
        /// Alice的CngKey(包括公钥和私钥）传递给RSACng类的构造函数；
        /// 签名通过调用SignHash方法创建。给散列签名时，SignHash方法需要了解散列算法；
        /// HashAlgorithmName.SHA384是创建散列所使用的算法。
        /// 此外，需要RSA填充。RSASignaturePadding枚举的可选项是PSS和Pkcsl：
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="aliceKey"></param>
        /// <returns></returns>
        private static byte[] AddSignatureToHash(byte[] hash, CngKey key)
        {
            using (var signAlg = new RSACng(key))
            {
                byte[] signed = signAlg.SignHash(hash, HashAlgorithmName.SHA384, RSASignaturePadding.Pss);
                return signed;
            }
        }

        /// <summary>
        /// Alice散列并签名后，Bob的任务可以在BobTasks方法中开始。Bob接收文档数据、散列码和签名，他使用Alice的公钥。首先，Alice的公钥使用CngKey.Import导入，分配给aliceKey变量。接下来，Bob使用辅助方法IsSignatureValid和IsDocumentUnchanged，来验证签名是否有效，文档是否不变。只有在两个条件是true时，文档写入控制台：
        /// </summary>
        /// <param name="document"></param>
        /// <param name="hash"></param>
        /// <param name="signature"></param>
        private static void BobTasks(byte[] data, byte[] hash, byte[] signature)
        {
            CngKey aliceKey = CngKey.Import(_alicePubKeyBlob, CngKeyBlobFormat.GenericPublicBlob);
            if (!IsSignatureValid(hash,signature,aliceKey))
            {
                Console.WriteLine("签名不合规");
                return;
            }
            if (!IsDocumentUnChanged(hash, data))
            {
                Console.WriteLine("文档已经变化");
                return;
            }
            Console.WriteLine("签名合规，文档无变化");
            Console.WriteLine($"从Alice获得的文档：{Encoding.UTF8.GetString(data,0,data.Length)}");

        }

        /// <summary>
        /// 为了验证签名是否有效，使用Alice的公钥创建RSACng类的一个实例。
        /// 通过这个类，使用VerifyHash方法传递散列、签名、早些时候使用的算法信息。
        /// 现在Bob知道，信息来自Alice：
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="signature"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool IsSignatureValid(byte[] hash, byte[] signature, CngKey key)
        {
            using (var sianAlg = new RSACng(key))
            {
                return sianAlg.VerifyHash(hash, signature, HashAlgorithmName.SHA384, RSASignaturePadding.Pss);
            }
        }

        /// <summary>
        /// 为了验证文档数据没有改变，Bob再次散列文件，并使用LINQ扩展方法SequenceEqual，
        /// 验证散列码是否与早些时候发送的相同。如果散列值是相同的，就可以假定文档没有改变
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsDocumentUnChanged(byte[] hash, byte[] data)
        {
            byte[] newHash = HashDocument(data);
            return newHash.SequenceEqual(hash);
        }
    }
}

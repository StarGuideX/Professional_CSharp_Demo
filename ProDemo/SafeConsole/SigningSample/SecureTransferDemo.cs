using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SafeConsole
{
    public static class SecureTransferDemo
    {
        private static CngKey _aliceKey;
        private static CngKey _bobKey;
        private static byte[] _alicePubKeyBlob;
        private static byte[] _bobPubKeyBlob;

        public static void SecureTransferStart()
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            try
            {
                CreateKeys();
                byte[] encryptedData = await AliceSendsDataAsync("一条发给bob加密消息");
                await BobReceivesDataAsync(encryptedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 使用EC DiffieHellman 521算法创建密钥
        /// </summary>
        private static void CreateKeys()
        {
            _aliceKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP521);
            _bobKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP521);
            _alicePubKeyBlob = _aliceKey.Export(CngKeyBlobFormat.EccPublicBlob);
            _bobPubKeyBlob = _aliceKey.Export(CngKeyBlobFormat.EccPublicBlob);
        }

        /// <summary>
        /// 在AliceSendsDataAsync()方法中，包含文本字符的字符串使用Encoding类转换为一个字节数组。
        /// 创建一个ECDiffieHellmanCng对象，用Alice的密钥对初始化它。
        /// Alice调用DeriveKeyMaterial()方法，从而使用其密钥对和Bob的公钥创建一个对称密钥。
        /// 返回的对称密钥使用对称算法AES加密数据。
        /// AesCryptoServiceProvider需要密钥和一个初始化矢量（IV）。
        /// IV从GenerateIV()方法中动态生成，对称密钥用EC Diffe-He11man算法交换，但还必须交换IV。
        /// 从安全性角度来看，在网络上传输未加密的IV是可行的一一只是密钥交换必须是安全的。
        /// IV存储为内存流中的第一项内容，其后是加密的数据，其中，CryptoStream类使用AesCryptoServiceProvider类创建的encryptor。
        /// 在访问内存流中的加密数据之前，必须关闭加密流。否则，加密数据就会丢失最后的位。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static async Task<byte[]> AliceSendsDataAsync(string message)
        {
            Console.WriteLine($"Alice发送了信息{message}");
            byte[] rawData = Encoding.UTF8.GetBytes(message);
            byte[] encryptedData = null;
            using (var aliceAlgorithm = new ECDiffieHellmanCng(_aliceKey))
            using (CngKey bobPubkey = CngKey.Import(_bobPubKeyBlob, CngKeyBlobFormat.EccPublicBlob))
            {
                byte[] symmKey = aliceAlgorithm.DeriveKeyMaterial(bobPubkey);
                Console.WriteLine($"Alice创建对称密钥——bob的公共密钥信息{Convert.ToBase64String(symmKey)}");

                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Key = symmKey;
                    aes.GenerateIV();
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            await ms.WriteAsync(aes.IV, 0, aes.IV.Length);
                            cs.Write(rawData, 0, rawData.Length);
                        }
                        encryptedData = ms.ToArray();
                    }
                    aes.Clear();
                }
            }
            Console.WriteLine($"Alice：加密消息{ Convert.ToBase64String(encryptedData)}");
            Console.WriteLine();
            return encryptedData;
        }

        /// <summary>
        /// Bob从BobReceivesDataAsync()方法的参数中接收加密数据。
        /// 首先，必须读取未加密的初始化矢量。AesCryptoServiceProvider类的BlockSize属性返回块的位数。
        /// 位数除以8，就可以计算出字节数。最快的方式是把数据右移3位。
        /// 右移1位就是除以2，右移2位就是除以4，右移3位就是除以8。
        /// 在for循环中，包含未加密IV的原字节的前几个字节写入数组iv中。
        /// 接着用Bob的密钥对实例化一个ECDiffieHellmanCng对象。
        /// 使用Alice的公钥，从DeriveKeyMaterial0方法中返回对称密钥。
        /// 比较Alice和Bob创建的对称密钥，可以看出所创建的密钥值相同。
        /// 使用这个对称密钥和初始化矢量，来自Alice的消息就可以用AesCryptoServiceProvider类解密。
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        private static async Task BobReceivesDataAsync(byte[] encryptedData)
        {
            Console.WriteLine($"Bob已接收加密数据");
            byte[] rawData = null;

            var aes = new AesCryptoServiceProvider();

            int nBytes = aes.BlockSize >> 3;
            byte[] iv = new byte[nBytes];
            for (int i = 0; i < iv.Length; i++)
            {
                iv[i] = encryptedData[i];
            }

            using (var bobAlgorithm = new ECDiffieHellmanCng(_bobKey))
            using (CngKey alicePubKey = CngKey.Import(_alicePubKeyBlob, CngKeyBlobFormat.EccPublicBlob))
            {
                byte[] symmKey = bobAlgorithm.DeriveKeyMaterial(alicePubKey);
                Console.WriteLine($"Bob创建对称密钥——Alice的公共密钥信息{Convert.ToBase64String(symmKey)}");

                aes.Key = symmKey;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        await cs.WriteAsync(encryptedData, nBytes, encryptedData.Length - nBytes);
                    }

                    rawData = ms.ToArray();

                    Console.WriteLine($"Bob解密信息：{Encoding.UTF8.GetString(rawData)}");
                }
                aes.Clear();
            }
        }
    }
}

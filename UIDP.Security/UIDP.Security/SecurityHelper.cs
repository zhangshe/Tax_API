using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace UIDP.Security
{
    /// <summary>
    /// 安全助手
    /// </summary>
    public static class SecurityHelper
    {

        #region prop

        /// <summary>
        /// 默认编码
        /// </summary>
        public static Encoding DefaultEncoding { get; } = Encoding.UTF8;

        #endregion prop

        public static string StringToMD5Hash(string inputString)

        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(inputString));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < encryptedBytes.Length; i++)

            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();

        }
        #region 对称加密算法

        #region Des 加解密

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="input"> 待加密的字符串 </param>
        /// <param name="key"> 密钥（8位） </param>
        /// <param name="encoding">编码，为 null 取默认值</param>
        /// <returns></returns>
        public static string DesEncrypt(string input, string key, Encoding encoding = null)
        {
            encoding = encoding ?? DefaultEncoding;

            try
            {
                var keyBytes = encoding.GetBytes(key);
                //var ivBytes = Encoding.UTF8.GetBytes(iv);

                var des = DES.Create();
                des.Mode = CipherMode.ECB; //兼容其他语言的 Des 加密算法
                des.Padding = PaddingMode.Zeros; //自动补 0

                using (var ms = new MemoryStream())
                {
                    var data = encoding.GetBytes(input);
                    byte[] ivBytes = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

                    using (var cs =
                        new CryptoStream(ms, des.CreateEncryptor(keyBytes, ivBytes), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch
            {
                return input;
            }
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="input"> 待解密的字符串 </param>
        /// <param name="key"> 密钥（8位） </param>
        /// <param name="encoding">编码，为 null 时取默认值</param>
        /// <returns></returns>
        public static string DesDecrypt(string input, string key, Encoding encoding = null)
        {
            encoding = encoding ?? DefaultEncoding;

            try
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] ivBytes = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

                var des = DES.Create();
                des.Mode = CipherMode.ECB; //兼容其他语言的Des加密算法
                des.Padding = PaddingMode.Zeros; //自动补0

                using (var ms = new MemoryStream())
                {
                    var data = Convert.FromBase64String(input);

                    using (var cs =
                        new CryptoStream(ms, des.CreateDecryptor(keyBytes, ivBytes), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }

                    return encoding.GetString(ms.ToArray());
                }
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        #endregion Des 加解密

        #endregion 对称加密算法

        #region 非对称加密算法

        /// <summary>
        /// 生成 RSA 公钥和私钥
        /// </summary>
        /// <param name="publicKey"> 公钥 </param>
        /// <param name="privateKey"> 私钥 </param>
        public static void GenerateRsaKeys(out string publicKey, out string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
        }

        /// <summary>
        /// RSA 加密
        /// </summary>
        /// <param name="publickey"> 公钥 </param>
        /// <param name="content"> 待加密的内容 </param>
        /// <param name="encoding">编码，为 null 时取默认编码</param>
        /// <returns> 经过加密的字符串 </returns>
        public static string RsaEncrypt(string publickey, string content, Encoding encoding = null)
        {
            encoding = encoding ?? DefaultEncoding;

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);

            var cipherbytes = rsa.Encrypt(encoding.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA 解密
        /// </summary>
        /// <param name="privatekey"> 私钥 </param>
        /// <param name="content"> 待解密的内容 </param>
        /// <param name="encoding"></param>
        /// <returns> 解密后的字符串 </returns>
        public static string RsaDecrypt(string privatekey, string content, Encoding encoding = null)
        {
            encoding = encoding ?? DefaultEncoding;

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privatekey);
            var cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return encoding.GetString(cipherbytes);
        }

        #endregion 非对称加密算法

        
        #region 文件加密算法
        // 对称算法的机密密钥
        static readonly byte[] KEY = {0x07, 0x09, 0x03, 0x015, 0x05, 0x06, 0x07, 0x08,
                                      0x09, 0x10, 0x04, 0x12, 0x13, 0x11, 0x15, 0x16};

        // 对称算法的初始化向量
        static readonly byte[] IV = {0x01, 0x02, 0x07, 0x04, 0x05, 0x06, 0x07, 0x08,
                                     0x09, 0x3, 0x11, 0x12, 0x07, 0x14, 0x15, 0x16};
        /// <summary>
		/// 文件加密。
		/// </summary>
		/// <param name="p_strFileName">要加密的文件名</param>
		/// <returns>加密成功返回1，不成功返回-1</returns>
		/// 
		public static int FileEncrypt(string p_strFileName)
        {
            int intHandle = 0;
            try
            {
                // 对文件加密
                intHandle = FileEncryptOrDecrypt(p_strFileName, true);
            }
            catch (Exception e)
            {
                throw e;
            }
            return intHandle;
        }

        /// <summary>
        /// 文件解密。
        /// </summary>
        /// <param name="p_strFileName">要解密的文件名</param>
        /// <returns>解密成功返回1，不成功返回-1</returns>
        /// 
        public static int FileDecrypt(string p_strFileName)
        {
            int intHandle = 0;
            try
            {
                // 对文件解密
                intHandle = FileEncryptOrDecrypt(p_strFileName, false);
            }
            catch (Exception e)
            {
                throw e;
            }
            return intHandle;
        }
        /// <summary>
		/// 文件加解密
		/// </summary>
		/// <param name="p_strFileName">要加解密的文件名</param>
		/// <param name="p_bIsEncrypt">为真，执行加密操作；为假，执行解密操作</param>
		/// <returns>加解密文件成功返回1，不成功返回-1</returns>
		/// 
		static int FileEncryptOrDecrypt(string p_strFileName, bool p_bIsEncrypt)
        {
            // 如果要加密文件，且文件不存在
            if (p_bIsEncrypt && (!File.Exists(p_strFileName)))
            {
                return -1;      // 返回-1
            }

            try
            {
                // 输入文件流
                FileStream fileStreamIn = new FileStream(p_strFileName, FileMode.Open,
                                                                        FileAccess.Read);

                // 输出文件流
                MemoryStream memoryStream = new MemoryStream();

                // 对称算法
                SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create();

                // 如果是加密操作，生成加密转换；如果是解密操作，生成解密转换
                ICryptoTransform cryptoTransform = p_bIsEncrypt
                                                 ? symmetricAlgorithm.CreateEncryptor(KEY, IV)
                                                 : symmetricAlgorithm.CreateDecryptor(KEY, IV);

                // 加密操作时，生成加密转换流；解密操作时，生成解密转换流				
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             cryptoTransform,
                                                             CryptoStreamMode.Write);

                // 从文件中读入数据，加解密后保存在内存
                FileToMemory(fileStreamIn, cryptoStream);

                // 将内存中数据写入磁盘文件
                MemoryToFile(memoryStream.ToArray(), p_strFileName);

                return 1;                               // 操作成功返回1
            }
            catch (Exception ex)                    // 捕获到异常
            {
                throw ex;

            }
        }
        /// <summary>
		/// 从磁盘文件中读取数据写入内存
		/// </summary>
		/// <param name="p_fileStreamIn">输入文件流</param>
		/// <param name="p_cryptoStream">将数据链接到加密转换的流</param>
		/// 
		static void FileToMemory(FileStream p_fileStreamIn, CryptoStream p_cryptoStream)
        {
            try
            {
                byte[] arrByteBuffer = new byte[100];           // 缓存器
                long lngReadLength = 0;                         // 总的已读入字节数
                long lngTotalLength = p_fileStreamIn.Length;    // 输入文件的总字节数
                int intLength;                                  // 每次读入时，实际读入的字节数
                while (lngReadLength < lngTotalLength)
                {
                    // 从输入文件流中读数据到缓冲器
                    intLength = p_fileStreamIn.Read(arrByteBuffer, 0, arrByteBuffer.Length);

                    // 加密转换流将缓冲器的数据写入到，与加密转换流关联的内存流
                    p_cryptoStream.Write(arrByteBuffer, 0, intLength);
                    lngReadLength += intLength;     // 更新总的已读入字节数
                }
                p_cryptoStream.Close();             // 关闭加密转换流 
                p_fileStreamIn.Close();             // 关闭输入文件流
            }
            catch (Exception )
            {
                //throw e;
            }
        }
        /// <summary>
		/// 将内存中的数据写入磁盘文件
		/// </summary>
		/// <param name="p_arrByteData">内存中的数据</param>
		/// <param name="p_strFileName">要存入的文件名</param>
		/// 
		static void MemoryToFile(byte[] p_arrByteData, string p_strFileName)
        {
            try
            {
                // 输出文件流
                FileStream fileStreamOut = new FileStream(p_strFileName, FileMode.Create,
                    FileAccess.Write);

                // 与输出文件流关联的二进制写入器
                BinaryWriter binaryWriter = new BinaryWriter(fileStreamOut);
                binaryWriter.Write(p_arrByteData);      // 二进制写入器向输出文件流中写入数据
                binaryWriter.Close();                   // 关闭二进制写入流
                fileStreamOut.Close();                  // 关闭输出文件流
            }
            catch (Exception )
            {
                //throw e;
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
//using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace DGPF.Security
{
    /// <summary>
    /// 压缩操作相关操作类，压缩格式为ZIP
    /// </summary>
    public class ZIP
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="targetFileName">目标文件名</param>
        /// <param name="sourceFileNames">源文件名</param>
        /// <returns>压缩包压缩是否成功</returns>
        public static bool ZipFile(string targetFileName, params string[] sourceFileNames)
        {
            bool result = true;

            try
            {
                using (ZipFile zip = new ZipFile(Encoding.Default))
                {
                    foreach (string filename in sourceFileNames)
                    {
                        ZipEntry e = zip.AddFile(filename, "");
                    }

                    zip.Save(targetFileName);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="targetFileName">目标文件名</param>
        /// <param name="sourceDirectory">源目录</param>
        /// <returns>压缩包压缩是否成功</returns>
        /// 
        public static bool DoZipFile(string targetFileName, string sourceDirectory)
        {
            bool result = true;

            try
            {
                string[] sourceFileNames = Directory.GetFiles(sourceDirectory);
                using (ZipFile zip = new ZipFile(Encoding.Default))
                {
                    foreach (string filename in sourceFileNames)
                    {
                        ZipEntry e = zip.AddFile(filename, "");
                    }

                    zip.Save(targetFileName);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="targetDirectory">解压后目录</param>
        /// <param name="zipFileName">压缩包文件名</param>
        /// <returns>解压结果是否成功</returns>
        public static bool UnZipFile(string targetDirectory, string zipFileName)
        {
            bool result = true;

            try
            {
                using (ZipFile zip = new ZipFile(zipFileName))
                {
                    zip.ExtractAll(targetDirectory);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// 更新压缩包内容
        /// </summary>
        /// <param name="zipFileName">压缩文件名</param>
        /// <param name="sourceFileName">要更新的文件</param>
        public static void UpdateFileToZip(string zipFileName, string sourceFileName)
        {
            using (ZipFile zip = new ZipFile(zipFileName))
            {
                string fileName = GetFileNameFromFullName(sourceFileName);
                if (zip.ContainsEntry(fileName))
                {
                    var stream = File.OpenRead(sourceFileName);
                    var z = zip.UpdateEntry(fileName, stream);
                    z.Comment = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                zip.Save();
            }
        }
        public static string ExtractSingleFile(string zipFileName, string fileName, string extractDirectoryPath = "")
        {
            string tempFileName = "";
            if (string.IsNullOrEmpty(extractDirectoryPath))
            {
                string temp = Environment.GetEnvironmentVariable("TEMP");
                DirectoryInfo info = new DirectoryInfo(temp);
                var tempInfo = info.CreateSubdirectory(Guid.NewGuid().ToString());
                extractDirectoryPath = tempInfo.FullName;
            }

            byte[] content = null;
            //转换stream为byte[]  
            Func<Stream, byte[]> toByteArray = (stream) =>
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                // 设置当前流的位置为流的开始   
                stream.Seek(0, SeekOrigin.Begin);
                return bytes;
            };

            using (ZipFile zip = new ZipFile(zipFileName, Encoding.Default))
            {
                if (zip.ContainsEntry(fileName))
                {
                    var entries = zip.Entries.Where(u => u.FileName.Equals(fileName));
                    if (entries == null || entries.Count() <= 0)
                    {
                        return "";
                    }

                    var entry = entries.First();

                    //将文件解压到内存流  
                    using (var stream = new MemoryStream())
                    {
                        entry.Extract(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        content = toByteArray(stream);
                    }
                }
            }

            //创建文件  
            tempFileName = Path.Combine(extractDirectoryPath, fileName);
            File.WriteAllBytes(tempFileName, content);

            return tempFileName;
        }
        /// <summary>
        /// 通过路径全名获取文件名
        /// </summary>
        /// <param name="fullFileName">全路井名</param>
        /// <returns>文件名</returns>
        private static string GetFileNameFromFullName(string fullFileName)
        {
            string fileName = fullFileName;
            int index = fullFileName.LastIndexOf('\\');
            if (index >= 0 && index < fullFileName.Length - 1)
            {
                fileName = fullFileName.Substring(index + 1);
            }

            return fileName;
        }
        #region 压缩字符串
        /// <summary>
        /// 将传入字符串以GZip算法压缩后，返回Base64编码字符
        /// </summary>
        /// <param name="rawString">需要压缩的字符串</param>
        /// <returns>压缩后的Base64编码的字符串</returns>
        public static string GZipCompressString(string rawString)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(rawString.ToString());
                byte[] zippedData = Compress(rawData);
                return (string)(Convert.ToBase64String(zippedData));
            }
        }

        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        static byte[] Compress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }


        /// <summary>
        /// 将传入的二进制字符串资料以GZip算法解压缩
        /// </summary>
        /// <param name="zippedString">经GZip压缩后的二进制字符串</param>
        /// <returns>原始未压缩字符串</returns>
        public static string GZipDecompressString(string zippedString)
        {
            if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
                return (string)(System.Text.Encoding.UTF8.GetString(Decompress(zippedData)));
            }
        }


        /// <summary>
        /// ZIP解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }
        #endregion
        
    }
}

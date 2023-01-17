using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TUF.Shared.Authorization;

namespace TUF.Shared.Services
{
    public interface ICrytoManager
    {
        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Decryt(string input);
        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Encryt(string input);
    }
    public class CrytoManager : ICrytoManager
    {
        
        private string key { get; set; } = "c1ab1581297047c68bda1522142e33fd";
        private string iv { get; set; } = "5a32f0a325dd45e2a36b2684c7ce684e";
        private int keysize { get; set; } = 128;
        private int blocksize { get; set; } = 128;
        private CipherMode cipermode { get; set; } = CipherMode.CBC;
        private PaddingMode padding { get; set; } = PaddingMode.PKCS7;


        public string Encryt(string input)
        {
            try
            {
                string rt = string.Empty;
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = keysize; //AES128로 사용시 
                aes.BlockSize = blocksize;
                aes.Mode = cipermode;
                aes.Padding = padding;
                byte[] keyBytes = HexStringToBinary(key);
                byte[] ivBytes = HexStringToBinary(iv);

                aes.Key = keyBytes;
                aes.IV = ivBytes;
                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] buf = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(input);
                        cs.Write(xXml, 0, xXml.Length);
                    }
                    buf = ms.ToArray();
                }
                string Output = ConvertByteToHexString(buf);// Convert.ToBase64String(buf);
                return Output.ToLower();
            }
            catch (Exception exc)
            {
                return "";
            }
        }

        private string ConvertByteToHexString(byte[] convertArr)
        {
            string convertArrString = string.Empty;
            convertArrString = string.Concat(Array.ConvertAll(convertArr, byt => byt.ToString("X2")));
            return convertArrString;
        }


        private byte[] HexStringToBinary(string hexstring)
        {
            var inputByteArray = new byte[hexstring.Length / 2];
            for (var x = 0; x < inputByteArray.Length; x++)
            {
                var i = Convert.ToInt32(hexstring.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            return inputByteArray;
        }


        public string Decryt(string input)
        {
            try
            {
                string rt = string.Empty;
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = keysize; //AES128로 사용시 
                aes.BlockSize = blocksize;
                aes.Mode = cipermode;
                aes.Padding = padding;
                byte[] keyBytes = HexStringToBinary(key);
                byte[] ivBytes = HexStringToBinary(iv);
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                var decrypt = aes.CreateDecryptor();
                byte[] buf = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {

                        byte[] xXml = HexStringToBinary(input);// Encoding.UTF8.GetBytes(input);
                        cs.Write(xXml, 0, xXml.Length);
                    }
                    buf = ms.ToArray();
                }
                string Output = Encoding.UTF8.GetString(buf);
                return Output;
            }
            catch (Exception exc)
            {
                return "";
            }
        }
    }
}

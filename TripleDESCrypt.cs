using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ConvenientTool
{
    class TripleDESCrypt
    {
        private TripleDESCryptoServiceProvider tdes;
        private GetHash hash = new GetHash();
        public TripleDESCrypt()
        {
            tdes = new TripleDESCryptoServiceProvider();
            tdes.Mode = CipherMode.CBC;
            tdes.Key = Encoding.UTF8.GetBytes(this.Rot13(Constants.Key)); //半角24文字 196bit
            tdes.IV = Encoding.UTF8.GetBytes(hash.ComputeMD5(Constants.IV).Substring(0,8)); //半角8文字 64bit
        }

        public bool bf6396d100_FileAngou(string strSrc, string strDest)
        {
            bool blRet = false;

            CryptoStream cs = null;
            FileStream fsOut = null;
            FileStream fsIn = null;

            try
            {
                // 暗号化ファイルを開く
                fsOut = new FileStream(strDest, FileMode.CreateNew, FileAccess.Write);
                ICryptoTransform enc = tdes.CreateEncryptor();
                cs = new CryptoStream(fsOut, enc, CryptoStreamMode.Write);

                fsIn = new FileStream(strSrc, FileMode.Open, FileAccess.Read);
                byte[] buf = new byte[1024];
                int len;
                while ((len = fsIn.Read(buf, 0, buf.Length)) > 0)
                {
                    cs.Write(buf, 0, len);
                }

                blRet = true;
            }
            catch
            {
                if (fsIn != null)
                {
                    fsIn.Close();
                    File.Delete(strDest);
                }
            }
            finally
            {
                if (fsIn != null) fsIn.Close();
                if (cs != null) cs.Close();
                if (fsOut != null) fsOut.Close();
            }
            return blRet;
        }


        public bool bf6396d100_FileHukukou(string strSrc, string strDest)
        {
            bool blRet = false;
            CryptoStream cs = null;
            FileStream fsOut = null;
            FileStream fsIn = null;

            try
            {
                // 復号対象ファイルを開く
                fsIn = new FileStream(strSrc, FileMode.Open, FileAccess.Read);

                // 復号ファイルを開く
                fsOut = new FileStream(strDest, FileMode.CreateNew, FileAccess.Write);

                ICryptoTransform enc = tdes.CreateDecryptor();
                cs = new CryptoStream(fsOut, enc, CryptoStreamMode.Write);

                byte[] buf = new byte[1024];
                int len;
                while ((len = fsIn.Read(buf, 0, buf.Length)) > 0)
                {
                    cs.Write(buf, 0, len);
                }

                blRet = true;
            }
            catch
            {
                if (fsOut != null)
                {
                    fsOut.Close();
                    File.Delete(strDest);
                }
            }
            finally
            {
                if (fsIn != null) fsIn.Close();
                if (cs != null) cs.Close();
                if (fsOut != null) fsOut.Close();
            }

            return blRet;
        }

        /// <summary>
        /// Rot13文字列を取得
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Rot13(string value)
        {
            char[] array = value.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = (int)array[i];

                if (number >= 'a' && number <= 'z')
                {
                    if (number > 'm')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                else if (number >= 'A' && number <= 'Z')
                {
                    if (number > 'M')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                array[i] = (char)number;
            }
            return new string(array);
        }

        public string Key
        {
            get { return this.Rot13(Constants.Key); }
            set { Key = value; }
        }

        public string IV
        {
            get { return this.hash.ComputeMD5(Constants.IV).Substring(0, 8); }
            set { IV = value; }
        }

    }
}

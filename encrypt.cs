class TripleDESCrypt
{
    private TripleDESCryptoServiceProvider tdes;

    public TripleDESCrypt()
    {
        tdes = new TripleDESCryptoServiceProvider();
        tdes.Mode = CipherMode.CBC;
        tdes.Key = Encoding.UTF8.GetBytes(this.Rot13(Constants.conKey));
        tdes.IV = Encoding.UTF8.GetBytes("5d41402a");
    }

    public bool bf6396d100_FileAngou(string strSrc, string strDest)
    {
        bool blRet = false;

        CryptoStream cs = null;
        FileStream fsOut = null;
        FileStream fsIn = null;

        try
        {
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
}


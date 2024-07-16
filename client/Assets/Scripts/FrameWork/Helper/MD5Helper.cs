using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class MD5Helper
{
	public static string FileMD5(string filePath)
	{
		byte[] retVal;
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
	        MD5 md5 = MD5.Create();
			retVal = md5.ComputeHash(file);
		}
		return retVal.ToHex("x2");
	}

    public static string MD5String(string strText)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(strText));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < encryptedBytes.Length; i++)
        {
            sb.AppendFormat("{0:x2}", encryptedBytes[i]);
        }
        return sb.ToString();
    }
}


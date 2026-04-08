using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class MD5Helper
{
	public static string FileMD5(string filePath)
	{
		byte[] retVal;
        using (var file = new FileStream(filePath, FileMode.Open))
        {
	        var md5 = MD5.Create();
			retVal = md5.ComputeHash(file);
		}
		return retVal.ToHex("x2");
	}

    //public static string MD5String(string strText)
    //{
    //    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
    //    byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(strText));
    //    StringBuilder sb = new StringBuilder();
    //    for (int i = 0; i < encryptedBytes.Length; i++)
    //    {
    //        sb.AppendFormat("{0:x2}", encryptedBytes[i]);
    //    }
    //    return sb.ToString();
    //}

    /// <summary>
    /// md5x2
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string Md5x2(string source)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(source);
        var s = Md5(bytes);
        return string.Join("", s.Select(t => t.ToString("x2")));
    }
    /// <summary>
    /// md5
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static byte[] Md5(byte[] source)
    {
        using var m = MD5.Create();
        return m.ComputeHash(source);
    }

    // Base64 编码方法
    public static string EncodeToBase64(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        var bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(bytes);
    }

    // Base64 解码方法
    public static string DecodeFromBase64(string base64Encoded)
    {
        if (string.IsNullOrEmpty(base64Encoded))
            return string.Empty;

        try
        {
            var bytes = Convert.FromBase64String(base64Encoded);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (FormatException)
        {
            Console.WriteLine("无效的 Base64 字符串");
            return string.Empty;
        }
    }
}


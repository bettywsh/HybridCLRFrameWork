using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ZipHelper : MonoBehaviour
{
    /// <summary>
    /// 压缩
    /// </summary>
    /// <param name="source">源目录</param>
    /// <param name="s">ZipOutputStream对象</param>
    public static void Compress(string source, ZipOutputStream s)
    {
        string[] filenames = Directory.GetFileSystemEntries(source);
        foreach (string file in filenames)
        {
            if (Directory.Exists(file))
            {
                Compress(file, s);  //递归压缩子文件夹
            }
            else
            {
                using (FileStream fs = File.OpenRead(file))
                {
                    byte[] buffer = new byte[4 * 1024];
                    ZipEntry entry = new ZipEntry(file.Replace(Application.dataPath.Replace("Assets", "ResCreate") + "/", ""));     //此处去掉盘符，如D:\123\1.txt 去掉D:
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);

                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        s.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }
            }
        }
    }

    /// <summary>
    /// 解压缩
    /// </summary>
    /// <param name="sourceFile">源文件</param>
    /// <param name="targetPath">目标路经</param>
    public static bool Decompress(string sourceFile, string targetPath)
    {
        if (!File.Exists(sourceFile))
        {
            throw new FileNotFoundException(string.Format("未能找到文件 '{0}' ", sourceFile));
        }
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }
        using (ZipInputStream s = new ZipInputStream(File.OpenRead(sourceFile)))
        {
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directorName = Path.Combine(targetPath, Path.GetDirectoryName(theEntry.Name));
                string fileName = Path.Combine(directorName, Path.GetFileName(theEntry.Name));
                // 创建目录
                if (directorName.Length > 0)
                {
                    Directory.CreateDirectory(directorName);
                }
                if (fileName != string.Empty)
                {
                    using (FileStream streamWriter = File.Create(fileName))
                    {
                        int size = 4096;
                        byte[] data = new byte[4 * 1024];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else break;
                        }
                    }
                }
            }
        }
        return true;
    }
}

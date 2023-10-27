
using System.Collections;
using System.Collections.Generic;
#if !UNITY_EDITOR
using System.Collections.Generic;
#endif
using System.IO;
using UnityEngine;
using YooAsset;

/// <summary>
/// ������Դ�嵥
/// </summary>
public class BuildinFileManifest : ScriptableObject
{
    public List<string> BuildinFiles = new List<string>();
}

/// <summary>
/// ��Դ�ļ������ؽ�����
/// </summary>
public class FileStreamDecryption : IDecryptionServices
{
    /// <summary>
    /// ͬ����ʽ��ȡ���ܵ���Դ������
    /// ע�⣺��������������Դ�������ͷŵ�ʱ����Զ��ͷ�
    /// </summary>
    AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
    {
        BundleStream bundleStream = new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        managedStream = bundleStream;
        return AssetBundle.LoadFromStream(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
    }

    /// <summary>
    /// �첽��ʽ��ȡ���ܵ���Դ������
    /// ע�⣺��������������Դ�������ͷŵ�ʱ����Զ��ͷ�
    /// </summary>
    AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
    {
        BundleStream bundleStream = new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        managedStream = bundleStream;
        return AssetBundle.LoadFromStreamAsync(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
    }

    private static uint GetManagedReadBufferSize()
    {
        return 1024;
    }
}

/// <summary>
/// Զ����Դ��ַ��ѯ������
/// </summary>
public class RemoteServices : IRemoteServices
{
    private readonly string _defaultHostServer;
    private readonly string _fallbackHostServer;

    public RemoteServices(string defaultHostServer, string fallbackHostServer)
    {
        _defaultHostServer = defaultHostServer;
        _fallbackHostServer = fallbackHostServer;
    }
    string IRemoteServices.GetRemoteMainURL(string fileName)
    {
        return $"{_defaultHostServer}/{fileName}";
    }
    string IRemoteServices.GetRemoteFallbackURL(string fileName)
    {
        return $"{_fallbackHostServer}/{fileName}";
    }
}

/// <summary>
/// ��Դ�ļ���ѯ������
/// </summary>
public class GameQueryServices : IBuildinQueryServices
{
    public bool Query(string packageName, string fileName)
    {
        // ע�⣺fileName�����ļ���ʽ
        return StreamingAssetsHelper.FileExists(packageName, fileName);
    }
}

public class StreamingAssetsDefine
{
    public const string RootFolderName = "yoo";
}

#if UNITY_EDITOR
/// <summary>
/// StreamingAssetsĿ¼����Դ��ѯ������
/// </summary>
public sealed class StreamingAssetsHelper
{
    public static void Init() { }
    public static bool FileExists(string packageName, string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, StreamingAssetsDefine.RootFolderName, packageName, fileName);
        return File.Exists(filePath);
    }
}
#else
/// <summary>
/// StreamingAssetsĿ¼����Դ��ѯ������
/// </summary>
public sealed class StreamingAssetsHelper
{
	private static bool _isInit = false;
	private static readonly HashSet<string> _cacheData = new HashSet<string>();

	/// <summary>
	/// ��ʼ��
	/// </summary>
	public static void Init()
	{
		if (_isInit == false)
		{
			_isInit = true;
			var manifest = Resources.Load<BuildinFileManifest>("BuildinFileManifest");
			if (manifest != null)
			{
				foreach (string fileName in manifest.BuildinFiles)
				{
					_cacheData.Add(fileName);
				}
			}
		}
	}

	/// <summary>
	/// �����ļ���ѯ����
	/// </summary>
	public static bool FileExists(string packageName, string fileName)
	{
		if (_isInit == false)
			Init();
		return _cacheData.Contains(fileName);
	}
}
#endif


/// <summary>
/// ��Դ�ļ�������
/// </summary>
public class BundleStream : FileStream
{
    public const byte KEY = 64;

    public BundleStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
    {
    }
    public BundleStream(string path, FileMode mode) : base(path, mode)
    {
    }

    public override int Read(byte[] array, int offset, int count)
    {
        var index = base.Read(array, offset, count);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] ^= KEY;
        }
        return index;
    }
}
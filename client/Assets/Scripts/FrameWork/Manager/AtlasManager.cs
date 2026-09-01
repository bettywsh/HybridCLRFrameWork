using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MonoSingleton<AtlasManager>
{
    //Dictionary<string, SpriteAtlas> spriteAtlasList = new Dictionary<string, SpriteAtlas>();
    public override void Init()
    {
        SpriteAtlasManager.atlasRequested += RequestAtlas;
    }

    public override void OnDestroy()
    {
        SpriteAtlasManager.atlasRequested -= RequestAtlas;
        base.OnDestroy();
    }


    void RequestAtlas(string atlasName, System.Action<SpriteAtlas> callback)
    {
        if (atlasName == "Common")
            return;
        Debug.LogError(atlasName);
        SpriteAtlas sa = ResManager.Instance.SceneLoadAsset<SpriteAtlas>($"Assets/App/Atlas/{atlasName}.spriteatlasv2");
        callback?.Invoke(sa);
    }

    public async UniTask<Sprite> GetSprite(string atlasName, string sprite) {
        SpriteAtlas sa = await ResManager.Instance.SceneLoadAssetAsync<SpriteAtlas>($"Assets/App/Atlas/{atlasName}.spriteatlasv2");
        if (sa == null)
        {
            Debug.LogError($"GetSprite atlas is null: {atlasName}/{sprite}");
            return null;
        }
        return sa.GetSprite(sprite);
    }
}

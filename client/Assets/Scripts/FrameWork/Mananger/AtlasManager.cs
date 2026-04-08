using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MonoSingleton<AtlasManager>
{
    //Dictionary<string, SpriteAtlas> spriteAtlasList = new Dictionary<string, SpriteAtlas>();
    public override async UniTask Init()
    {
        await base.Init();
        SpriteAtlasManager.atlasRequested += RequestAtlas;
    }

    private void OnDestroy()
    {
        SpriteAtlasManager.atlasRequested -= RequestAtlas;
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
        Sprite sprites = sa.GetSprite(sprite);
        return sa.GetSprite(sprite);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MonoSingleton<AtlasManager>
{
    Dictionary<string, SpriteAtlas> spriteAtlasList = new Dictionary<string, SpriteAtlas>();
    public override void Init()
    {
        SpriteAtlasManager.atlasRequested += RequestAtlas;
    }

    private void OnDestroy()
    {
        SpriteAtlasManager.atlasRequested -= RequestAtlas;
    }


    async void RequestAtlas(string atlasName, System.Action<SpriteAtlas> callback)
    {
        SpriteAtlas sa = await ResManager.Instance.SceneLoadAssetAsync<SpriteAtlas>( $"Assets/App/Atlas/{atlasName}{ResConst.AtlasExtName}");
        callback(new SpriteAtlas());
    }
}

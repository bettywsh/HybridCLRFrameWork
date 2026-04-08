using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundManager : MonoSingleton<SoundManager>
{

    private AudioSource audio;
    private Transform root;
    private List<AudioSource> effect_audio = new List<AudioSource>();
    private string curModuleName;
    public bool isCanBackGround = true;
    public bool isCanEffect = true;
    public float background_volume = 1f;
    public float effect_volume = 1f;

    public override async UniTask Init()
    {
        await base.Init();
        if (audio == null)
        {
            //创建一个名称为Sound的空GameObject
            GameObject go = this.gameObject;
            //在其上添加AudioListener 和 AudioSource 组件
            go.AddComponent<AudioListener>();
            audio = go.AddComponent<AudioSource>();
            root = go.transform;
            isCanBackGround = CanPlayBackSound();
            background_volume = PlayerPrefs.GetFloat("BackGroundVolume", 1f);
            SetBackGroundVolume(background_volume);
            audio.volume = background_volume;
            isCanEffect = CanPlaySoundEffect();
            effect_volume = PlayerPrefs.GetFloat("SoundEffectVolume", 1f);
            SetEffectVolume(effect_volume);
            for (int i = 0; i < effect_audio.Count; i++)
            {
                effect_audio[i].volume = effect_volume;
            }
        }       
    }


    //存背景音乐音量
    public void SetBackGroundVolume(float volume)
    {
        background_volume = volume;
        PlayerPrefs.SetFloat("BackGroundVolume", background_volume);
        audio.volume = background_volume;
    }

    //存音效音量
    public void SetEffectVolume(float volume)
    {
        effect_volume = volume;
        for (int i = 0;i< effect_audio.Count; i++)
        {
            effect_audio[i].volume = volume;
        }
        PlayerPrefs.SetFloat("SoundEffectVolume", effect_volume);
    }

    //获取背景音乐音量
    public float GetBackGroundVolume()
    {
        return PlayerPrefs.GetFloat("BackGroundVolume");
    }

    //获取音效音量
    public float GetEffectVolume()
    {
        return PlayerPrefs.GetFloat("SoundEffectVolume");
    }


    public async UniTask<AudioClip> LoadAudioClipAsync(string name)
    {
        //不在缓存中 则异步加载资源
        return await ResManager.Instance.CommonLoadAssetAsync<AudioClip>(name);
    }

    public bool CanPlayBackSound()
    {
        string key = "BackSound";
        int i = PlayerPrefs.GetInt(key, 1);
        return i == 1;
    }

    public void SetCanPlayBackSound(bool flag)
    {
        if (CanPlayBackSound() != flag)
        {
            string key = "BackSound";
            int i = PlayerPrefs.GetInt(key, 1);
            if (i == 1)
            {
                PlayerPrefs.SetInt(key, 0);
                StopBacksound();
            }
            else
            {
                PlayerPrefs.SetInt(key, 1);
                if (audio.clip != null)
                {
                    PlayBackSound(audio.clip);
                }
            }
        }
    }

    public async void PlayBackSound(string name)
    {
        audio.loop = true;
        //改为异步加载
        audio.clip = await LoadAudioClipAsync(name);
        if (CanPlayBackSound())
        {
            audio.Play();
        }
        else
        {
            StopBacksound();
        }

    }

    public void PlayBackSound(AudioClip clip)
    {
        audio.loop = true;
        audio.clip = clip;
        if (CanPlayBackSound())
        {
            audio.Play();
        }
        else
        {
            StopBacksound();
        }
    }

    public void PlayEffectSound(AudioClip clip)
    {
        GameObject go = PoolManager.Instance.CreatePool("SoundEffects", ResManager.Instance.CommonLoadAsset<GameObject>("Assets/App/Prefab/Battle/SoundEffect"), root);
        //GameObject go = new GameObject(clip.name);
        go.name = clip.name;
        AudioSource effect = go.GetComponent<AudioSource>();
        effect.loop = false;
        effect.playOnAwake = false;
        effect.volume = effect_volume;
        //改为异步加载
        effect.clip = clip;
        effect_audio.Add(effect);
        effect.Play();

    }

    public async void PlayEffectSound(string name)
    {
        GameObject go = PoolManager.Instance.CreatePool("SoundEffects", ResManager.Instance.CommonLoadAsset<GameObject>("Assets/App/Prefab/Battle/SoundEffect"), root);
        go.name = name;
        AudioSource effect = go.GetComponent<AudioSource>();
        effect.loop = false;
        effect.playOnAwake = false;
        effect.volume = effect_volume;
        //改为异步加载
        effect.clip = await LoadAudioClipAsync(name);
        effect_audio.Add(effect);
        effect.Play();

    }

    public void StopEffectSound(string name) 
    {
        for (int i = effect_audio.Count - 1; i >= 0; i--)
        {
            if (effect_audio[i].name == name)
            {
                PoolManager.Instance.ReturnObjectToPool("SoundEffects", effect_audio[i].gameObject);
                effect_audio.RemoveAt(i);
            }
        }
    }

    public void StopAllEffectSound()
    {
        for (int i = effect_audio.Count - 1; i >= 0; i--)
        {
            PoolManager.Instance.ReturnObjectToPool("SoundEffects", effect_audio[i].gameObject);
            effect_audio.RemoveAt(i);         
        }
    }

    public void PauseAllEffectSound()
    {
        for (int i = effect_audio.Count - 1; i >= 0; i--)
        {
            effect_audio[i].Pause();
        }
    }

    public void UnPauseAllEffectSound()
    {
        for (int i = effect_audio.Count - 1; i >= 0; i--)
        {
            effect_audio[i].UnPause();
        }
    }

    public bool EffectSoundIsOn(string name) {
        bool isON = false;
        for (int i = effect_audio.Count - 1; i >= 0; i--)
        {
            if (effect_audio[i].name == name)
            {
                isON = true;
            }
            else
            {
               isON = false ;
            }
        }
        return isON;
    }

    public void StopBacksound()
    {
        audio.Stop();
    }

    public void PauseBacksound()
    {
        audio.Pause();
    }

    public void UnPauseBacksound()
    {
        audio.UnPause();
    }

    public bool CanPlaySoundEffect()
    {
        string key = "SoundEffect";
        int i = PlayerPrefs.GetInt(key, 1);
        return i == 1;
    }
        
    public void SetCanPlaySoundEffect(bool flag)
    {
        if (CanPlaySoundEffect() != flag)
        {
            string key = "SoundEffect";
            int i = PlayerPrefs.GetInt(key, 1);
            if (i == 1)
            {
                PlayerPrefs.SetInt(key, 0);
            }
            else
            {
                PlayerPrefs.SetInt(key, 1);
            }
        }
    }


    void Update()
    {
        if (effect_audio.Count > 0) {
            for (int i = effect_audio.Count - 1; i >= 0; i--)
            {
                if (!effect_audio[i].isPlaying)
                {
                    PoolManager.Instance.ReturnObjectToPool("SoundEffects", effect_audio[i].gameObject);
                    effect_audio.RemoveAt(i);                    
                }
            }
        }
    }

    public override void Dispose() {
        base.Dispose();
        PoolManager.Instance.DestoryPoolName("SoundEffects");
        GameObject.Destroy(root.gameObject); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer mixer;
    public AudioSource bgmSource;
    public AudioClip[] bgmClips;
    public AudioClip[] diceClips;
    public AudioClip[] UIClips;
    public AudioClip[] battleClips;

    //

    public Slider bgmSlider, sfxSlider;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        BgSoundPlay((int)SEvent.X);
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject sfx = new GameObject(sfxName + "Sound");
        AudioSource audioSource = sfx.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(sfx, clip.length);
    }

    public void BgmVolume(float size)
    {
        //mixer.SetFloat("Bgm",Mathf.Log10(size) * 20);
        mixer.SetFloat("Bgm",(size <= -20) ? -80f : size);

        SaveDataManager.instance.bgmVolume = size;
    }
    public void SfxVolume(float size)
    {
        
        mixer.SetFloat("Sfx",(size <= -30) ? -80f : size);
        mixer.SetFloat("Dice",(size <= -30) ? -80f : size-10);
        
        SaveDataManager.instance.sfxVolume = size;
        
        //mixer.SetFloat("Sfx",Mathf.Log10(size) * 20);
        //mixer.SetFloat("Dice",Mathf.Log10(size) * 20);
    }

    public void DiceSfxPlay()
    {
        var c = Random.Range(3,6);

        for(int i = 0; i<c; i++)
        {
            GameObject sfx = new GameObject(i+ "Sound");
            AudioSource audioSource = sfx.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Dice")[0];
            var tmpClip = diceClips[Random.Range(0,diceClips.Length)];
            audioSource.clip = tmpClip;
            audioSource.Play();

            Destroy(sfx, tmpClip.length);
        }
    }

    
    public void UISfxPlay(int _n)
    {
        GameObject sfx = new GameObject(_n + "Sound");
        AudioSource audioSource = sfx.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = UIClips[_n];
        audioSource.Play();

        Destroy(sfx, UIClips[_n].length);
    }
    public void BattleSfxPlay(int _n)
    {
        GameObject sfx = new GameObject(_n + "Sound");
        AudioSource audioSource = sfx.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = battleClips[_n];
        audioSource.Play();

        Destroy(sfx, battleClips[_n].length);
    }

    public void BgSoundPlay(int _n)
    {
        bgmSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Bgm")[0];
        bgmSource.clip = bgmClips[_n];
        bgmSource.loop = true;
        bgmSource.volume = 0.1f;
        bgmSource.Play();
    }
    
}

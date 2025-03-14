using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance;

    //

    public int language = 1;

    //

    public float bgmVolume = -10;
    public float sfxVolume = -20;

    public int exp = 0, maxExp = 0, level = 0;


    public List<string> job = new List<string>(){"기사","마법사"}; 

    //

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Hi~ People~"+scene.name);
        Debug.Log(bgmVolume+" : "+sfxVolume);
        Debug.Log(language);
        LanguageChange();
        SoundChange();
    }

    void SoundChange()
    {
        if(StartSoundManager.instance != null)
        {
            StartSoundManager.instance.bgmSlider.value = bgmVolume;
            StartSoundManager.instance.sfxSlider.value = sfxVolume;
        }
        if(SoundManager.instance != null)
        {
            SoundManager.instance.bgmSlider.value = bgmVolume;
            SoundManager.instance.sfxSlider.value = sfxVolume;
        }
    }

    void LanguageChange()
    {
        if(StartTextManager.instance != null)StartTextManager.instance.curLanguage = language;
        if(TextManager.instance != null)TextManager.instance.curLanguage = language;
    }

    public void ExpUp(int e)
    {
        exp += e+1;
    }

}

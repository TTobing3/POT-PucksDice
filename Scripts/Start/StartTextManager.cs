using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class StartTextManager : MonoBehaviour
{
    public static StartTextManager instance;
    public System.Action TextRefresh; // 유동적으로 변하는 애들 갱신
    public int curLanguage = 1;
    public TextAsset tSystem;
    public TextMeshProUGUI[] targetText; //  고정되어 있는 언어들
    List<string> defaultText = new List<string>();
    public Dictionary<string, string[]> transLateList = new Dictionary<string, string[]>();

    public int[] fontSize;

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
        
        DefaultLanguageSetting();

        string[] line = tSystem.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }

    }
    void Start()
    {
        ChangeLanguage(curLanguage);
    }
    void DefaultLanguageSetting()
    {
        foreach(TextMeshProUGUI i in targetText)
        {
            defaultText.Add(i.text);
        }
    }
    public void ChangeLanguage(int _language)
    {
        curLanguage = _language;
        SaveDataManager.instance.language = _language;

        DefaultLanguage(_language);

        //TextRefresh();
        
        foreach(TextMeshProUGUI i in targetText)
        {
            foreach(TextMeshProUGUI j in i.GetComponentsInChildren<TextMeshProUGUI>())
            {
                j.text = transLateList[j.text][_language];
            }
        }
    }
    void DefaultLanguage(int _language)
    {
        for(int i = 0; i<targetText.Length; i++)
        {
            foreach(TextMeshProUGUI j in targetText[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                j.text = defaultText[i];
            }
        }
    }
}

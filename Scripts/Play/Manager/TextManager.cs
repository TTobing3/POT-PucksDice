using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public static class Translate
{
    public static string Text(string _text)
    {
        return TextManager.instance.transLateList[_text][TextManager.instance.curLanguage];
    }
    public static string SkillText(string _skill)
    {
        string tmpText = Translate.Text(_skill); 
        foreach(string i in DataManager.instance.skillEft[_skill].Split('/'))
        {
            tmpText += "\n";

            var tmpSkillDes = i.Split(':');
            var tmpSkillDesDetail = tmpSkillDes[1].Split(',');

            float tmpFloat;

            var tmpA = float.TryParse(tmpSkillDesDetail[0], out tmpFloat) ? tmpSkillDesDetail[0] : Translate.Text(tmpSkillDesDetail[0]);
            var tmpB = "";
            if(tmpSkillDesDetail.ToList().Contains("장착"))
            {
                if(tmpSkillDesDetail[2] == "장착")
                {
                    tmpB = Translate.Text("장착한 A*의 개수xB*").Replace("A*",Translate.Text(tmpSkillDesDetail[3])).Replace("B*",tmpSkillDesDetail[1]);
                }
            }
            else if(tmpSkillDesDetail.Length > 1)
            {
                    tmpB = float.TryParse(tmpSkillDesDetail[1], out tmpFloat) ? tmpSkillDesDetail[1] : Translate.Text(tmpSkillDesDetail[1]);
               
            }

            tmpText += Translate.Text(DataManager.instance.skillEftDes[tmpSkillDes[0]]).Replace("A*",tmpA).Replace("B*",tmpB);
        }
        return tmpText;
    }
    public static string AbilityText(string _ability)
    {
        var tmpText = Translate.Text(_ability) + "\n" + Translate.Text(DataManager.instance.abliltyDes[_ability][2]);

        return tmpText;
    }
}
public class TextManager : MonoBehaviour
{
    public static TextManager instance;

    public int curLanguage = 1;

    public System.Action TextRefresh;
    public TextAsset tItem, tName, tMonster, tSkill, tSystem, tMap, tAbility, tEffect, tEffectDes, tEvent, tAchieve, skillDes, skillEftDes, mapDes, eventDes, monsterDes, abliltyDes, composeDes;
    public Dictionary<string, string[]> transLateList = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> mapDesList = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> skillInfoList = new Dictionary<string, string[]>();
    public Dictionary<string, string> skillDesList = new Dictionary<string, string>();


    public TextMeshProUGUI[] dmg;
    public StoryText storyText, achieveText;

    [SerializeField] JobButton jobButton;

    public TextMeshProUGUI[] targetText;
    List<string> defaultText = new List<string>();

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

        #region translate

        DefaultLanguageSetting();
        
        string[] line = tSystem.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        
        line = tMonster.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
           
        }
        line = tItem.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        line = tSkill.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        line = tName.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        
        line = tMap.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        
        line = tAbility.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        line = tEffect.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        line = tEffectDes.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        line = tEvent.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }
        line = tAchieve.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');

            if(!transLateList.ContainsKey(e[0])) transLateList.Add(e[0], e); // 번역할 언어, 언어 / 한글 / 영어
        }

        #endregion


        line = skillDes.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim(); 
            string[] e = line[i].Split('\t');
            skillInfoList.Add(e[0],e);
            skillDesList.Add(e[0], e[3]);
            DataManager.instance.skillEft.Add(e[0],e[2]);
        }
        line = mapDes.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            DataManager.instance.mapDes.Add(e[0],e);
        }
        line = eventDes.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            DataManager.instance.eventDes.Add(e[0],e);
        }
        line = skillEftDes.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            DataManager.instance.skillEftDes.Add(e[0],e[1]);
        }
        line = monsterDes.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            DataManager.instance.enemyName.Add(e[0]);
            DataManager.instance.enemySkills.Add(e[1]);
            DataManager.instance.enemyArmour.Add(e[2]);
            DataManager.instance.enemyStats.Add(e[3]);
        }
        line = abliltyDes.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            DataManager.instance.abliltyDes.Add(e[0],e);
            DataManager.instance.abliltytName.Add(e[0]);
        }
        line = composeDes.text.Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            line[i] = line[i].Trim();
            string[] e = line[i].Split('\t');

            string[] mat = new string[6]{e[1],e[2],e[3],e[4],e[5],e[6]};
            System.Array.Sort(mat);
            var matList = mat.ToList();
            matList.Add(e[0]);

            DataManager.instance.composeDes.Add(matList.ToArray());
        }
    }

    void Start()
    {
        ChangeLanguage(curLanguage);
    }

    public void Dmg(float _dmg, Transform _target, float _strength, bool isPlayer,int _type = 0,bool isHeal = false)
    {

        int playerValue = isPlayer ? 1 : -1;

        foreach(TextMeshProUGUI i in dmg)
        {
            if(!i.gameObject.activeSelf)
            {
                i.gameObject.SetActive(true);

                var iRect = i.GetComponent<RectTransform>();

                i.transform.position = new Vector3(_target.position.x, _target.position.y+1*_target.localScale.y);
                
                i.transform.DOMoveX(i.transform.position.x+Random.Range(-2f*playerValue+_strength,-3f*playerValue+_strength),1.5f);
                i.transform.DOMoveY(i.transform.position.y+Random.Range(-1f,1f),1.5f);
                //i.transform.DOMoveY(i.transform.position.y+Random.Range(-1f,-2f),1).SetDelay(0.5f);
                iRect.localScale = new Vector3(1+_strength, 1+_strength, 1+_strength);

                if(_type == 0)
                {
                    i.color = new Color(1.0f-_strength/2,0.5f-_strength/2,0.5f-_strength/2,1);
                }
                else if(_type == 1)
                {
                    i.color = new Color(0.5f-_strength/2,1.0f-_strength/2,0.5f-_strength/2,1);
                }

                if(isHeal) i.text = '+'+_dmg.ToString();
                else i.text = '-'+_dmg.ToString();

                i.fontStyle = FontStyles.Bold;

                i.DOFade(0,2).OnComplete(()=>i.gameObject.SetActive(false));



                break;
            }
        }
        
    }

    public void Eft(string _eft, bool isPlayer, Transform _target)
    {

        int playerValue = isPlayer ? 1 : -1;

        _eft = Translate.Text(_eft);

        foreach(TextMeshProUGUI i in dmg)
        {
            if(!i.gameObject.activeSelf)
            {
                i.gameObject.SetActive(true);

                var iRect = i.GetComponent<RectTransform>();

                i.transform.position = new Vector3(_target.position.x, _target.position.y+1*_target.localScale.y);
                
                i.transform.DOMoveX(i.transform.position.x+Random.Range(-2f*playerValue,-3f*playerValue),1.5f);
                i.transform.DOMoveY(i.transform.position.y+Random.Range(-1f,1f),1.5f);
                //i.transform.DOMoveY(i.transform.position.y+Random.Range(-1f,-2f),1).SetDelay(0.5f);
                iRect.localScale = new Vector3(1, 1, 1);

                i.color = new Color(0,0,0,1);
                i.text = _eft;

                i.fontStyle = FontStyles.Bold;

                i.DOFade(0,2).OnComplete(()=>i.gameObject.SetActive(false));



                break;
            }
        }

    }

    public void ChangeLanguage(int _language)
    {
        curLanguage = _language;
        SaveDataManager.instance.language = _language;
        
        DefaultLanguage();

        //if(TextRefresh != null) TextRefresh();
        TextRefresh();
        
        foreach(TextMeshProUGUI i in targetText)
        {
            foreach(TextMeshProUGUI j in i.GetComponentsInChildren<TextMeshProUGUI>())
            {
                j.text = transLateList[j.text][_language];
            }
        }
    }

    void DefaultLanguageSetting()
    {
        foreach(TextMeshProUGUI i in targetText)
        {
            defaultText.Add(i.text);
        }
    }

    void DefaultLanguage()
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
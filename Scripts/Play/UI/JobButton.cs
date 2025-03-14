using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.U2D.Animation;

public class JobButton : MonoBehaviour
{
    public TextMeshProUGUI jobText;

    int curJob = 0;

    void Awake()
    {
    }

    void Start()
    {
        TextManager.instance.TextRefresh += JobTextChange;
        JobArmourChange(DataManager.instance.jobs[curJob]);
        JobTextChange();
        GameManager.instance.curPlayer.JobChange(DataManager.instance.jobs[curJob]);

        SelectButton(true);
        SelectButton(false);
    }

    public void SelectButton(bool _isNext)
    {
        if(GameManager.instance.isStarted) return;

        SoundManager.instance.UISfxPlay(10);
        
        if(_isNext)
        {
            if(curJob == SaveDataManager.instance.job.Count-1)
            {
                curJob = 0;
            }
            else
            {
                curJob++;
            }
        }
        else
        {
            if(curJob == 0)
            {
                curJob = SaveDataManager.instance.job.Count-1;
            }
            else
            {
                curJob--;
            }
        }
        JobArmourChange(SaveDataManager.instance.job[curJob]);
        JobTextChange();
        GameManager.instance.curPlayer.JobChange(SaveDataManager.instance.job[curJob]);

        //

        var tmpAbliltyList = new List<string>();

        foreach(string i in DataManager.instance.abliltytName)
        {
            if(DataManager.instance.abliltyDes[i][1] == SaveDataManager.instance.job[curJob])
            {
                tmpAbliltyList.Add(i);
            }
        }
        
        GameManager.instance.curPlayer.abilityList = tmpAbliltyList;

        if(tmpAbliltyList.Count > 0)
        {
            GameManager.instance.abliltyImage.color = Color.white;
            GameManager.instance.abliltyImage.sprite = DataManager.instance.abliltyImages[DataManager.instance.abliltytName.IndexOf(tmpAbliltyList[0])];
        }
        else
        {
            GameManager.instance.abliltyImage.color = Color.clear;
        }

    }
    void JobTextChange()
    {
        foreach(TextMeshProUGUI i in jobText.GetComponentsInChildren<TextMeshProUGUI>())
        {
            i.text = Translate.Text(SaveDataManager.instance.job[curJob]);
        }
    }
    void JobArmourChange(string _job)
    {
        var armour = DataManager.instance.jobArmourSet[ System.Array.IndexOf(DataManager.instance.jobs, _job) ].Split(',');

        foreach(SpriteResolver i in GameManager.instance.curPlayer.GetComponentsInChildren<SpriteResolver>())
        {
            switch(i.GetCategory())
            {
                case "WeaponIn" :
                i.SetCategoryAndLabel("WeaponIn",armour[0]);
                break;
                case "Helmet" :
                i.SetCategoryAndLabel("Helmet",armour[1]);
                break;
                case "Armour" :
                i.SetCategoryAndLabel("Armour",armour[2]);
                break;
                case "ShoulderArmour" :
                i.SetCategoryAndLabel("ShoulderArmour",armour[2]);
                break;
            }
        }
    }
}

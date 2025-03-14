using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class EventEntity : MonoBehaviour
{
    //요구사항
    //보상 : 유물 장비 스킬 스텟
    public Sprite[] eventImages, eventImagesUsed;
    
    public Animator eye;
    public SpriteRenderer dice, eyeRenderer;
    public Sprite[] eyes;
    string curDice;
    
    public int require, amendsStat;

    public string[] amendsSkill, amendsItem;

    public SpriteRenderer spriteRenderer;

    int curEventNumber;
    string curEvent;

    bool isRoll, isSet;

    //


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartSetting(int _number)
    {
        DOTween.Kill(transform);
        DOTween.Kill(spriteRenderer);

        curEventNumber = _number;
        curEvent = DataManager.instance.mapDes[GameManager.instance.curMap][4].Split(',')[curEventNumber];

        TextManager.instance.storyText.TextChange(Translate.Text(curEvent));

        isRoll = false;
        isSet = false;

        spriteRenderer.DOFade(1,0.1f);
        dice.transform.position = new Vector2(transform.position.x,transform.position.y+2);
        dice.transform.rotation = Quaternion.Euler(Vector3.zero);

        eye.SetInteger("P",Random.Range(1,4));
        eye.SetTrigger("roll");

        GameManager.instance.FinishStage();

        spriteRenderer.sprite = DataManager.instance.eventImages[System.Array.IndexOf(DataManager.instance.eventName, curEvent)]; //
        
        SoundManager.instance.DiceSfxPlay();
        DOTween.Shake(() => dice.transform.position, x => dice.transform.position = x, 3, 0.5f, 20, 90, true, true).OnComplete(()=>SetRequire());
    }

    public void SetRequire()
    {
        //요구사항 설정
        require = Random.Range(0,6);

        eye.SetInteger("D",require);
        eye.SetTrigger("stop");

        isSet = true;
    }
    
    public void RollDice()
    {
        if(isRoll || !isSet) return;

        //GameManager.instance.FinishStage(); //숫자보고 먹튀 방지

        SoundManager.instance.DiceSfxPlay();

        isRoll = true;

        DOTween.Kill(transform);

        var d = Random.Range(0,6);

        GameManager.instance.curPlayer.eye.SetInteger("P",Random.Range(1,4));
        GameManager.instance.curPlayer.eye.SetTrigger("roll");
        GameManager.instance.curPlayer.dice.transform.DORotate(new Vector3(0,0,360),1f,RotateMode.FastBeyond360).SetEase(Ease.Unset);
        DOTween.Shake(() => GameManager.instance.curPlayer.dice.transform.position, x => GameManager.instance.curPlayer.dice.transform.position = x, 1f, 0.5f, 10, 70, true, true).OnComplete(()=>RequireCheck(d));
    }

    public void RequireCheck(int _n)
    {
        GameManager.instance.curPlayer.eye.SetInteger("D",_n);
        GameManager.instance.curPlayer.eye.SetTrigger("stop");
        SoundManager.instance.DiceSfxPlay();

        if(_n < require)  //ㅗ
        {
            DOTween.Shake(() => dice.transform.position, x => dice.transform.position = x, 1, 0.5f, 20, 90, true, true);
        }
        else if(_n > require)  //굳
        {
            dice.transform.DOMoveY(transform.position.y+3,0.5f).SetEase(Ease.Unset);
            dice.transform.DOMoveY(transform.position.y-10,0.5f).SetEase(Ease.InQuad).SetDelay(0.75f);
            dice.transform.DORotate(new Vector3(0,0,Random.Range(0,360)),2).OnComplete(() =>GetItem(1));
        }
        else  //완벽 추가보상
        {
            if(curEventNumber == 1)
            {
                GameManager.instance.StatsTextChange("Equal Eye?\nYour Selected...");
                GameManager.instance.curPlayer.DiceChange("StoneDice");
                DOTween.Shake(() => GameManager.instance.curPlayer.dice.transform.position, x => GameManager.instance.curPlayer.dice.transform.position = x, 1f, 0.5f, 10, 70, true, true);
            }
            else
            {
                GameManager.instance.StatsTextChange("Lucky! Equal Eye\nRandom Stats +Eyes");
                GameManager.instance.curPlayer.StatsChange(Random.Range(0,4),_n+1);
            }
            dice.transform.DOScale(new Vector3(1.5f,1.5f),0.5f);
            dice.transform.DOScale(new Vector3(1,1),0.5f).SetDelay(0.5f).OnComplete(() => GetItem(0));
        }

        FinishEvent();
    }

    public void GetItem(int _type)
    {
        
        var tmpStat = DataManager.instance.eventDes[curEvent][2].Split('~');
        amendsStat = Random.Range(int.Parse(tmpStat[0]),int.Parse(tmpStat[1])+1);
        Debug.Log(amendsStat);
        Debug.Log(int.Parse(tmpStat[0]));
        Debug.Log(int.Parse(tmpStat[1]));

        amendsSkill = DataManager.instance.eventDes[curEvent][3].Split(',');

        spriteRenderer.sprite = DataManager.instance.eventUsedImages[System.Array.IndexOf(DataManager.instance.eventName, curEvent)];
        
        GameManager.instance.GetDice(amendsSkill[Random.Range(0,amendsSkill.Length)]);
        GameManager.instance.GetStatDice(amendsStat);

    }

    public void FinishEvent()
    {
        GameManager.instance.eventRoll.DOAnchorPosY(-625,2).SetEase(Ease.Unset);
    }

    void DiceChange(string _diceName)
    {
        curDice = _diceName;
        dice.GetComponent<SpriteResolver>().SetCategoryAndLabel("DiceBase",_diceName);
    }

}

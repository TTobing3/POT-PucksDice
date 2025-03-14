using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillTable : MonoBehaviour
{
    RectTransform rect;
    public bool on = true, isMoving = false;
    

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SkillTableButton()
    {
        if(isMoving) return;

        SoundManager.instance.UISfxPlay(1);
        SoundManager.instance.DiceSfxPlay();

        isMoving = true;

        if(on)
        {
            on = false;
            rect.DOAnchorPos(new Vector2(-1200,0),1).SetEase(Ease.Unset).OnComplete(()=>isMoving = false);

            GameManager.instance.tradeTable.ResetCost();

            foreach(SkillDice i in GameManager.instance.skillDices)
            {
                if(i.selected || !i.gameObject.activeSelf || i.moveLock) continue;
                if(i.cost != -1)
                {
                    GameManager.instance.tradeTable.costs[i.cost] = 0;
                    i.cost = -1;
                }
                if(i.breakSelected) i.breakSelected = false;
                if(i.composeSelected)
                {
                    i.composeNumber = -1; 
                    i.composeSelected = false;
                }
                i.copyNumber = 0;
                DOTween.Kill(i.rect);
                i.rect.DOAnchorPos(new Vector2(0,0),1);
            }
            foreach(StatDice i in GameManager.instance.statDices)
            {
                if(!i.gameObject.activeSelf || i.rolled) continue;
                DOTween.Kill(i.rect);
                i.rect.DOAnchorPos(new Vector2(0,0),1);
            }
        }
        else
        {
            on = true;
            rect.DOAnchorPos(new Vector2(-580,0),1).SetEase(Ease.Unset).OnComplete(()=>isMoving = false);

            if(GameManager.instance.map.on) GameManager.instance.map.MapButton();
            

            foreach(SkillDice i in GameManager.instance.skillDices)
            {
                if(i.selected || !i.gameObject.activeSelf || i.moveLock) continue;
                DOTween.Kill(i.rect);
                i.rect.DOAnchorPos(new Vector2(Random.Range(-330,200),Random.Range(-270,270)),Random.Range(1,3)).SetEase(Ease.Unset);
                i.rect.rotation = Quaternion.Euler(new Vector3(0,0,Random.Range(0,360)));
            }
            foreach(StatDice i in GameManager.instance.statDices)
            {
                if(!i.gameObject.activeSelf || i.rolled) continue;
                DOTween.Kill(i.rect);
                i.rect.DOAnchorPos(new Vector2(Random.Range(-330,200),Random.Range(-270,270)),Random.Range(1,3)).SetEase(Ease.Unset);
                i.rect.rotation = Quaternion.Euler(new Vector3(0,0,Random.Range(0,360)));
            }
            
        }
    }

    public void StartSetting()
    {
        on = false;
        rect.DOAnchorPos(new Vector2(-1200,0),1).SetEase(Ease.Unset).OnComplete(()=>isMoving = false);
    }
}

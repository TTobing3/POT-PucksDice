using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HomeTableButton : MonoBehaviour
{
    public Image homeFade;
    public RectTransform[] tables, actButtons;
    bool isMoving = false;


    public void Select(int i)
    {
        if(isMoving) return;
        isMoving = true;

        homeFade.gameObject.SetActive(true);
        homeFade.DOFade(0.5f,1).OnComplete(()=>isMoving = false);
        //
        if(i == 0) //분해
        {
            SoundManager.instance.UISfxPlay(7);
            tables[0].DOAnchorPosX(650,1);
            actButtons[0].DOAnchorPosY(-350,1);
        }
        if(i == 1) //복제
        {
            SoundManager.instance.UISfxPlay(8);
            tables[1].DOAnchorPosX(650,1);
            actButtons[1].DOAnchorPosY(-350,1);
        }
        if(i == 2) //조합
        {
            SoundManager.instance.UISfxPlay(9);
            tables[2].DOAnchorPosX(650,1);
            actButtons[2].DOAnchorPosY(-350,1);
        }
        if(i == 3)
        {
            
        }
    }

    public void Back()
    {
        if(isMoving) return;
        isMoving = true;

        SoundManager.instance.UISfxPlay(7);

        homeFade.DOFade(0,1).OnComplete(()=>BackSetting());
        tables[0].DOAnchorPosX(1700,0.8f);
        tables[1].DOAnchorPosX(1700,0.8f);
        tables[2].DOAnchorPosX(1900,0.8f);
        actButtons[0].DOAnchorPosY(-800,0.8f);
        actButtons[1].DOAnchorPosY(-800,0.8f);
        actButtons[2].DOAnchorPosY(-800,0.8f);
        if(GameManager.instance.skillTable.on) GameManager.instance.skillTable.SkillTableButton();

    }

    void BackSetting()
    {
        isMoving = false;
        homeFade.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class DiceBreak : MonoBehaviour
{
    public CinemachineVirtualCamera humanCam;

    public ParticleSystem breakEffect;
    public ParticleSystem[] composeEffects;


    public void Break()
    {
        foreach(SkillDice i in GameManager.instance.skillDices)
        {
            if(i.breakSelected)
            {
                SoundManager.instance.UISfxPlay(3);
                breakEffect.Play();
                StartCoroutine(GameManager.instance.CamShake());
                var f = GameManager.instance.GetDice2("주사위 조각");
                GameManager.instance.skillDices[f].rect.anchoredPosition = new Vector2(840,7);
                GameManager.instance.skillDices[f].rect.DOAnchorPos(new Vector2(Random.Range(600,1140),Random.Range(-200,200)),Random.Range(0.5f,1.5f));
                GameManager.instance.skillDices[f].rect.rotation = Quaternion.Euler(new Vector3(0,0,Random.Range(0,360)));
                i.DiceBreak();
            }
        }
    }

    public void Copy()
    {
        SkillDice empty = null;
        SkillDice target = null;

        foreach(SkillDice i in GameManager.instance.skillDices)
        {
            if(i.copyNumber == 1)
            {
                empty = i;
            }
            if(i.copyNumber == 2)
            {
                target = i;
            }
        }

        if(empty == null || target == null) return;
        if(empty.skillName != "빈 주사위") return;

        empty.DiceSelled();

        GameManager.instance.GetDice(target.skillName);
                
        SoundManager.instance.UISfxPlay(4);
    }

    public void Compose()
    {
        //System.Array.Sort(현재 등록해둔)
        //System.Array.Sort(조합표)
        //조합표 : 7칸 짜리 배열 (마지막 칸 결과)
        //조합대 : 6칸 짜리 없음 배열 
        //딕셔너리 : 배열 6칸 - 결과

        string[] composedDices = new string[6]{"없음","없음","없음","없음","없음","없음"};

        var isReady = false;

        foreach(SkillDice i in GameManager.instance.skillDices)
        {
            if(i.composeSelected)
            {
                composedDices[i.composeNumber] = i.skillName;
                composeEffects[i.composeNumber].Play();
                i.DiceBreak();
                
                isReady = true;
            }
        }

        if(!isReady) return;

        StartCoroutine(GameManager.instance.CamShake());

        System.Array.Sort(composedDices);

        var tmpSuccess = true;
        var targetCompose = new string[7]{"없음","없음","없음","없음","없음","없음","주사위 조각"};

        foreach(string[] i in DataManager.instance.composeDes)
        {
            tmpSuccess = true;

            targetCompose = i;

            for(int j = 0; j < 6; j++)
            {
                if(composedDices[j] != i[j]) 
                {
                    tmpSuccess = false;
                    break;
                }
            }

            if(tmpSuccess) break;
        }

        Debug.Log(tmpSuccess);

        if(tmpSuccess)
        {
            var r = GameManager.instance.GetDice2(targetCompose[6]);
            composeEffects[6].Play();
            GameManager.instance.skillDices[r].rect.anchoredPosition = new Vector2(925,25) ;

            SoundManager.instance.UISfxPlay(6);
        }
        else
        {
            var r = GameManager.instance.GetDice2("주사위 조각");
            composeEffects[7].Play();
            GameManager.instance.skillDices[r].rect.anchoredPosition = new Vector2(925,25) ;
            
            SoundManager.instance.UISfxPlay(5);

        }
        /*
        if(DataManager.instance.composeDes.ContainsKey(composedDices))
        {
            var resultDice = DataManager.instance.composeDes[composedDices];

            GameManager.instance.GetDice2(resultDice, new Vector2(-320+580+650,20));
        }
        else
        {
            GameManager.instance.GetDice2("주사위 조각", new Vector2(-320+580+650,20));
        }  
        */
    }

}

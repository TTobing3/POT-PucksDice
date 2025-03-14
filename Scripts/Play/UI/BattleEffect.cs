using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattleEffect : MonoBehaviour
{
    public ParticleSystem[] effects;
    public SpriteRenderer effectImage; 

    public void Play(string _type)
    {
        Debug.Log(_type);
        switch(_type)
        {
            case "마법" :
            effects[0].Play();
            break;

            case "출혈" :
            effects[1].Play();
            break;
            
            case "독" :
            effects[2].Play();
            break;
            case "베기" :
            effectImage.gameObject.SetActive(true);
            effectImage.color = Color.white;
            effectImage.sprite = DataManager.instance.effectImages[System.Array.IndexOf(DataManager.instance.effectName, "베기")];
            effectImage.DOFade(0,1).OnComplete(()=>effectImage.gameObject.SetActive(false));
            break;
            case "거미줄" :
            effectImage.gameObject.SetActive(true);
            effectImage.color = Color.white;
            effectImage.sprite = DataManager.instance.effectImages[System.Array.IndexOf(DataManager.instance.effectName, "거미줄")];
            effectImage.DOFade(0,3).OnComplete(()=>effectImage.gameObject.SetActive(false));
            break;
            case "타격" :
            effectImage.gameObject.SetActive(true);
            effectImage.color = Color.white;
            effectImage.sprite = DataManager.instance.effectImages[System.Array.IndexOf(DataManager.instance.effectName, "타격")];
            effectImage.DOFade(0,0.5f).OnComplete(()=>effectImage.gameObject.SetActive(false));
            effectImage.transform.DOScale(Vector3.zero, 0.5f).OnComplete(()=>effectImage.transform.localScale = Vector3.one);
            break;
            case "물기" :
            effectImage.gameObject.SetActive(true);
            effectImage.color = Color.white;
            effectImage.sprite = DataManager.instance.effectImages[System.Array.IndexOf(DataManager.instance.effectName, "물기")];
            effectImage.transform.DOScaleY(0.5f,0.2f);
            effectImage.transform.DOScaleY(1f,0.2f).SetDelay(0.1f);
            effectImage.DOFade(0, 1f).SetDelay(0.5f).OnComplete(()=>effectImage.gameObject.SetActive(false));
            break;
            case "할퀴기" :
            effectImage.gameObject.SetActive(true);
            var tmpPosition = effectImage.transform.position;
            effectImage.color = Color.white;
            effectImage.sprite = DataManager.instance.effectImages[System.Array.IndexOf(DataManager.instance.effectName, "할퀴기")];
            effectImage.transform.DOMoveY(tmpPosition.y-0.2f,0.2f).OnComplete(()=>effectImage.transform.position = tmpPosition);
            effectImage.DOFade(0, 0.2f).OnComplete(()=>effectImage.gameObject.SetActive(false));
            break;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class StatDice : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] SkillTable skillTable;
    [SerializeField] Animator eyeAnimator;
    public RectTransform rect;
    public Image eyeImage;
    CanvasGroup canvasGroup;

    public GameObject bag;
    public bool selected = false, rolled = false;
    public int number = -1, value;

    public string skillName;
    void Awake()
    {
        eyeImage = eyeAnimator.GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    } 
    void Start()
    {
        //EyeSetting(Random.Range(0,6));
    }
    public void OnBeginDrag(PointerEventData eventData)
    {   
        if(!skillTable.on || rolled) return;

        bag.SetActive(false);
        DOTween.Kill(rect);
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(!skillTable.on || rolled) return;

        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!skillTable.on || rolled) return;
        

        bag.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        SoundManager.instance.DiceSfxPlay();

        //rect.rotation = Quaternion.Euler(new Vector3(0,0,Random.Range(-30,30)));

        rect.DOShakeAnchorPos(0.5f,10,10,90,true,true);
        rect.DOShakeRotation(0.5f,30,30,90,true);
        
    }

    public void EyeSetting(int _value)
    {
        eyeAnimator.SetTrigger("stop");
        eyeAnimator.SetInteger("D", _value);
        value = _value;
    }

    public void StatSelected(int _stat)
    {
        bag.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        SoundManager.instance.DiceSfxPlay();

        //rect.rotation = Quaternion.Euler(new Vector3(0,0,Random.Range(-30,30)));

        GameManager.instance.curPlayer.StatsChange(_stat, value+1);
        rect.DOShakeRotation(2,10,10,90,true).OnComplete(()=>ResetSetting());
        System.Array.ForEach(GetComponentsInChildren<Image>(), x => x.DOFade(0,2));//UIFadeOui
    }

    void ResetSetting()
    {
        rect.anchoredPosition = new Vector3(2500,0,0);
        rolled = false;
        System.Array.ForEach(GetComponentsInChildren<Image>(), x => x.DOFade(1,0.1f).OnComplete(()=>gameObject.SetActive(false)));
    }


    public void OnPointerDown(PointerEventData eventData) //zmfflr
    {
        if(!skillTable.on || rolled) return;

        if(selected)
        {
            GameManager.instance.curPlayer.skills[number] = "";

            selected = false;
            number = -1;
        }
    }

}
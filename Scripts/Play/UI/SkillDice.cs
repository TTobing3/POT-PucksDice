using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
public class SkillDice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] SkillTable skillTable;
    [SerializeField] PopUp skillDes;
    public RectTransform rect;
    RectTransform desRect;
    TextMeshProUGUI desText;
    CanvasGroup canvasGroup;

    public GameObject bag;
    public bool selected = false, moveLock = false, breakSelected = false, composeSelected = false;
    public int number = -1, cost = -1, copyNumber = 0, composeNumber = -1;

    public string skillName;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        desRect = skillDes.GetComponent<RectTransform>();
        desText = skillDes.GetComponentInChildren<TextMeshProUGUI>();
    } 
    void OnEnable() 
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {   
        if(!skillTable.on || moveLock) return;

        if(cost != -1) GameManager.instance.tradeTable.costs[cost] = 0;
        
        cost = -1;

        skillDes.gameObject.SetActive(true);
        skillDes.TextChange($"{Translate.SkillText(skillName)}");

        bag.SetActive(false);
        DOTween.Kill(rect);
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(!skillTable.on || moveLock) return;

        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        desRect.anchoredPosition = new Vector2(rect.anchoredPosition.x-25,rect.anchoredPosition.y+50);

        if( desRect.anchoredPosition.x - desRect.sizeDelta.x + 416 < 0 ) 
        {
            desRect.anchoredPosition = 
            new Vector2(rect.anchoredPosition.x+desRect.sizeDelta.x+25,rect.anchoredPosition.y+50);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!skillTable.on || moveLock) return;
        
        skillDes.gameObject.SetActive(false);
        
        bag.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        SoundManager.instance.DiceSfxPlay();
        
        //rect.rotation = Quaternion.Euler(new Vector3(0,0,Random.Range(-30,30)));

        rect.DOShakeAnchorPos(1,10,10,90,true,true);
        rect.DOShakeRotation(1,10,10,90,true);
    }
    public void OnPointerDown(PointerEventData eventData) //zmfflr
    {
        skillDes.gameObject.SetActive(true);
        skillDes.TextChange($"{Translate.SkillText(skillName)}");
        
        if(skillTable.on)
        {
            desRect.anchoredPosition = new Vector2(rect.anchoredPosition.x-25,rect.anchoredPosition.y+50);

            if( desRect.anchoredPosition.x - desRect.sizeDelta.x + 416 < 0 ) 
            {
                desRect.anchoredPosition = 
                new Vector2(rect.anchoredPosition.x+desRect.sizeDelta.x+25,rect.anchoredPosition.y+50);
            }
        }
        else
        { 
            desRect.anchoredPosition = 
            new Vector2(rect.anchoredPosition.x+desRect.sizeDelta.x+25,rect.anchoredPosition.y-desRect.sizeDelta.y/2);
        }

        if(!skillTable.on || moveLock) return;

        if(selected)
        {
            GameManager.instance.curPlayer.skills[number] = "";

            selected = false;
            number = -1;
        }
        if(breakSelected) breakSelected = false;
        if(composeSelected)
        {
            composeNumber = -1; 
            composeSelected = false;
        }
        copyNumber = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        skillDes.gameObject.SetActive(false);
    }

    public void DiceBreak()
    {
        breakSelected = false;
        composeNumber = -1; 
        composeSelected = false;
        copyNumber = 0;
        moveLock = true;
        SellFinish();
    }

    public void DiceSelled()
    {
        breakSelected = false;
        composeNumber = -1; 
        composeSelected = false;
        copyNumber = 0;
        moveLock = true;
        rect.DOAnchorPos(new Vector2(2500,0),2).OnComplete(()=>SellFinish());
    }

    void SellFinish()
    {
        DOTween.Kill(rect);
        moveLock = false;
        rect.anchoredPosition = new Vector3(2500,0,0);
        selected = false;
        number = -1;
        cost = -1;
        skillName = "";
        gameObject.SetActive(false);
        //System.Array.ForEach(GetComponentsInChildren<Image>(), x => x.DOFade(1,0.1f).OnComplete(()=>gameObject.SetActive(false)));
    }


}

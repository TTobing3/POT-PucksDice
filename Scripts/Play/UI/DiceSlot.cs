using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DiceSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] int number, kind;
    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && kind == 0 && GameManager.instance.curPlayer.skills[number] == "")
        {
            if(!eventData.pointerDrag.gameObject.name.Contains("SkillDice")) return;

            var targetRect = eventData.pointerDrag.GetComponent<RectTransform>();
            targetRect.anchoredPosition = rect.anchoredPosition;
            targetRect.rotation = rect.rotation;

            var targetDice = eventData.pointerDrag.GetComponent<SkillDice>();
            targetDice.selected = true;
            targetDice.number = number;

            GameManager.instance.curPlayer.skills[number] = targetDice.skillName;
        }
        if(eventData.pointerDrag != null && kind == 1)
        {
            if(!eventData.pointerDrag.gameObject.name.Contains("StatDice")) return;
            
            var targetRect = eventData.pointerDrag.GetComponent<RectTransform>();
            targetRect.anchoredPosition = rect.anchoredPosition;
            targetRect.rotation = rect.rotation;

            var targetDice = eventData.pointerDrag.GetComponent<StatDice>();
            targetDice.StatSelected(number);
            targetDice.rolled = true;
        }
        if(eventData.pointerDrag != null && kind == 2 && GameManager.instance.tradeTable.costs[number] == 0)
        {
            if(!eventData.pointerDrag.gameObject.name.Contains("SkillDice")) return;
            
            var targetRect = eventData.pointerDrag.GetComponent<RectTransform>();
            targetRect.anchoredPosition = new Vector2(rect.anchoredPosition.x+580, rect.anchoredPosition.y);
            targetRect.rotation = rect.rotation;

            var targetDice = eventData.pointerDrag.GetComponent<SkillDice>();
            targetDice.cost = number;

            GameManager.instance.tradeTable.costs[number]++;

        }
        if(eventData.pointerDrag != null && kind == 3)
        {
            if(!eventData.pointerDrag.gameObject.name.Contains("SkillDice")) return;
            
            foreach(SkillDice i in GameManager.instance.skillDices)
            {
                if(i.breakSelected) return;
            }
            
            var targetRect = eventData.pointerDrag.GetComponent<RectTransform>();
            targetRect.anchoredPosition = new Vector2(rect.anchoredPosition.x+580+650, rect.anchoredPosition.y);
            targetRect.rotation = rect.rotation;

            //올려진 주사위 정보, 주사위 정보를 부술 주사위에 주고
            //주사위 부수기 버튼
            //
            var targetDice = eventData.pointerDrag.GetComponent<SkillDice>();
            targetDice.breakSelected = true;


            //올려진 거 dice selld
            //get dice / 주사위 조각
        }
        if(eventData.pointerDrag != null && kind == 4)
        {
            if(!eventData.pointerDrag.gameObject.name.Contains("SkillDice")) return;
            
            var targetRect = eventData.pointerDrag.GetComponent<RectTransform>();
            targetRect.anchoredPosition = new Vector2(rect.anchoredPosition.x+580+650, rect.anchoredPosition.y);
            targetRect.rotation = rect.rotation;

            var targetDice = eventData.pointerDrag.GetComponent<SkillDice>();

            if(number == 1) //재료
            {
                targetDice.copyNumber = 1;
            }
            else if(number == 2) //대상
            {
                targetDice.copyNumber = 2;
            }

            //다이스에 카피 타겟, 선택된 빈 주사위

            
        }
        if(eventData.pointerDrag != null && kind == 5)
        {
            if(!eventData.pointerDrag.gameObject.name.Contains("SkillDice")) return;
            
            var targetRect = eventData.pointerDrag.GetComponent<RectTransform>();
            targetRect.anchoredPosition = new Vector2(rect.anchoredPosition.x+580+650, rect.anchoredPosition.y);
            targetRect.rotation = rect.rotation;

            var targetDice = eventData.pointerDrag.GetComponent<SkillDice>();

            targetDice.composeNumber = number;
            targetDice.composeSelected = true;

            //다이스에 카피 타겟, 선택된 빈 주사위
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class EnemyPopUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] PopUp skillDes; //
    [SerializeField] int n; //
    RectTransform rect;
    RectTransform desRect; //
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        
        desRect = skillDes.GetComponent<RectTransform>();
    } 
    public void OnPointerDown(PointerEventData eventData) //zmfflr
    {
        skillDes.gameObject.SetActive(true);
        skillDes.TextChange($"{Translate.SkillText(GameManager.instance.enemySkills[n])}");
        
        desRect.anchoredPosition = 
        new Vector2(rect.anchoredPosition.x-25,rect.anchoredPosition.y-desRect.sizeDelta.y/2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        skillDes.gameObject.SetActive(false);
    }
}

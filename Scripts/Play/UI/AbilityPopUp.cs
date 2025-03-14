using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityPopUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] PopUp abliltyDes; //
    [SerializeField] public int n; //
    public RectTransform rect;
    public Image image;
    RectTransform desRect; //
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>(); 
        desRect = abliltyDes.GetComponent<RectTransform>();
    } 
    public void OnPointerDown(PointerEventData eventData) //zmfflr
    {

        if(GameManager.instance.curPlayer.abilityList.Count > n)
        {
            abliltyDes.gameObject.SetActive(true);
            abliltyDes.TextChange($"{Translate.AbilityText(GameManager.instance.curPlayer.abilityList[n])}"); // "\n{Translate.SkillText(GameManager.instance.enemySkills[n])}");
            
            desRect.anchoredPosition = 
            new Vector2(rect.anchoredPosition.x,rect.anchoredPosition.y-desRect.sizeDelta.y/2);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        abliltyDes.gameObject.SetActive(false);
    }
}

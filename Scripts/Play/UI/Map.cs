using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class Map : MonoBehaviour
{
    public RectTransform[] box;
    public RectTransform[] dotLine;
    public Image[] iconImages, decoImage;
    public Sprite[] icons, deco, deco1;
    public bool on = false, isMoving = false;
    RectTransform rect;

    public TextMeshProUGUI mapName;

    void Awake()
    {
    }
    void Start()
    {
        TextManager.instance.TextRefresh += MapNameChange;
        rect = GetComponent<RectTransform>();
    }
    public void MapButton()
    {
        if(isMoving) return;

        isMoving = true;

        SoundManager.instance.UISfxPlay(0);

        if(on)
        {
            on = false;
            rect.DOAnchorPos(new Vector2(1600,0),2).SetEase(Ease.Unset).OnComplete(()=>isMoving = false);
        }
        else
        {
            on = true;
            rect.DOAnchorPos(new Vector2(220,0),2).SetEase(Ease.Unset).OnComplete(()=>isMoving = false);

            if(GameManager.instance.skillTable.on) GameManager.instance.skillTable.SkillTableButton();
        }

    }

    public void MapNameChange()
    {
        mapName.text = "- "+Translate.Text(GameManager.instance.curMap)+" -";
    }

    public void MapOut()
    {
        isMoving = true;
        rect.DOAnchorPos(new Vector2(1900,0),1).SetEase(Ease.Unset).OnComplete(()=>isMoving = false);
    }
    public void MapIn()
    {
        isMoving = true;
        on = false;
        rect.DOAnchorPos(new Vector2(1600,0),1).SetEase(Ease.Unset).OnComplete(()=>isMoving = false);
    }


}

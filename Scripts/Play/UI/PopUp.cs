using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class PopUp : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    Image image;
    RectTransform rect;

    void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void TextChange(string _text)
    {
        text.text = _text;
        rect.sizeDelta = new Vector2(text.preferredWidth+100, text.preferredHeight+20) ;
    }

}

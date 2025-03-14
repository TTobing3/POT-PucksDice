using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class StoryText : MonoBehaviour
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
        gameObject.SetActive(true);

        image.color = Color.clear;
        text.color = Color.clear;

        image.DOColor(new Color(1,0.78f,0.2f,1),2);
        image.DOColor(new Color(1,0.78f,0.2f,0),1).SetDelay(1.5f).OnComplete(()=>gameObject.SetActive(false));
        text.DOColor(new Color(1,0.78f,0.2f,1),2);
        text.DOColor(new Color(1,0.78f,0.2f,0),1).SetDelay(1.5f);

        text.text = _text;
        rect.sizeDelta = new Vector2(text.preferredWidth+100, rect.sizeDelta.y) ;
    }
    public void AchieveTextChange(string _text)
    {
        gameObject.SetActive(true);

        Debug.Log(System.Array.IndexOf(DataManager.instance.achieveName,_text));

        //Debug.Log();

        image.sprite = DataManager.instance.achieveImage[0];

        image.color = Color.clear;
        text.color = Color.clear;

        image.DOColor(Color.white,2);
        image.DOFade(0,1).SetDelay(5f).OnComplete(()=>gameObject.SetActive(false));
        text.DOColor(new Color(0.2f,0.2f,0.2f,1),2);
        text.DOFade(0,1).SetDelay(5f);

        text.text = Translate.Text(_text);
        //rect.sizeDelta = new Vector2(text.preferredWidth+100, rect.sizeDelta.y) ;
    }
}

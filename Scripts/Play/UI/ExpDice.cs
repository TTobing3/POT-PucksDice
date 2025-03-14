using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ExpDice : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] Animator eyeAnimator;
    [SerializeField] ParticleSystem getEffect;
    public RectTransform rect;
    public int value = 0;
    public bool used = false;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void OnEnable() 
    {
        //생성 시 화면 안 무작위 위치로 가도록
    }
    public void EyeSetting(int _value)
    {
        Debug.Log(_value);
        eyeAnimator.SetTrigger("stop");
        eyeAnimator.SetInteger("D", _value);
        value = _value;
    }
    public void OnPointerDown(PointerEventData eventData) //zmfflr
    {
        //빛 입자 화 하면서 - 파티클
        //투명도 내려서 사라지고
        //경험치 업
        Use();
    }
    public void Use()
    {
        if(used) return;
        SaveDataManager.instance.ExpUp(value);


        used = true;

        getEffect.Play();

        foreach(Image i in GetComponentsInChildren<Image>())
        {
            i.DOFade(0,0.5f).OnComplete(()=>ResetSetting());
        }
    }
    void ResetSetting()
    {
        rect.anchoredPosition = new Vector2(0,650);
        System.Array.ForEach(GetComponentsInChildren<Image>(), x => x.DOFade(1,0.1f).OnComplete(()=>gameObject.SetActive(false)));
    }

    void OnDisable() 
    {
        used =false;
        value = 0;
    }
}

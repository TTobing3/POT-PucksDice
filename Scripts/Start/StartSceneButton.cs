using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
public class StartSceneButton : MonoBehaviour
{

    public RectTransform startButtonRect, recordButtonRect, titleTextRect, modeTitleTextRect, settingPageRect;
    public Image fade, settingFade, maxFade, AdPaper, ChPaper;
    public TextMeshProUGUI AdText, ChText;
    public GameObject modeTitle, setting;

    bool isMoving;

    public void Start()
    {
        fade.gameObject.SetActive(true);
        fade.color = new Color(0,0,0,1);
        fade.DOFade(0,1).OnComplete(()=>fade.gameObject.SetActive(false));
    }

    public void StartButton()
    {
        if(isMoving) return;
        isMoving = true;

        StartSoundManager.instance.UISfxPlay(0);
        
        titleTextRect.DOAnchorPos(new Vector2(130,800),0.5f);

        startButtonRect.DOAnchorPos(new Vector2(950,0),0.3f).SetEase(Ease.Unset);
        startButtonRect.DOAnchorPos(new Vector2(2000,0),0.7f).SetEase(Ease.OutQuad).SetDelay(0.3f).OnComplete(()=>MoveEnd());

        recordButtonRect.DOAnchorPos(new Vector2(950,-350),0.7f).SetEase(Ease.OutQuad).SetDelay(0.1f);
        recordButtonRect.DOAnchorPos(new Vector2(2000,-350),0.7f).SetEase(Ease.OutQuad).SetDelay(0.4f);

        fade.gameObject.SetActive(true);
        DOTween.Kill(fade);
        fade.DOFade(0.5f,1);
    }
    
    public void SettingButton()
    {
        if(isMoving) return;
        isMoving = true;

        StartSoundManager.instance.UISfxPlay(2);

        setting.SetActive(true);
        
        settingPageRect.DOAnchorPos(new Vector2(100,0),1.5f).SetEase(Ease.OutQuad).OnComplete(()=>isMoving = false);
        settingFade.gameObject.SetActive(true);
        DOTween.Kill(fade);
        settingFade.DOFade(0.5f,1);
    }

    public void SettingOffButton()
    {
        if(isMoving) return;
        isMoving = true;

        StartSoundManager.instance.UISfxPlay(2);

        settingPageRect.DOAnchorPos(new Vector2(2000,0),1.5f).SetEase(Ease.OutQuad).OnComplete(()=>SettingOffFinish());
        DOTween.Kill(settingFade);
        settingFade.DOFade(0f,1);
    }

    void SettingOffFinish()
    {
        setting.SetActive(false);
        settingFade.gameObject.SetActive(false);
        isMoving = false;

    }

    public void RecordButton()
    {
        if(isMoving) return;
        isMoving = true;
        
        titleTextRect.DOAnchorPos(new Vector2(130,800),0.5f);

        startButtonRect.DOAnchorPos(new Vector2(950,0),0.7f).SetEase(Ease.Unset).SetDelay(0.1f);
        startButtonRect.DOAnchorPos(new Vector2(1600,0),0.7f).SetEase(Ease.OutQuad).SetDelay(0.4f);

        recordButtonRect.DOAnchorPos(new Vector2(950,-350),0.3f).SetEase(Ease.OutQuad);
        recordButtonRect.DOAnchorPos(new Vector2(1600,-350),0.7f).SetEase(Ease.OutQuad).SetDelay(0.3f).OnComplete(()=>MoveEnd());

        fade.gameObject.SetActive(true);
        fade.DOFade(0.5f,1);

    }

    void MoveEnd()
    {
        isMoving = false;
        ModeOn();
    }

    void ModeOn()
    {
        modeTitle.SetActive(true);
        modeTitleTextRect.DOAnchorPos(new Vector2(0,350),0.7f).SetEase(Ease.OutQuad);

        FadeInUI(AdPaper.gameObject,false);
        FadeInUI(ChPaper.gameObject,false);

        //AdPaper.DOColor(new Color(1, 0.5f, 0.5f, 1), 0.5f).SetEase(Ease.OutQuad);
        //ChPaper.DOColor(new Color(0.5f, 0.5f, 1, 1), 0.5f).SetEase(Ease.OutQuad);
        //AdText.DOColor(new Color(0.7f, 0.5f, 0.5f, 1), 0.5f).SetEase(Ease.OutQuad);
        //ChText.DOColor(new Color(0.5f, 0.5f, 0.7f, 1), 0.5f).SetEase(Ease.OutQuad);
    }

    public void ReSetting()
    {
        if(isMoving) return;
        isMoving = true;

        titleTextRect.DOAnchorPos(new Vector2(130,300),1.2f);
        startButtonRect.DOAnchorPos(new Vector2(1000,0),1f);
        recordButtonRect.DOAnchorPos(new Vector2(1000,-350),1.2f);
    }

    public void FadeInUI(GameObject _target, bool _fadeOut)
    {
        _target.gameObject.SetActive(true);
        foreach(Image i in _target.GetComponentsInChildren<Image>())
        {
            i.DOFade(1,1).SetEase(Ease.OutQuad);
            if(_fadeOut) i.DOFade(0,1).SetDelay(1).OnComplete(()=>_target.SetActive(false)).SetEase(Ease.OutQuad);
            
        }
        foreach(TextMeshProUGUI i in _target.GetComponentsInChildren<TextMeshProUGUI>())
        {
            i.DOFade(1,1).SetEase(Ease.OutQuad);
            if(_fadeOut) i.DOFade(0,1).SetDelay(1).SetEase(Ease.OutQuad);
        }
    }

    public void ModeSelect(string _mode)
    {
        StartSoundManager.instance.UISfxPlay(1);

        if(_mode == "모험")
        {
            ModeStart();
        }
        if(_mode == "도전")
        {
            ModeStart();
        }
    }

    void ModeStart()
    {
        maxFade.gameObject.SetActive(true);
        maxFade.DOFade(1,1).OnComplete(()=> GoToADScene());
        
    }

    void GoToADScene()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("PlayScene");
    }
}

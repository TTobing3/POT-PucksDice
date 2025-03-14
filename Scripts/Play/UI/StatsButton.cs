using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class StatsButton : MonoBehaviour
{
    public Animator eye;
    public SpriteRenderer dice, eyeRenderer;
    public Sprite[] eyes;
    public bool rolling;

    public bool isRoll= false;

    RectTransform rect;
    Vector2 startingPosition;
    
    private void Start() {
        rect = GetComponent<RectTransform>();
        startingPosition = rect.anchoredPosition;
    }

    public void RollDice(int _stat)
    {
        if(isRoll) return;

        DOTween.Kill(dice.transform);
        eye.ResetTrigger("stop");

        if(GameManager.instance.tu2.activeSelf) GameManager.instance.tu2.SetActive(false);

        SoundManager.instance.DiceSfxPlay();

        DOTween.Kill(transform);

        isRoll = true;
        rolling = true;

        var d = Random.Range(0,6);

        GameManager.instance.startingValue[d]++;

        eye.SetInteger("P",Random.Range(1,4));
        eye.SetTrigger("roll");
        dice.transform.DORotate(new Vector3(0,0,360),0.5f,RotateMode.FastBeyond360).SetEase(Ease.Unset);
        DOTween.Shake(() => dice.transform.position, x => dice.transform.position = x, 0.5f, 0.5f, 10, 70, true, true).OnComplete(()=>SetStats(d,_stat));
    }
    public void SetStats(int _number, int _stat)
    {
        
        dice.transform.DOMove(new Vector2(GameManager.instance.curPlayer.transform.position.x,GameManager.instance.curPlayer.transform.position.y+3), 0.3f);
        eye.SetTrigger("stop");
        eye.SetInteger("D",_number);

        _number++;

        DOTween.Shake(() => transform.position, x => transform.position = x, 0.3f, 0.5f, 15, 90, true, true).OnComplete(()=>StartGame());
        
        SoundManager.instance.DiceSfxPlay();

        foreach(TextMeshProUGUI i in GetComponentsInChildren<TextMeshProUGUI>())
        {
            i.text = _number.ToString();
        }

        GameManager.instance.curPlayer.StatsChange(_stat,_number);


    }

    void StartGame()
    {
        rolling = false;
        var isEnd = true;
        foreach(StatsButton i in GameManager.instance.startButtons)
        {
            if(!i.isRoll || i.rolling) 
            {
                isEnd = false;
            }
        }

        if(isEnd)
        {
            //var luckyTime = 1;
            /*
            foreach(int i in GameManager.instance.startingValue)
            {
                if(i==4)
                {
                    GameManager.instance.curPlayer.StatsChange(0,4);
                    GameManager.instance.curPlayer.StatsChange(0,4);
                    GameManager.instance.curPlayer.StatsChange(0,4);
                    GameManager.instance.curPlayer.StatsChange(0,4);
                    GameManager.instance.StatsTextChange("What The Fore?\nALL STATS +4");
                } 
                if(i==3) 
                {
                    GameManager.instance.curPlayer.StatsChange(Random.Range(0,4),3);
                    GameManager.instance.StatsTextChange("OMG! 3Dice\nRandom Stat +3");
                    GameManager.instance.GetDice(DataManager.instance.skillNames[Random.Range(0, DataManager.instance.skillNames.Length)]);
                }
                if(i==2) 
                {
                    GameManager.instance.StatsTextChange("Lucky Pair\nRandom Dice");
                    GameManager.instance.GetDice(DataManager.instance.skillNames[Random.Range(0, DataManager.instance.skillNames.Length)]);
                }
            }
            */
            //dice.DOFade(1,luckyTime).OnComplete(()=>GameManager.instance.StartGame());

            GameManager.instance.StartGame();
            
        }

    }
}

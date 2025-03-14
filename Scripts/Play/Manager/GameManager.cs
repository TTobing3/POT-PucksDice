using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SEvent{Q, E, T ,H, X, B}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //

    public GameObject settingObject;


    //

    public GameObject tu, tu2;
    public int[] startingValue = new int[6];

    //

    public BattleEffect playerParticle, enemyParticle;

    //
    public Image abliltyImage;
    //

    public SpriteRenderer curBackground;
    public Image fade, finalFade, statsText;
    public TextMeshProUGUI defeat, victory;
    public GameObject defeatPanel, expBar, nextButton, homePanel, homeBack;
    public Map map;
    public SkillTable skillTable;
    public TradeTable tradeTable;
    public SkillDice[] skillDices; 
    public StatDice[] statDices;
    public ExpDice[] expDices;

    //
    
    public StatBar statBar;
    public RectTransform playerBar, enemyBar, battleStopRect, homeButton;
    public TextMeshProUGUI enemyNameText, enemyStatText;

    //

    public RectTransform enemyTable;
    public string[] enemySkills = new string[6];
    public Image[] enemySkillList;

    //

    public RectTransform eventRoll;

    //

    public CinemachineVirtualCamera humanCam;
    CinemachineBasicMultiChannelPerlin humanCamShake;
    public CinemachineFramingTransposer humanCamComposer;

    //

    public BattleStopButton battleStopImage;
    public bool isStarted = false, battleStop = false, reserveStop = false;
    public GameObject startPanel;
    public StatsButton[] startButtons;
    public GameObject[] startObjects;

    //

    public List<string> skillList = new List<string>();
    //public string[] skillSetting = new string[6];

    public AbilityPopUp[]  abilityImages;

    //

    public int curLevel = 0; // 몇 단계인지
    public int[] expGain = new int[6]{0,0,0,0,0,0};
    public string curMap; // 숲, 동굴 그런거 -> 스테이지 제네레이트 하는거임
    int curMapType;

    //

    public Entity curPlayer, curEnemy;
    public GameObject curTrader;
    public EventEntity eventEntity;
    public SEvent?[] stages = new SEvent?[6]{null, null, null, null, null, null};
    public int curStage;

    public int[] tmpStage = new int[6];

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        humanCamComposer = humanCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        humanCamShake = humanCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        //
        
        curMap = DataManager.instance.mapLevel[curLevel].Split(',')[Random.Range(0,2)];
        curBackground.sprite = DataManager.instance.mapImage[System.Array.IndexOf(DataManager.instance.mapName, curMap)];

        //

        fade.gameObject.SetActive(true);
        fade.color = Color.black;
        fade.DOFade(0,1.5f).SetEase(Ease.InQuad).OnComplete(()=>fade.gameObject.SetActive(false));

        //
        
        TextManager.instance.TextRefresh += EnemyTextChange;
    }

    public void StatsTextChange(string _text)
    {
        statsText.gameObject.SetActive(true);
        statsText.DOFade(0.5f,1);
        statsText.DOFade(0,1).SetDelay(2).OnComplete(()=>statsText.gameObject.SetActive(false));

        foreach(TextMeshProUGUI i in statsText.GetComponentsInChildren<TextMeshProUGUI>())
        {
            i.text = _text;
            i.DOFade(1,1);
            i.DOFade(0,1).SetDelay(2);
        }
    }

    public void StartGame()
    {
        if(isStarted) return;

        isStarted = true;

        GenerateStage();

        foreach(StatsButton i in startButtons)
        {
            i.transform.DOMove(new Vector2(curPlayer.transform.position.x, curPlayer.transform.position.y+3), 0.5f).SetDelay(1.5f).SetEase(Ease.Unset);
            i.transform.DOScale(Vector3.zero,1).SetDelay(1.5f).OnComplete(()=>i.gameObject.SetActive(false));
            i.GetComponent<Image>().DOFade(0, 1.3f).SetDelay(1f);
        }

        FadeOutUI(startObjects[0]);
        FadeOutUI(startObjects[1]);
        FadeOutUI(startObjects[2]);

        DOTween.To(()=>humanCam.m_Lens.OrthographicSize,x=>humanCam.m_Lens.OrthographicSize=x,5,3).OnComplete(()=>AfterStart());
        humanCam.m_Follow = curPlayer.transform;

        //*tmp skilltest

        foreach(string i in DataManager.instance.jobSkills[curPlayer.job].Split(','))
        {
            GetDice(i);
        }
            GetDice("베기");
            GetDice("물기");
            GetDice("할퀴기");
            GetDice("거미줄");
            GetDice("베기");

        //

        abliltyImage.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-630,480),1);
    }

    void AfterStart()
    {
        tu.SetActive(true);
        map.MapButton();
        skillTable.StartSetting();
        homeButton.DOAnchorPosX(-870,1);
    }
    
    public void FadeOutUI(GameObject _target)
    {
        foreach(Image i in _target.GetComponentsInChildren<Image>())
        {
            i.DOFade(0,3).OnComplete(()=>_target.SetActive(false));
        }
        foreach(TextMeshProUGUI i in _target.GetComponentsInChildren<TextMeshProUGUI>())
        {
            i.DOFade(0,3);
        }
    }
    public void FadeInUI(GameObject _target, bool _fadeOut)
    {
        _target.gameObject.SetActive(true);
        foreach(Image i in _target.GetComponentsInChildren<Image>())
        {
            i.DOFade(1,3);
            if(_fadeOut) i.DOFade(0,3).SetDelay(3).OnComplete(()=>_target.SetActive(false));
            
        }
        foreach(TextMeshProUGUI i in _target.GetComponentsInChildren<TextMeshProUGUI>())
        {
            i.DOFade(1,3);
            if(_fadeOut) i.DOFade(0,3).SetDelay(3);
        }
    }

    void Update()
    {
        if(stages[5] == null) return;
        tmpStage[0] = (int)stages[0];
        tmpStage[1] = (int)stages[1];
        tmpStage[2] = (int)stages[2];
        tmpStage[3] = (int)stages[3];
        tmpStage[4] = (int)stages[4];
        tmpStage[5] = (int)stages[5];
    }

    // 시작
    public void GeneratePlayer()
    {

        //색
        //특성
        //장비
        //주사위 세팅
    }

    void BackgroundImageChange()
    {
        curBackground.sprite = DataManager.instance.mapImage[System.Array.IndexOf(DataManager.instance.mapName, curMap)];
        curEnemy.DestroyThis();
    }

    public void GenerateStage()
    {
        for(int i = 0; i<6; i++)
        {
            stages[i] = null;
        }

        
        if(curLevel != 0) //0은 게임 시작할 때 정해짐
        {
            //*tmp  
            //curLevel = 0;
            curMap = DataManager.instance.mapLevel[curLevel].Split(',')[Random.Range(0,2)];

            DOTween.Kill(fade);
            fade.gameObject.SetActive(true);
            fade.DOFade(1,2).OnComplete(()=>BackgroundImageChange()); //온컴플리트 두 개 달기? 안 됨
            fade.DOFade(0,2).SetDelay(2.5f).OnComplete(()=>fade.gameObject.SetActive(false));
        }

        curMapType = Random.Range(0,3);

        map.MapNameChange();


        //*tmp

       // curMapType = 1;

        //

        var bossNumber = Random.Range(0,6);
        stages[bossNumber] = SEvent.B;

        var tradeNumber = Random.Range(0,6);
        while(tradeNumber == bossNumber) tradeNumber = Random.Range(0,6);
        stages[tradeNumber] = SEvent.T;

        
        var enemyNumber = Random.Range(0,6);
        while(enemyNumber == bossNumber || enemyNumber == tradeNumber) enemyNumber = Random.Range(0,6);
        stages[enemyNumber] = SEvent.E;

        if(Random.Range(0,60) == 0)
        {
            var hiddenNumber = Random.Range(0,6);
            while(hiddenNumber == bossNumber || hiddenNumber == tradeNumber || hiddenNumber == enemyNumber) hiddenNumber = Random.Range(0,6);
            stages[hiddenNumber] = SEvent.H;
        }

        //        

        var tmpPos = new float[]{Random.Range(0,4),Random.Range(0,4),Random.Range(-10,10f),Random.Range(-10,10f)};

        map.box[4].GetComponent<RectTransform>().anchoredPosition =
        new Vector2(map.box[(int)tmpPos[0]].GetComponent<RectTransform>().anchoredPosition.x+ tmpPos[2], map.box[4].GetComponent<RectTransform>().anchoredPosition.y); 

        map.dotLine[3].GetComponent<RectTransform>().anchoredPosition =
        new Vector2(map.box[(int)tmpPos[0]].GetComponent<RectTransform>().anchoredPosition.x+ tmpPos[2], map.dotLine[3].GetComponent<RectTransform>().anchoredPosition.y); 

        map.box[5].GetComponent<RectTransform>().anchoredPosition =
        new Vector2(map.box[(int)tmpPos[1]].GetComponent<RectTransform>().anchoredPosition.x+ tmpPos[3], map.box[5].GetComponent<RectTransform>().anchoredPosition.y);

        map.dotLine[4].GetComponent<RectTransform>().anchoredPosition =
        new Vector2(map.box[(int)tmpPos[1]].GetComponent<RectTransform>().anchoredPosition.x+ tmpPos[3], map.dotLine[4].GetComponent<RectTransform>().anchoredPosition.y);

        var decoPos = new float[]{Random.Range(-380,600f),Random.Range(-300,600f)};

        decoPos[1] = tmpPos[0] > 1 ? Random.Range(-380,80f) : Random.Range(180,600f);
        decoPos[0] = tmpPos[1] > 1 ? Random.Range(-380,80f) : Random.Range(180,600f);

        //

        map.decoImage[0].GetComponent<RectTransform>().anchoredPosition =
        new Vector2(decoPos[0],Random.Range(110f,190));
        map.decoImage[1].GetComponent<RectTransform>().anchoredPosition =
        new Vector2(decoPos[1],Random.Range(-280f,-200));
        map.decoImage[2].GetComponent<RectTransform>().anchoredPosition = 
        tmpPos[1] > 1 ? new Vector2(655,303) : new Vector2(-440,300);

        //

        for(int i = 0; i<6; i++)
        {
            map.box[i].GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0,Random.Range(-5,5f));

            if(stages[i] == null) stages[i] = (SEvent)Random.Range(0,2);
            
            map.iconImages[i].sprite = map.icons[(int)stages[i]];
            
            map.box[i].GetComponent<RectTransform>().anchoredPosition =
            new Vector2(map.box[i].GetComponent<RectTransform>().anchoredPosition.x, map.box[i].GetComponent<RectTransform>().anchoredPosition.y);
        }
    
        var mapCode = System.Array.IndexOf(DataManager.instance.mapName, curMap);
        map.decoImage[0].sprite = map.deco[mapCode];
        map.decoImage[0].SetNativeSize();
        map.decoImage[1].sprite = map.deco1[mapCode];
        map.decoImage[1].SetNativeSize();
        map.decoImage[2].sprite = map.deco[mapCode];
        map.decoImage[2].SetNativeSize();

        //
        for(int i = 0; i<tradeTable.costs.Length; i++)
        {
            tradeTable.costs[i] = 0;
        }

        foreach(RewardDice i in tradeTable.rewardDices)
        {
            i.soldOut = false;
        }

        //



    }

    public void SelectStage(int _number)
    {
        if(map.isMoving) return;

        if(stages[_number] == SEvent.X) return;

        if(map.on) map.MapButton();
        if(skillTable.on) skillTable.SkillTableButton();
        homeButton.DOAnchorPosX(-1280,1);
        eventRoll.DOAnchorPosY(-625,2);

        homePanel.GetComponent<HomeTableButton>().Back();

        fade.gameObject.SetActive(true);
        fade.DOFade(1,1.5f).OnComplete(()=>SelectingSetting(_number));
        fade.DOFade(0,2f).SetDelay(2.5f).OnComplete(()=>fade.gameObject.SetActive(false));

        curStage = _number;
    }

    void SelectingSetting(int _number)
    {
        if(homePanel.activeSelf)
        { 
            homePanel.SetActive(false);
            homeBack.SetActive(false);
        }
        StageSetting(_number);
    }

    void EnemyTextChange()
    {
        if(curEnemy != null)
        {
            enemyNameText.text = Translate.Text(curEnemy.curName);
            enemyStatText.text = $"[{Translate.Text("체력")} : {curEnemy.stat[0]} {Translate.Text("근력")} : {curEnemy.stat[1]} {Translate.Text("지력")} : {curEnemy.stat[2]} {Translate.Text("민첩")} : {curEnemy.stat[3]}]";
        }
    }

    void StageSetting(int _number)
    {
        if(curTrader) Destroy(curTrader);

        eventEntity.gameObject.SetActive(false);
        
        SoundManager.instance.BgSoundPlay((int)stages[_number]);

        curBackground.sprite = DataManager.instance.mapImage[System.Array.IndexOf(DataManager.instance.mapName, curMap)];

        switch(stages[_number])
        {
            case SEvent.B :
                var enemayName = DataManager.instance.mapDes[curMap][2].Split(',')[curMapType];

                var enemyCode = DataManager.instance.enemyName.IndexOf(enemayName);
                
                TextManager.instance.storyText.TextChange(Translate.Text(enemayName)); //*plz 이거 Text.text 로 번역된 언어 주도록 해야할 듯?

                curEnemy = Instantiate(DataManager.instance.enemys[enemyCode],new Vector3(3,-2.5f), Quaternion.identity).GetComponent<Entity>();
                curEnemy.StartSetting(enemyCode);

                curEnemy.curName = enemayName;
                
                BattleSetting();

            break;
            case SEvent.H :

                enemayName = DataManager.instance.mapDes[curMap][3].Split(',')[Random.Range(0,3)];

                //*tmp

                enemyCode = DataManager.instance.enemyName.IndexOf(enemayName);

                TextManager.instance.storyText.TextChange(Translate.Text(enemayName));

                curEnemy = Instantiate(DataManager.instance.enemys[enemyCode],new Vector3(3,-2.5f), Quaternion.identity).GetComponent<Entity>();
                curEnemy.StartSetting(enemyCode);

                curEnemy.curName = enemayName;

                BattleSetting();

            break;
            case SEvent.E :

                enemayName = DataManager.instance.mapDes[curMap][1].Split(',')[curMapType];


                enemyCode = DataManager.instance.enemyName.IndexOf(enemayName);

                //*tmp

                //enemyCode = 20;

                TextManager.instance.storyText.TextChange(Translate.Text(enemayName));

                curEnemy = Instantiate(DataManager.instance.enemys[enemyCode],new Vector3(3,-2.5f), Quaternion.identity).GetComponent<Entity>();
                curEnemy.StartSetting(enemyCode);

                curEnemy.curName = enemayName;

                BattleSetting();
            break;
            
            case SEvent.Q :

                eventEntity.gameObject.SetActive(true);
                eventEntity.StartSetting(Random.Range(0,3));

                //map.MapOut();
                eventRoll.DOAnchorPosY(-400,2).SetEase(Ease.Unset);

            break;
            
            case SEvent.T :

                var traderCode = System.Array.IndexOf(DataManager.instance.traderName,"PuckTrader");
                curTrader = Instantiate(DataManager.instance.traders[traderCode],new Vector3(1,-2.5f), Quaternion.identity);

                SettingTrade("PuckTrader");

                map.MapOut();
                tradeTable.rect.DOAnchorPos(new Vector2(0,0),2);


            break;
        }
    }

    void BattleSetting()
    {
            EnemyTextChange();


                statBar.curEnemy = curEnemy;
                
                Transform playerCenter = curPlayer.transform, enemyCenter = curEnemy.transform; 

                foreach(Transform i in curPlayer.GetComponentsInChildren<Transform>())
                {
                    if(i.name == "hitPoint") playerCenter = i;
                }
                foreach(Transform i in curEnemy.GetComponentsInChildren<Transform>())
                {
                    if(i.name == "hitPoint") enemyCenter = i;
                }


                playerParticle.transform.position = playerCenter.position;//curPlayer.transform.position;
                enemyParticle.transform.position = enemyCenter.position;//curEnemy.transform.position;

                //배틀세팅을 만들까
                curPlayer.onBattle = true;
                curPlayer.curHp = curPlayer.maxHp;
                curEnemy.onBattle = true;

                map.MapOut();
                enemyTable.DOAnchorPos(new Vector2(1180,35), 2);
                SoundManager.instance.UISfxPlay(2);

                playerBar.DOAnchorPos(new Vector2(-515, -455), 2);
                enemyBar.DOAnchorPos(new Vector2(515, -455), 2);
                
                battleStopRect.DOAnchorPosY(-430,1);

                BattleSpdSetting(curEnemy,curPlayer);
    }

    public void StopBattle()
    {
        // 0 : > 1 : ll
        battleStopImage.image.sprite = battleStopImage.images[battleStopImage.image.sprite == battleStopImage.images[0] ? 1 : 0];

        if(battleStopImage.image.sprite == battleStopImage.images[1])
        {
            battleStop = false;
            reserveStop = false;
            return;
        }
        if(!curPlayer.isReady && !curEnemy.isReady) battleStop = true;
        else reserveStop = true;

        /*
        if(curPlayer.isReady || curEnemy.isReady)
        {
            reserveStop = reserveStop ? false : true;
        }
        else
        {
            battleStop = battleStop ? false : true;
        }
        */
    }

    public void BattleSpdSetting(Entity _enemy, Entity _player)
    {
        var standardSpd = 10;

        if(_enemy.stat[3] == _player.stat[3])
        {
            _player.battleSpd = standardSpd; 
            _enemy.battleSpd = standardSpd;

            return;
        }
        else
        {
            int enemySpd = _enemy.stat[3], playerSpd = _player.stat[3];

            var gap = Mathf.Lerp(enemySpd, playerSpd, 0.5f);

            //인트값이라 그냥 갭이 정중앙이 아닐 수 도 있어서 이렇게 따로
            
                _player.battleSpd = (standardSpd + (playerSpd - (int)gap)) < 1 ? 1 : (standardSpd + (playerSpd - (int)gap)) ;
                _enemy.battleSpd = (standardSpd + (enemySpd - (int)gap)) < 1 ? 1 : (standardSpd + (enemySpd - (int)gap)) ;

        }

    }

    public void FinishBattle(Entity _loser)
    {
        map.MapIn();
        
        playerBar.DOAnchorPos(new Vector2(-1500, -455), 2);
        enemyBar.DOAnchorPos(new Vector2(1500, -455), 2);
        
        battleStopRect.DOAnchorPosY(-810,1);

        enemyTable.DOAnchorPos(new Vector2(1500,0), 2);
        SoundManager.instance.UISfxPlay(2);

        curPlayer.onBattle = false;
        curEnemy.onBattle = false;
        
        battleStopImage.image.sprite = battleStopImage.images[1];

        battleStop = false;
        reserveStop = false;
            
        
        //

        //보스는 전리품 상자를 주는 거로

        //

        Debug.Log("a");

        FinishStage();


    }

    public void FinishStage()
    {
        if(curPlayer.died) return;
        homeButton.DOAnchorPosX(-870,1);
        if(stages[curStage] == SEvent.B)
        { 
            //map level길이 보고 대충
            if(curLevel == 1)
            {
                ClearGame();
                return;
            }
            curLevel++;
            GenerateStage();
            return;
        }
        stages[curStage] = SEvent.X;
        map.iconImages[curStage].sprite = map.icons[(int)SEvent.X];
    }

    public void GetAbility(string _ability)
    {
        AbilityPopUp tmpAbility = null;

        foreach(AbilityPopUp i in abilityImages)
        {
            if(!i.gameObject.activeSelf)
            {
                tmpAbility = i;
                i.gameObject.SetActive(true);
                break;
            }
        }

        if(tmpAbility != null)
        {
            curPlayer.abilityList.Add(_ability);

            tmpAbility.n = curPlayer.abilityList.Count-1;
            tmpAbility.image.sprite = DataManager.instance.abliltyImages[DataManager.instance.abliltytName.IndexOf(_ability)];
            tmpAbility.rect.anchoredPosition = new Vector2(-630 + (curPlayer.abilityList.Count - 1 )*130, 480);

            if(tmpAbility.rect.anchoredPosition.x > 900)
            {
                tmpAbility.rect.anchoredPosition = new Vector2(-630 + (curPlayer.abilityList.Count - 13 )*130, 350);
            }

            if(_ability == "고블린 귀") curPlayer.StatsChange(2,3);
            if(_ability == "커다란 몽둥이") curPlayer.StatsChange(1,4);

        }

        //tmpAbility.GetComponent<Image>().sprite = DataManager.instance.abliltyImages[DataManager.instance.abliltytName.IndexOf(_ability)];
        //tmpAbility.GetComponent<RectTransform>().anchoredPosition = new Vector2(-630 + (curPlayer.abilityList.Count - 1 )*130, 480);

        //tmpAbliltyList.Add(i);
    }


    // SkillDice Setting
    public void GetDice(string _name)
    {
        SoundManager.instance.DiceSfxPlay();

        //var n = skillList.Count;

        var n = 0;

        for(int i = 0; i<skillDices.Length; i++)
        {
            if(i == 40 && skillDices[i].gameObject.activeSelf) return;

            if(!skillDices[i].gameObject.activeSelf)
            {
                n = i;
                break;
            }
        }

        skillList.Add(_name);

        skillDices[n].gameObject.SetActive(true);

        //skillDices[n].rect.anchoredPosition = new Vector3(2500,0);

        skillDices[n].GetComponent<Image>().sprite = DataManager.instance.skillDic[_name];
        skillDices[n].GetComponent<SkillDice>().skillName = _name;
        skillDices[n].rect.DOAnchorPos(new Vector2(Random.Range(1000,1800),Random.Range(-300,300)),Random.Range(1,2)).SetEase(Ease.Unset);
        skillDices[n].rect.DOAnchorPos(new Vector2(0,0),Random.Range(1,2)).SetEase(Ease.Unset).SetDelay(2);

        //skillDices[n].rect.rotation = Quaternion.Euler(new Vector3(0,0,Random.Range(0,360)));
    }

    public int GetDice2(string _name)
    {
        SoundManager.instance.DiceSfxPlay();

        //var n = skillList.Count;

        var n = 0;

        for(int i = 0; i<skillDices.Length; i++)
        {
            if(i == 40 && skillDices[i].gameObject.activeSelf) return n;

            if(!skillDices[i].gameObject.activeSelf)
            {
                n = i;
                break;
            }
        }

        skillList.Add(_name);

        skillDices[n].gameObject.SetActive(true);

        skillDices[n].GetComponent<Image>().sprite = DataManager.instance.skillDic[_name];
        skillDices[n].GetComponent<SkillDice>().skillName = _name;

        return n;
    }

    public void GetStatDice(int _value)
    {
        SoundManager.instance.DiceSfxPlay();

        StatDice tmpStatDice = null; 

        foreach(StatDice i in statDices)
        {
            if(!i.gameObject.activeSelf)
            {
                tmpStatDice = i;
                break;
            }
        }

        if(tmpStatDice == null) return;

        tmpStatDice.gameObject.SetActive(true);
        tmpStatDice.eyeImage.SetNativeSize();
        tmpStatDice.EyeSetting(_value);
        
        tmpStatDice.rect.DOAnchorPos(new Vector2(Random.Range(1000,1800),Random.Range(-300,300)),Random.Range(1,2)).SetEase(Ease.Unset);
        tmpStatDice.rect.DOAnchorPos(new Vector2(0,0),Random.Range(1,2)).SetEase(Ease.Unset).SetDelay(2);

    }

    public void GetExpDice(int _value)
    {
        
        ExpDice tmpExpDice = null; 

        foreach(ExpDice i in expDices)
        {
            if(!i.gameObject.activeSelf)
            {
                tmpExpDice = i;
                break;
            }
        }

        if(tmpExpDice == null) return;

        SoundManager.instance.DiceSfxPlay();

        tmpExpDice.gameObject.SetActive(true);
        tmpExpDice.EyeSetting(_value);
        
        tmpExpDice.rect.DOAnchorPos(new Vector2(Random.Range(-880,880),Random.Range(-420,420)),Random.Range(1,2)).SetEase(Ease.Unset);
        tmpExpDice.rect.DORotate(new Vector3(0,0,Random.Range(0,360)),Random.Range(0f,1f)).SetEase(Ease.InSine);
    }

    public void SettingTrade(string _trader)
    {
        var traderSplit = DataManager.instance.traderItems[System.Array.IndexOf(DataManager.instance.traderName, _trader)].Split(',');

        tradeTable.rect.DOAnchorPos(new Vector2(0,0), 2);

        tradeTable.rewardDices[0].RewardSetting(traderSplit[0]);
        tradeTable.rewardDices[1].RewardSetting(traderSplit[1]);
        tradeTable.rewardDices[2].RewardSetting(traderSplit[2]);
    }

    public void FinishTrade()
    {
        FinishStage();
        map.MapIn();
        tradeTable.rect.DOAnchorPos(new Vector2(800,0), 2);
    }

    //

    void CreateEnemy()
    {
        
    }

    public void DefeatGame()
    {
        DOTween.Kill(fade);

        fade.gameObject.SetActive(true);
        fade.DOFade(0.5f,1);

        var humanCamScreenX = humanCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX;
        //humanCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.6f;
        //humanCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.65f;
        DOTween.To(()=>humanCam.m_Lens.OrthographicSize,x=>humanCam.m_Lens.OrthographicSize=x,2,5);
        
        DOTween.To(()=>humanCamComposer.m_ScreenX,x=>humanCamComposer.m_ScreenX=x,0.77f,5);
        DOTween.To(()=>humanCamComposer.m_ScreenY,x=>humanCamComposer.m_ScreenY=x,0.7f,5);

        //DOTween.To(()=>humanCamScreenX,x=>humanCamScreenX=x,0.6f,3);

        playerBar.DOAnchorPos(new Vector2(-1500, -455), 2);
        enemyBar.DOAnchorPos(new Vector2(1500, -455), 2);
        battleStopRect.DOAnchorPosY(-810,1);
        
        eventRoll.DOAnchorPosY(-625,2).SetEase(Ease.Unset);

        FinishTrade();

        map.MapOut();
        homeButton.DOAnchorPosX(-1280,1);

        //homePanel.GetComponent<HomeTableButton>().Back();
        homePanel.gameObject.SetActive(false);
        homeBack.gameObject.SetActive(false);

        startPanel.SetActive(false);

        enemyTable.DOAnchorPos(new Vector2(1500,0), 2);
        skillTable.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-1500,0), 2);
        FadeInUI(defeatPanel,false);

        FinishGame();
    }
    public void ClearGame()
    {
        //finishgame

        //클리어 이미지 치우면 똑같이
        
        DOTween.Kill(fade);
        DOTween.Kill(victory);

        fade.gameObject.SetActive(true);
        fade.DOFade(0.5f,1);

        victory.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,300);
        victory.gameObject.SetActive(false);

        var humanCamScreenX = humanCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX;
        //humanCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.6f;
        //humanCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.65f;
        DOTween.To(()=>humanCam.m_Lens.OrthographicSize,x=>humanCam.m_Lens.OrthographicSize=x,2,5.5f).OnComplete(()=>FinishGame());;
        
        DOTween.To(()=>humanCamComposer.m_ScreenX,x=>humanCamComposer.m_ScreenX=x,0.65f,5).OnComplete(()=>FadeInUI(victory.gameObject,false));
        DOTween.To(()=>humanCamComposer.m_ScreenY,x=>humanCamComposer.m_ScreenY=x,0.7f,5);

        playerBar.DOAnchorPos(new Vector2(-1500, -455), 2);
        enemyBar.DOAnchorPos(new Vector2(1500, -455), 2);
        
        battleStopRect.DOAnchorPosY(-810,1);

        enemyTable.DOAnchorPos(new Vector2(1500,0), 2);
        skillTable.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-1500,0), 2);

        map.MapOut();
        homeButton.DOAnchorPosX(-1280,1);

        TextManager.instance.achieveText.AchieveTextChange("평화로운 세상");
        

    }

    void FinishGame()
    {
        /*
         경험치 바 생성
         경험치 다이스 생성 : 클리어 한 지역 다이스 획득

         계속 버튼 누르면 남아있는 다이스 모두 획득
        */
        expBar.SetActive(true);

        foreach(int i in expGain)
        {
            for(int j = 0; j<i; j++)
            {
                GetExpDice(i-1);
            }
        }

        foreach(Image i in expBar.GetComponentsInChildren<Image>())
        {
            var tmpColor = i.color;
            tmpColor.a = 0f;
            i.color = tmpColor;
        }
        FadeInUI(expBar,false);
        FadeInUI(nextButton,false);
    }

    public void ForgiveGame()
    {
        SettingOnOff();
        curPlayer.OnDie(true);
    }

    public void GotoStartSceneReady()
    {
        //if(defeatPanel.GetComponent<Image>().color.a < 1) return;
        //FadeInUI(defeatPanel,false);

        finalFade.gameObject.SetActive(true);
        finalFade.DOFade(1,1.5f).OnComplete(()=>GotoStartScene());

        foreach(ExpDice i in expDices)
        {
            if(i.gameObject.activeSelf) i.Use();
        }

        //transform.DOMoveX(0,3).OnComplete(()=>GotoStartScene());
    }

    public void GotoStartScene()
    {  
        DOTween.KillAll();
        SceneManager.LoadScene("StartScene");
    }

    public void SettingOnOff()
    {
        SoundManager.instance.UISfxPlay(10);
        settingObject.SetActive(!settingObject.activeSelf);
    }

    public void GotoHome()
    {
        if(map.isMoving || skillTable.isMoving) return;
        fade.gameObject.SetActive(true);
        if(map.on) map.MapButton();
        if(skillTable.on) skillTable.SkillTableButton();
        homeButton.DOAnchorPosX(-1280,1);
        eventRoll.DOAnchorPosY(-625,2);

        fade.DOFade(1,1.5f).OnComplete(()=>GotoHomeSetting());
        
        fade.DOFade(0,1.5f).SetDelay(1.5f).OnComplete(()=>fade.gameObject.SetActive(false));
    }
    void GotoHomeSetting()
    {  
        homePanel.SetActive(true);
        homeBack.SetActive(true);
    }


    public IEnumerator CamShake(float intense = 1)
    {
        humanCamShake.m_AmplitudeGain = intense;
        humanCamShake.m_FrequencyGain = 5;

        yield return new WaitForSeconds(0.1f);

        humanCamShake.m_AmplitudeGain = intense/2;
        humanCamShake.m_FrequencyGain = 5;
        
        yield return new WaitForSeconds(0.1f);

        
        humanCamShake.m_AmplitudeGain = 0;
        humanCamShake.m_FrequencyGain = 0;

    }
}

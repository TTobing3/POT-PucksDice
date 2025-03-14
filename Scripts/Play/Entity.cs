using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Linq;


public class Entity : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] statTexts;


    public Entity entity;
    public string curName;

    //
    public int[] stat = new int[4]{0,0,0,0}; //hp, str, spr, spd;
    int[] plusStat = new int[4]{0,0,0,0};
    public float maxHp{ get{ return stat[0] * 10f; } }
    public float curHp = 0;

    //extra status
    
    public bool isReady = false, isActing = false, onBattle = false, isPlayer, died, skillMaintain;
    public float rp = 50, maxRp = 100, battleSpd = 0; // ready point
    
    //

    public string[] skills = new string[6];
    public string curSkill;
    public List<string> curSkillEft = new List<string>();
    
    public List<string> curSkillEftDetail = new List<string>();

    Dictionary<string,int> statString = new Dictionary<string, int>();

    //

    public List<string> abilityList = new List<string>();

    //
    SpriteRenderer spriteRenderer;
    public Animator animator, eye;
    public SpriteRenderer dice, eyeRenderer;
    public Vector3 dicePosition;
    public Sprite[] eyes;
    
    //

    public int job;
    Color color;
    string curDice;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        entity = GetComponent<Entity>();

        statString.Add("체력",0);
        statString.Add("근력",1);
        statString.Add("지력",2);
        statString.Add("민첩",3);

        dicePosition = dice.transform.position;
    }

    void Start()
    {
        ColorChange(new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1));
    }

    void Update()
    {
        if(died) return;

        if(!isReady && rp < maxRp && onBattle && !GameManager.instance.battleStop)
        {
            rp += (battleSpd + plusStat[3]) * 3 * Time.deltaTime;
        }
        else if(!isReady && rp >= maxRp)
        {
            RollDice();
        }
    }

    public void StartSetting(int e)
    {
        //if(isPlayer) return;

        //doll

        //지향 스텟

        //지향 스킬

        JobChange(DataManager.instance.enemyName[e]); // 그 유동적으로 하려면 어   떻    게 그 구분

        var armour = DataManager.instance.enemyArmour[ e ].Split(',');
        for(int i = 0; i<armour.Length; i++)
        {
            armour[i] = armour[i].Split('/')[Random.Range(0,armour[i].Split('/').Length)];
        }

        foreach(SpriteResolver i in GetComponentsInChildren<SpriteResolver>())
        {
            switch(i.GetCategory())
            {
                case "WeaponIn" :
                i.SetCategoryAndLabel("WeaponIn",armour[0]);
                break;
                case "Helmet" :
                i.SetCategoryAndLabel("Helmet",armour[1]);
                break;
                case "Armour" :
                i.SetCategoryAndLabel("Armour",armour[2]);
                break;
                case "ShoulderArmour" :
                i.SetCategoryAndLabel("ShoulderArmour",armour[2]);
                break;
            }
        }

        var statRange = DataManager.instance.enemyStats[e].Split(',');

        for(int i = 0; i<4; i++)
        {
            var statMinMax = statRange[i].Split('~');
            stat[i] = Random.Range(int.Parse(statMinMax[0]),int.Parse(statMinMax[1])+1);
        }

        var skillList = DataManager.instance.enemySkills[e].Split(',');
        for(int i = 0; i<6; i++)
        {
            GainSkill(i, skillList[Random.Range(0,skillList.Length)]);
        }
        curHp = maxHp;

        //

        if(DataManager.instance.enemyName[e].Contains("큰"))
        {
            transform.localScale *= 1.2f;
            dicePosition = dice.transform.position;
        }
    }

    public void GainSkill(int _n, string _skill)
    {
        skills[_n] = _skill;
        GameManager.instance.enemySkills[_n] = _skill;
        GameManager.instance.enemySkillList[_n].sprite = DataManager.instance.skillDic[_skill];
    }

    public void DiceChange(string _diceName)
    {
        curDice = _diceName;
        dice.GetComponent<SpriteResolver>().SetCategoryAndLabel("DiceBase",_diceName);
    }

    void ColorChange(Color _color)
    {
        foreach(SpriteRenderer i in GetComponentsInChildren<SpriteRenderer>())
        {
            i.material.SetColor("_G",_color);
        }
    }

    public void StatsChange(int _stat, int _power)
    {
        stat[_stat] += _power;

        if(_stat == 0)
        {
            curHp += _power * 10;
        }
        else if(_stat == 3 && GameManager.instance.curEnemy != null)
        {
            GameManager.instance.BattleSpdSetting(GameManager.instance.curEnemy,this);
        }

        statTexts[0].text = ": "+stat[0];
        statTexts[1].text = ": "+stat[1];
        statTexts[2].text = ": "+stat[2];
        statTexts[3].text = ": "+stat[3];
    }

    public void JobChange(string _job)
    {
        job = System.Array.IndexOf(DataManager.instance.jobs, _job);

        if(job == -1)
        {
            animator.SetInteger("job",0);
        }
        else
        {
            animator.SetInteger("job",job);
        }

        animator.SetTrigger("idle");

    }

    //

    public void RollDice()
    {
        if(died || isReady) return;
        
        isReady = true;

        var d = Random.Range(0,6);

        SoundManager.instance.DiceSfxPlay();

        eye.SetInteger("P",Random.Range(1,4));
        eye.SetTrigger("roll");
        dice.transform.DORotate(new Vector3(0,0,360),1.5f,RotateMode.FastBeyond360).SetEase(Ease.Unset);
        DOTween.Shake(() => dice.transform.position, x => dice.transform.position = x, 1.5f, 0.5f, 20, 90, true, true).OnComplete(()=>ActSkill(d));
    }

    public void ActSkill(int _number)
    {
        if(died || isActing) return;
        
        var target = isPlayer ? GameManager.instance.curEnemy : GameManager.instance.curPlayer;

        if(target.GetComponent<Entity>().died) return;

        rp = 0;
        maxRp = 100;
        skillMaintain = false;
        isActing = true;

        dice.transform.DOMove(dicePosition, 1);
        eye.SetTrigger("stop");
        eye.SetInteger("D",_number);

        curSkillEft = new List<string>();
        curSkillEftDetail = new List<string>();

        curSkill = skills[_number];

        if(skills[_number] == "" || TextManager.instance.skillInfoList[curSkill][1] == "없음")
        {
            FinishSkill();
            return;
        }

        if(abilityList.Contains("장물") && Random.Range(0,10) < 3) StartCoroutine(DotDamage(0f,stat[2]  * 1,0f,"독"));
            
        var tmpSkillEft = DataManager.instance.skillEft[curSkill].Split('/');

        foreach(string i in tmpSkillEft)
        {
            PrepareEffect(i);
            if(TextManager.instance.skillInfoList[curSkill][1] == "방어") 
            {
                skillMaintain = true;
                if(i.Split(':')[0] == "준비") rp += float.Parse(i.Split(':')[1]);
            }
            
                
        }

        animator.SetTrigger(TextManager.instance.skillInfoList[curSkill][4]);

    }

    public void PrepareEffect(string _skillEft)
    {
        if(_skillEft == "없음") return;

        var tmpSkillDes = _skillEft.Split(':');
        var tmpSkillEft = tmpSkillDes[1].Split(',');

        curSkillEft.Add(tmpSkillDes[0]);
        curSkillEftDetail.Add(tmpSkillDes[1]);

    }

    public IEnumerator DotDamage(float _strDamage, float _sprDamage, float _fixDamage, string _type)
    {
        yield return new WaitForSeconds(1f);

        if(!isPlayer) GameManager.instance.enemyParticle.Play(_type);
        else GameManager.instance.playerParticle.Play(_type);

        OnHit(_strDamage,_sprDamage,_fixDamage);

        if(_strDamage/2>=1 || _sprDamage/2>=1 )
        {
            StartCoroutine(DotDamage(_strDamage/2,_sprDamage/2,_fixDamage/2,_type));
        }
        else
        {
            yield break;
        }

    }

    public void Shock()
    {
        if(died || !onBattle || curHp <= 0) return;
        if(rp >= maxRp) 
        {
            maxRp = 100;
            rp = 0;
        }
        
        eye.SetTrigger("stop");

        isActing = true;
        skillMaintain = false;
        animator.SetTrigger("hit");
        TextManager.instance.Eft("충격",!isPlayer,transform);
    }

    public IEnumerator Pain(float _power,int _type, int _stack)
    {
        yield return new WaitForSeconds(2f);
        
        if(curHp <= 0) yield break;
        
        Shock();

        switch(_type) // 0 물리 1 마법 2 고정
        {
            case 0 : OnHit(_power/5,0,0);
            break;
            case 1 : OnHit(0,_power/5,0);
            break;
            case 2 : OnHit(0,0,_power/5);
            break;
        }
        
        if( _stack < 5 )
        {
            StartCoroutine(Pain(_power,_type,_stack+1));
        }
        else
        {
            yield break;
        }
    }

    public void AttackTarget()
    {
        var target = isPlayer ? GameManager.instance.curEnemy : GameManager.instance.curPlayer;

        if(abilityList.Contains("피 묻은 붕대")) StartCoroutine(DotDamage(stat[1]  * 1,0f,0f,"출혈"));
        if(abilityList.Contains("불발탄") && Random.Range(0,5) == 4)
        {
            Shock();
            return;
        }

        if(target.curSkillEft.Contains("무시"))
        {
            var tmpSkillDetail = target.curSkillEftDetail[ target.curSkillEft.IndexOf("무시") ].Split(',');

            if(Random.Range(0,100) < ( target.stat[statString[tmpSkillDetail[0]]] * float.Parse(tmpSkillDetail[1]) / stat[statString[tmpSkillDetail[0]]] ) * 100 )
            {
                TextManager.instance.Eft(("무시"),!isPlayer,target.transform);
                return;
            }
        }
        

        for(int i = 0; i<curSkillEft.Count; i++)
        {
            var tmpSkillDetail = curSkillEftDetail[i].Split(',');

            switch(curSkillEft[i])
            {
                case "회복" :
                var tmpHeal = stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]);
                curHp += tmpHeal;
                TextManager.instance.Dmg(Mathf.Round(tmpHeal*100)/100, transform, tmpHeal/(float)maxHp, !isPlayer,1,true);
                TextManager.instance.Eft(curSkillEft[i],!isPlayer,transform);
                break;
                case "고정회복" :
                tmpHeal = float.Parse(tmpSkillDetail[0]);
                curHp += tmpHeal;
                TextManager.instance.Dmg(Mathf.Round(tmpHeal*100)/100, transform, tmpHeal/(float)maxHp, !isPlayer,1, true);
                TextManager.instance.Eft(curSkillEft[i],!isPlayer,transform);
                break;
                case "물리피해" :

                float tmpStrDamage;

                if(tmpSkillDetail.ToList().Contains("장착") && tmpSkillDetail[2] == "장착")
                {
                    var tmpCount = skills.ToList().Count(x => x == tmpSkillDetail[3]);
                    tmpStrDamage = stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]) * tmpCount;
                }
                else
                {
                    tmpStrDamage = stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]);
                }

                
                if(abilityList.Contains("가죽 두건")) rp += 20;
                if(abilityList.Contains("보석 반지") && TextManager.instance.skillInfoList[curSkill][4] == "punch" && Random.Range(0,10) == 0) tmpStrDamage *= 2;
                if(abilityList.Contains("술통"))
                {
                    if(Random.Range(0,2) == 0)
                    {
                        Shock();
                        return;
                    }
                    else
                    {
                        tmpStrDamage *= 2;
                    }
                }

                target.OnHit(tmpStrDamage,0,0);
                break;
                case "마법피해" :
                var tmpSprDamage = stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]);
                
                if(abilityList.Contains("술통"))
                {
                    if(Random.Range(0,2) == 0)
                    {
                        Shock();
                        return;
                    }
                    else
                    {
                        tmpSprDamage *= 2;
                    }
                }

                target.OnHit(0,tmpSprDamage,0);
                break;
                case "고정피해" :
                target.OnHit(0,0,float.Parse(tmpSkillDetail[0]));
                break;
                case "자기피해" :
                OnHit(0,0,stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]));
                //if(curHp > stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]))
                //{
                //    OnHit(0,0,stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]));
                //}
                break;
                case "출혈" :
                TextManager.instance.Eft(curSkillEft[i],!isPlayer,target.transform);
                StartCoroutine(target.DotDamage(stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]),0f,0f,"출혈"));
                break;
                case "독" :
                TextManager.instance.Eft(curSkillEft[i],!isPlayer,target.transform);
                StartCoroutine(target.DotDamage(0f,stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]),0f,"독"));
                break;
                case "고통" : // 마법일 때 키워드랑, 고정피해일 때 키워드 또 만들기
                TextManager.instance.Eft(curSkillEft[i],!isPlayer,target.transform);
                StartCoroutine(target.Pain(stat[statString[tmpSkillDetail[0]]]  * float.Parse(tmpSkillDetail[1]),1,0));
                break;
                case "둔화" :
                target.maxRp += float.Parse(tmpSkillDetail[0]);
                TextManager.instance.Eft(curSkillEft[i],!isPlayer,target.transform);
                break;
                case "충격" :
                target.Shock();
                break;
                case "지침" :
                TextManager.instance.Eft(curSkillEft[i],!isPlayer,target.transform);
                if(target.rp > float.Parse(tmpSkillDetail[0]))
                {
                    target.rp -= float.Parse(tmpSkillDetail[0]);
                }
                else
                {
                    target.rp = 0;
                }
                break;
                case "준비" :
                    rp += int.Parse(tmpSkillDetail[0]);
                    TextManager.instance.Eft(curSkillEft[i],!isPlayer,transform);
                break;
                case "신속" :
                    TextManager.instance.Eft(curSkillEft[i],!isPlayer,transform);
                    if(maxRp > int.Parse(tmpSkillDetail[0])) maxRp -= int.Parse(tmpSkillDetail[0]);
                    else maxRp = 0;
                break;
            }
            
        }

        //이펙트는?

        // 사운드

        SoundManager.instance.BattleSfxPlay(System.Array.IndexOf(DataManager.instance.effectSoundName, TextManager.instance.skillInfoList[curSkill][6]));

        if(isPlayer) GameManager.instance.enemyParticle.Play(TextManager.instance.skillInfoList[curSkill][5]);
        else GameManager.instance.playerParticle.Play(TextManager.instance.skillInfoList[curSkill][5]);
    }

    public void OnDie(bool isForgive = false)
    {
        if(!isForgive)
        {
            if(died || !onBattle) return;
        }
        died = true;
        animator.SetTrigger("die");

        DOTween.Kill(dice.transform);

        dice.transform.DOMoveY(transform.position.y+3,0.5f).SetEase(Ease.Unset);
        dice.transform.DOMoveY(transform.position.y-10,0.5f).SetEase(Ease.InQuad).SetDelay(0.75f);
        dice.transform.DORotate(new Vector3(0,0,Random.Range(0,360)),2);


        if(isPlayer) 
        {
            GameManager.instance.DefeatGame();
            //GameManager.instance.FadeInUI(GameManager.instance.fade.gameObject, false);
            GameManager.instance.FadeInUI(GameManager.instance.defeat.gameObject, false);
        }
        else 
        {
            var tmpFirst = true;

            foreach(SpriteRenderer i in GetComponentsInChildren<SpriteRenderer>())
            {
                if(tmpFirst && !isPlayer)
                {
                    i.DOFade(0,3).SetDelay(5).OnComplete(() => DestroyThis());
                }
                else
                {
                    i.DOFade(0,3).SetDelay(5);
                }
                tmpFirst = false;
            }
            
            if(abilityList.Contains("앙크")) StatsChange(0,3);

            if(GameManager.instance.stages[GameManager.instance.curStage] == SEvent.B)
            {
                GameManager.instance.expGain[GameManager.instance.curLevel] = +1;
                GameManager.instance.FadeInUI(GameManager.instance.victory.gameObject, true);
            }
            else
            {
                GameManager.instance.FadeInUI(GameManager.instance.victory.gameObject, true);
                GameManager.instance.GetDice(skills[Random.Range(0,6)]);
                GameManager.instance.GetStatDice(Random.Range(0,6));
            }
            
            GameManager.instance.FinishBattle(entity);
        }
    }

    public void OnHit(float _strDamage, float _sprDamage, float _fixDamage)
    {
        if(died) return;

        var target = isPlayer ? GameManager.instance.curEnemy : GameManager.instance.curPlayer;

        foreach(SpriteRenderer i in GetComponentsInChildren<SpriteRenderer>())
        {
            i.color = Color.red;
            i.DOColor(Color.white,0.3f);
        }

        //
        if(abilityList.Contains("마도서") && Random.Range(0,5) > 2)
        {
            TextManager.instance.Eft(("무시"),isPlayer,transform);
            return;
        }

        //

        for(int i = 0; i<curSkillEft.Count; i++)
        {
            var tmpSkillDetail = curSkillEftDetail[i].Split(',');

            switch(curSkillEft[i])
            {
                case "즉시 준비" :
                TextManager.instance.Eft(curSkillEft[i],isPlayer,transform);
                rp += 100;
                break;
                case "무시" :
                //무시 키워드는 AttackTarget에서
                break;
                case "감소" :
                TextManager.instance.Eft(curSkillEft[i],isPlayer,transform);
                if(tmpSkillDetail[0] == "피해량")
                {
                    _strDamage *= float.Parse(tmpSkillDetail[1]);
                    _sprDamage *= float.Parse(tmpSkillDetail[1]);
                    _fixDamage *= float.Parse(tmpSkillDetail[1]);
                }
                else if(tmpSkillDetail[0] == "물리 피해량")
                {
                    _strDamage *= float.Parse(tmpSkillDetail[1]);
                }
                else if(tmpSkillDetail[0] == "마법 피해량")
                {
                    _sprDamage *= float.Parse(tmpSkillDetail[1]);
                }
                break;
            }
        }

        var damage = _strDamage + _sprDamage + _fixDamage;

        
        if(abilityList.Contains("갑옷"))
        {
            //TextManager.instance.Eft(("감소"),isPlayer,transform);
            damage -= stat[0];
        }

        if(damage < 0) damage = 0;

        TextManager.instance.Dmg(Mathf.Round(damage*100)/100, transform, damage/(float)maxHp, isPlayer);

        if(maxHp/3 < damage) StartCoroutine(GameManager.instance.CamShake());
        if(maxHp/1.5f < damage) StartCoroutine(GameManager.instance.CamShake(2f));

        curHp -= damage;
        
        if(curHp < 1)
        {
            OnDie();
        }
    }

    public void FinishSkill()
    {
        var target = isPlayer ? GameManager.instance.curEnemy : GameManager.instance.curPlayer;

        if(!skillMaintain && !died) animator.SetTrigger("idle");
        
        if(GameManager.instance.reserveStop && !target.isReady)
        { 
            GameManager.instance.battleStop = true;
            GameManager.instance.reserveStop = false;
            //GameManager.instance.battleStopImage.image.sprite = GameManager.instance.battleStopImage.images[GameManager.instance.battleStop ? 0 : 1];
        } 

        isReady = false;
        isActing = false;
    }

    public void DestroyThis()
    {
        foreach(SpriteRenderer i in GetComponentsInChildren<SpriteRenderer>())
        {
            DOTween.Kill(i);
        }
        DOTween.Kill(this);

        Destroy(gameObject);
    }

}

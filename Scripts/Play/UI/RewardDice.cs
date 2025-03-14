using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardDice : MonoBehaviour
{
    public int number;

    public string reward;
    
    [SerializeField] Sprite soldOutImage, statDiceImage;

    Image image;

    public bool soldOut;

    void Awake() 
    {
        image = GetComponent<Image>();
        
    }

    public void RewardSetting(string _reward)
    {
        soldOut = false;
        var rewardSplit = _reward.Split('/');

        if(rewardSplit[0] == "Skill")
        {
            image.sprite = DataManager.instance.skillImages[ System.Array.IndexOf(DataManager.instance.skillNames, rewardSplit[1]) ];
        }
        if(rewardSplit[0] == "Stat")
        {
            image.sprite = statDiceImage;
        }

        reward = _reward;

    }

    public void BuyIt()
    {
       if(!GameManager.instance.tradeTable.CostCheck(number) || soldOut) return;

        //cost check

        image.sprite = soldOutImage;
        

        soldOut = true;

        //image change

        //get dice

        var rewardSplit = reward.Split('/');

        if(rewardSplit[0] == "Skill")
        {
            GameManager.instance.GetDice(rewardSplit[1]);
        }
        if(rewardSplit[0] == "Stat")
        {
            if(rewardSplit[1] == "-1")
            {
                GameManager.instance.GetStatDice(Random.Range(0,6));
            }
            else
            {
                GameManager.instance.GetStatDice(int.Parse(rewardSplit[1])); 
            }
        }
        
            //겟 주사위
            //압수 주사위
        if(number == 1)
        {
            foreach(SkillDice i in GameManager.instance.skillDices)
            {
                if(i.cost != -1 && i.cost < 2) i.DiceSelled();
            }
        }
        if(number == 2)
        {
            foreach(SkillDice i in GameManager.instance.skillDices)
            {
                if(i.cost >= 2 && i.cost < 6) i.DiceSelled();
            }
        }
        if(number == 3)
        {
            foreach(SkillDice i in GameManager.instance.skillDices)
            {
                if(i.cost >= 6 && i.cost < 12) i.DiceSelled();
            }
        }

        //or get item

        //or get statdice
    }
}

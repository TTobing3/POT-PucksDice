using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeTable : MonoBehaviour
{
    //거래 자리는 3개

    //2개 거래 : 주사위 두 개를 올린다. 두 개가 올려졌는 지 확인. 확인 되었으면 보상 주사위를 가져온다. 보상 주사위를 가지는 순간 제물 주사위는
    //우측으로 이동 및 비활성화

    public string[] rewards = new string[3]; //
    public int[] costs = new int[12];

    public RewardDice[] rewardDices;

    public RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public bool CostCheck(int _number)
    {
        if(_number == 1)
        {
            for(int i = 0; i<2; i++)
            {
                if(costs[i] == 0) return false;
            }
            return true;
            //겟 주사위
            //압수 주사위
        }
        if(_number == 2)
        {
            for(int i = 2; i<6; i++)
            {
                if(costs[i] == 0) return false;
            }
            return true;
        }
        if(_number == 3)
        {
            for(int i = 6; i<12; i++)
            {
                if(costs[i] == 0) return false;
            }
            return true;
        }

        return false;
    }

    public void ResetCost()
    {
        System.Array.ConvertAll(costs, x => x = 0);
    }
    

    //4개 거래
    //6개 거래

    //거래 종료

}

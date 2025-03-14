using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatBar : MonoBehaviour
{
    [SerializeField] Slider p_HpBar, p_HpSubBar, p_RpBar, e_HpBar, e_HpSubBar, e_RpBar, expBar;
    [SerializeField] TextMeshProUGUI p_HpText,p_RpText,e_HpText,e_RpText,expText;

    public Entity curPlayer, curEnemy;

    void Update()
    {
        if(curPlayer != null && curPlayer.maxHp > 0)
        {
            p_HpBar.value = ( curPlayer.curHp / curPlayer.maxHp );
            p_HpSubBar.value = Mathf.Lerp( p_HpSubBar.value, curPlayer.curHp / curPlayer.maxHp, Time.deltaTime * 3f );
            p_RpBar.value = ( curPlayer.rp / curPlayer.maxRp );

            p_HpText.text = $"{(int)curPlayer.curHp} / {curPlayer.maxHp}";
            p_RpText.text = $"{(int)curPlayer.rp} / {curPlayer.maxRp}";
        }
        if(curEnemy != null && curEnemy.maxHp > 0)
        {
            e_HpBar.value = ( curEnemy.curHp / curEnemy.maxHp );
            e_HpSubBar.value = Mathf.Lerp( e_HpSubBar.value, curEnemy.curHp / curEnemy.maxHp, Time.deltaTime * 3f );
            e_RpBar.value = ( curEnemy.rp / curEnemy.maxRp );
            
            e_HpText.text = $"{(int)curEnemy.curHp} / {curEnemy.maxHp}";
            e_RpText.text = $"{(int)curEnemy.rp} / {curEnemy.maxRp}";
        }

        if(expBar.gameObject.activeSelf)
        {
            //expBar.value = ( SaveDataManager.instance.exp / SaveDataManager.instance.maxExp );
            expBar.value = Mathf.Lerp( expBar.value, (float)SaveDataManager.instance.exp / SaveDataManager.instance.maxExp, Time.deltaTime * 3f );
            expText.text = $"{(int)SaveDataManager.instance.exp} / {SaveDataManager.instance.maxExp}";

        }

    }
}

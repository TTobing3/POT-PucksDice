using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageButton : MonoBehaviour
{
    public void LanguageButtonSelect(bool isRightButton)
    {
        SoundManager.instance.UISfxPlay(10);
        if(isRightButton)
        {
            if(TextManager.instance.curLanguage < TextManager.instance.transLateList["언어"].Length-1)
            {
                TextManager.instance.ChangeLanguage(TextManager.instance.curLanguage+1);
            }
            else
            {
                TextManager.instance.ChangeLanguage(1);
            }
        }
        else
        {
            if(TextManager.instance.curLanguage == 1)
            {
                TextManager.instance.ChangeLanguage(TextManager.instance.transLateList["언어"].Length-1);
            }
            else
            {
                TextManager.instance.ChangeLanguage(TextManager.instance.curLanguage-1);
            }

        }
    }
    public void StartLanguageButtonSelect(bool isRightButton)
    {
        StartSoundManager.instance.UISfxPlay(3);
        if(isRightButton)
        {
            if(StartTextManager.instance.curLanguage < StartTextManager.instance.transLateList["언어"].Length-1)
            {
                StartTextManager.instance.ChangeLanguage(StartTextManager.instance.curLanguage+1);
            }
            else
            {
                StartTextManager.instance.ChangeLanguage(1);
            }
        }
        else
        {
            if(StartTextManager.instance.curLanguage == 1)
            {
                StartTextManager.instance.ChangeLanguage(StartTextManager.instance.transLateList["언어"].Length-1);
            }
            else
            {
                StartTextManager.instance.ChangeLanguage(StartTextManager.instance.curLanguage-1);
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleStopButton : MonoBehaviour
{
    public Sprite[] images;
    public Image image;
    private void Awake() {
        image = GetComponent<Image>();
    }

}

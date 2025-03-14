using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPOutline : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Color lineColor = new Color();

    void OnEnable()
    {
        
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();
        textmeshPro.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0.1f);
        textmeshPro.outlineWidth = 0.1f;
        textmeshPro.outlineColor = lineColor; // new Color(0.81f, 0.81f, 0.81f, 1);
    }
}

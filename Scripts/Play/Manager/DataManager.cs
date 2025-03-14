using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string[] skillNames;
    public Sprite[] skillImages, abliltyImages;
    public Dictionary<string, Sprite> skillDic = new Dictionary<string, Sprite>();
    public Dictionary<string, string> skillEft = new Dictionary<string, string>();
    public Dictionary<string, string> skillEftDes = new Dictionary<string, string>();
    public Dictionary<string, string[]> mapDes = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> eventDes = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> abliltyDes = new Dictionary<string, string[]>();
    public List<string[]> composeDes = new List<string[]>();

    //

    [HideInInspector] public List<string> abliltytName;

    //

    public string[] achieveName;
    public Sprite[] achieveImage;

    //

    public string[] effectSoundName;
    public string[] effectName;
    public Sprite[] effectImages;

    //

    public string[] eventName;
    public Sprite[] eventImages;
    public Sprite[] eventUsedImages;
    
    //
    public string[] traderName;
    public GameObject[] traders;
    public string[] traderItems;
    //

    public string[] mapLevel;
    public string[] mapName;
    public Sprite[] mapImage;

    //
    //
    public GameObject[] enemys; 
    [HideInInspector] public List<string> enemyName;
    [HideInInspector] public List<string> enemySkills;
    [HideInInspector] public List<string> enemyArmour;
    [HideInInspector] public List<string> enemyStats;
    //
    public string[] jobs; // 주사위 눈 별 직업 해도 될 듯
    public string[] jobSkills;
    public string[] jobArmourSet;
    //
    public Sprite[] diceEyes;


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
    }

    void OnEnable()
    {
        for(int i = 0; i<skillNames.Length; i++)
        {
            skillDic.Add(skillNames[i], skillImages[i]);
        }
        
    }
}

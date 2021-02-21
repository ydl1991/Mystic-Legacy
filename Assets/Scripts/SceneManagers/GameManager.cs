using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static int lastLevelGold { get; set; }

    public static GameManager s_instance;

    //UI
    public Text m_goldText;
    public int m_gold;

    //IAP 
    public bool hasExplosion;
    public bool hasShield;
    public bool hasRose;

    //achievements stuff
    //public GooglePlayGamesManager socialManager;
    public bool towerTookDamage;
    public bool wonHonorableDefenderAchievement;

    private void Awake()
    {
        s_instance = this;

        if (SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            //socialManager = GameObject.Find("SocialManager").GetComponent<GooglePlayGamesManager>();

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //start the level in landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft; //setting screen orientation
        if (lastLevelGold != 0)
            m_gold = lastLevelGold;

        //if made it to level one end, give rookie achievement 
        if(SceneManager.GetActiveScene().name == "LevelOneEnd")
        {
            //socialManager?.GiveRookieAchivement();
        }

    }


    // Update is called once per frame
    void Update()
    {
        //print the amount of gold
        if (m_goldText != null)
            m_goldText.text = m_gold.ToString();
    }

    public void GiveCredit()
    {
        ////give achievement for completing level one
        //if (googlePlayManager != null)
        //    googlePlayManager.GetComponent<GooglePlayGamesManager>()?.GiveRookieAchivement();
    }

    public void CheckHonorableDefenderAchievement()
    {
      
        if (!towerTookDamage)
        {
            //socialManager?.UpdateHonorableDefenderAchievement();
            wonHonorableDefenderAchievement = true;
        }

        //change tower took damage variable at start of each
        if (wonHonorableDefenderAchievement)
        {
            //do not reset the tower took damage bool
        }
        else if (!wonHonorableDefenderAchievement)
        {
            //reset the tower took damage bool
            if (towerTookDamage)
            {
                towerTookDamage = false;
            }
        }
    }

   

    public void ResetGold()
    {
        lastLevelGold = 0;
        m_gold = 0;
    }

    public void RecordGold()
    {
        lastLevelGold = m_gold;
    }

    public int GetGold()
    {
        return lastLevelGold;
    }

    public void GiveGold(int amount)
    {
        if (m_goldText != null)
            m_gold += amount;
    }

    public int CurrentSceneId()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}

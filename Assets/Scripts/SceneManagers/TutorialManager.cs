using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject m_tutorialPanel = null;
    [SerializeField] Text m_tutorialText = null;

    public static TutorialManager s_instance;
    public UnitUpgradePanelController m_unitPanel;
    public GameObject m_selectionPanel;
    public SceneTransition m_transition;
    public EnemySpawner m_enemySpawner;

    private Enemy m_currentTarget;
    private int m_swipeDamage;
    private Vector3 m_tabDownPos;
    private int m_layerMask;

    // Tutorial flag
    private bool m_tapOnUnitBuildingBlock;
    private bool m_buildUnit;

    private bool m_allowTapToUnitUpgrade;
    private bool m_tapOnUnitToUpgrade;

    private bool m_allowTapToTowerUpgrade;
    private bool m_tapOnTowerToUpgrade;

    //public GooglePlayGamesManager socialManager;

    public Tower m_tower;

    private void Awake()
    {
        s_instance = this;
        m_currentTarget = null;
        m_layerMask = (1 << 8) | (1 << 9);
        m_tabDownPos = Vector3.zero;
        m_swipeDamage = 30;

        m_tapOnUnitBuildingBlock = false;
        m_buildUnit = false;
        m_tapOnUnitToUpgrade = false;
        m_tapOnTowerToUpgrade = false;

        m_allowTapToUnitUpgrade = false;
        m_allowTapToTowerUpgrade = false;

        //socialManager = GameObject.Find("SocialManager").GetComponent<GooglePlayGamesManager>();

    }

    // Start is called before the first frame update
    void Start()
    {
        //by default, have the panel turn off
        m_tutorialPanel.SetActive(false);
        StartCoroutine(StartDelay());


    }

    public IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(TutorialDemo());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_tabDownPos = Helpers.ScreenToWorldPosition(Input.mousePosition);
            m_tabDownPos.y = 1.2f;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 dest = Helpers.ScreenToWorldPosition(Input.mousePosition);
            dest.y = 1.2f;
 
            // shoot a line to test collision with enemies
            RaycastHit hit;
            if (Physics.Linecast(m_tabDownPos, dest, out hit, m_layerMask)) 
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    Debug.Log("hit " + hit.collider.tag + " with id: " + enemy.id.ToString());
                    enemy.GotHit(m_swipeDamage);
                }
                else if (hit.collider.CompareTag("Shield"))
                {
                    Destroy(hit.collider.gameObject);
                }
            } 
        }
    }

    public void ResetLevel()
    {
        StopAllCoroutines();
        StartCoroutine(ResetDemo());
    }

    public IEnumerator ResetDemo()
    {
        //set panel to active
        m_tutorialPanel.SetActive(true);

        //push some text through :)
        m_tutorialText.text = "Uh oh! Someone got in to your tower.";

        //clearing the message
        yield return new WaitForSeconds(1.10f);
        m_tutorialText.text = "";

        //push some text through:)
        m_tutorialText.text = "Let's try that again.";

        //clearing the message
        yield return new WaitForSeconds(1.0f);
        m_tutorialText.text = "";

        //set panel to active
        m_tutorialPanel.SetActive(false);

        //reload the same scene
        ReloadTheScene();
    }

    IEnumerator TutorialDemo()
    {
        //set panel to active
        m_tutorialPanel.SetActive(true);

        //push some text through :)
        m_tutorialText.text = "Welcome to your land!";

        //clearing the message
        yield return new WaitForSeconds(2f);

        //push some text through :)
        m_tutorialText.text = "Your mission is to defend your tower from enemies.";

        //clearing the message
        yield return new WaitForSeconds(4f);

        //push some text through :)
        m_tutorialText.text = "It looks like some enemies are coming! Build a unit to defeat them.";

        //clearing the message
        yield return new WaitForSeconds(3f);

        //push some text through :)
        m_tutorialText.text = "To build a unit, tap on a gray block.";

        yield return new WaitForSeconds(2f);

        // if not tapping on it, check back next frame
        while(!m_tapOnUnitBuildingBlock)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //describe unit
        m_tutorialText.text = "Normal Unit fires projectiles that damage a single enemy.";

        yield return new WaitForSeconds(4f);

        m_tutorialText.text = "Iceball Unit fires elemental projectiles that slows an enemy down.";

        yield return new WaitForSeconds(4f);

        m_tutorialText.text = "Cannonball Unit fires explosive projectile that damage multiple enemies in a range.";     

        //clearing the message
        yield return new WaitForSeconds(4f);
        
        // if not built unit, check back next frame
        while(!m_buildUnit)
        {
            m_tutorialText.text = "Now build an unit.";     
            yield return null;
        }

        m_currentTarget = m_enemySpawner.TutorialEnemySpawning(EnemyType.kNormal);
        
        yield return new WaitForSeconds(1f);
        m_tutorialText.text = "Here comes the standard enemy, it deals an one-time melee damage to your tower.";

        yield return new WaitForSeconds(5f);

        m_tutorialText.text = "If you want some extra help, swipe on them to attack.";

        while (m_currentTarget)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        m_currentTarget = m_enemySpawner.TutorialEnemySpawning(EnemyType.kShield);

        yield return new WaitForSeconds(1f);

        m_tutorialText.text = "Here comes the shield enemy, the shield blocks any damages and effects from unit! Swipe on them to break the shield.";

        yield return new WaitForSeconds(5f);

        m_tutorialText.text = "If you want some extra help, swipe on them to attack.";

        while (m_currentTarget)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        m_currentTarget = m_enemySpawner.TutorialEnemySpawning(EnemyType.kRanged);

        yield return new WaitForSeconds(1f);

        m_tutorialText.text = "Here comes the ranged enemy, it continuously deals ranged damage to your tower";

        yield return new WaitForSeconds(5f);

        m_tutorialText.text = "If you want some extra help, swipe on them to attack.";

        while (m_currentTarget)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);
  
        m_tutorialText.text = "You can tap on built units to bring up the Unit menu.";
        m_allowTapToUnitUpgrade = true;

        //clearing the message
        yield return new WaitForSeconds(3f);

        // if not tap on unit, check back next frame
        while(!m_tapOnUnitToUpgrade)
        {
            m_tutorialText.text = "Tap on built unit.";     
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        m_tutorialText.text = "You can use this menu to upgrade your Unit's damage.";

        //clearing the message
        yield return new WaitForSeconds(4f);

        m_tutorialText.text = "You can tap on your tower to bring up the Tower menu.";
        m_allowTapToTowerUpgrade = true;

        yield return new WaitForSeconds(3f);

        // if not tap on tower, check back next frame
        while(!m_tapOnTowerToUpgrade)
        {
            m_tutorialText.text = "Tap on tower.";     
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        m_tutorialText.text = "You can use this menu to upgrade your Tower's health.";

        //clearing the message
        yield return new WaitForSeconds(4f);

        m_tutorialText.text = "I think you got the hang of this!";

        //clearing the message
        yield return new WaitForSeconds(2f);
        m_tutorialText.text = "Good luck and have fun!";

        //give the getting started achievement
        //socialManager.GiveGettingStartedAchievement();

        yield return new WaitForSeconds(2f);
        //set panel to active
        m_tutorialPanel.SetActive(false);

        //send player to main menu
        BackToMenu();
    }

    public void BackToMenu()
    {
        m_transition.FadeToLevel(0);
    }    
    
    public void ReloadTheScene()
    {
        m_transition.FadeToLevel(1);
    }

    public Enemy GetClosestEnemy(Vector3 pos)
    {
        return m_currentTarget;
    }

    public void ResetEnemy()
    {
        m_currentTarget = null;
    }

    public void TapOnUnitSpawner()
    {
        m_tapOnUnitBuildingBlock = true;
    }

    public void TapOnBuiltUnit()
    {
        if (m_allowTapToUnitUpgrade)
            m_tapOnUnitToUpgrade = true;
    }

    public void TapOnTower()
    {
        if (m_allowTapToTowerUpgrade)
            m_tapOnTowerToUpgrade = true;
    }

    public void BuildUnit()
    {
        m_buildUnit = true;
    }
}

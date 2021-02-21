using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool levelCompleted { get; private set; }

    public static LevelManager s_instance;

    public SceneTransition m_transition;
    public int m_levelNumber;

    private Vector3 m_tabDownPos;
    private int m_layerMask;

    public Text m_timer;
    public float m_countdown;

    public UnitUpgradePanelController m_unitPanel;
    public GameObject m_selectionPanel;

    private List<Enemy> m_enemies;
    public int m_swipeDamage;

    public GameManager gameManager;
    public Tower m_tower;
    public Button explosionButton;

    public IAPUpgradeManager IAPUpgradeManager;

    //public GooglePlayGamesManager socialManager;
    public int enemiesDefeated;

    public bool towerAttacked;

    void Awake()
    {
        s_instance = this;
        levelCompleted = false;
        m_tabDownPos = Vector3.zero;
        m_timer.color = Color.green;
        m_enemies = new List<Enemy>();
        m_swipeDamage = 30;
        // layer 8 is the enemy layer, layer 9 is the enemy layer
        m_layerMask = (1 << 8) | (1 << 9) | (1 << 11);

        //CheckUpgrades();

        //socialManager = GameObject.Find("SocialManager").GetComponent<GooglePlayGamesManager>();


    }

    void Start()
    {
        if (m_levelNumber == 1)
        {
            InstructionPanelController.showMessage("The gold you earn from each level can be carried on to the next, arrange your gold wisely.");
        }
        else if (m_levelNumber == 2)
        {
            InstructionPanelController.showMessage("Shield Enemy immunes from turret, swipe them to break the shield.");
        }
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

        // Timer and load to next level
        m_timer.text = string.Format("{0:00}:{1:00}", (int)(m_countdown / 60f), (int)(m_countdown % 60f));
        m_countdown -= Time.deltaTime;

        if (m_countdown <= 0f)
        {
            m_countdown = 0f;
            if (!levelCompleted)
            {
                Debug.Log("Level Finished");
                levelCompleted = true;
                //CheckHonorableDefenderAchievement();
                GameManager.s_instance.RecordGold();
                CleanUp();
                m_transition.FadeToNextLevel();
            }
        }
        else if (m_countdown <= 60f && m_timer.color != Color.red)
        {
            m_timer.color = Color.red;
        }
    }

    //check for the honorable defender achievement
    public void CheckHonorableDefenderAchievement()
    {
        towerAttacked = m_tower.tookDamage;
        Debug.Log(m_tower.tookDamage);
        Debug.Log(towerAttacked);

        //if the tower was not attacked, give the player the achievement
        if (!towerAttacked)
        {
            //if(socialManager != null)
            {
                //socialManager.UpdateHonorableDefenderAchievement();
            }
        }
    }


    //check the game manager for upgrades
    public void CheckUpgrades()
    {
        if (gameManager.hasShield)
        {
            //increase tower health
            Debug.Log("Tower upgraded!");
            m_tower.TowerIAPUpgrade();
        }

        if (gameManager.hasRose)
        {
            //increase player damage
            Debug.Log("Swipe damage upgraded!");
            UpgradePlayerDamage();
        }

        if (gameManager.hasExplosion)
        {
            //allow the player to use the explosion
            Debug.Log("Explosion ability unlocked!");

            explosionButton.gameObject.SetActive(true);

            //print the number of explosions available to the button

        }
        else if (!gameManager.hasExplosion)
        {
            explosionButton.gameObject.SetActive(false);
        }


    }

    public void UpgradePlayerDamage()
    {
        m_swipeDamage *= 2;
    }

    public int AddEnemy(Enemy enemy)
    {
        // check if there is any empty spot in the list, insert enemy to spot if yes
        for (int i = 0; i < m_enemies.Count; ++i)
        {
            if (m_enemies[i] == null)
            {
                m_enemies[i] = enemy;
                return i;
            }
        }

        // otherwise add enemy to the back of the list
        int id = m_enemies.Count;
        m_enemies.Add(enemy);
        return id;
    }


    public void DestroyEnemy(int id)
    {
        // if id is valid and enemy exists, kill enemy
        if (id < m_enemies.Count && m_enemies[id] != null)
        {
            Destroy(m_enemies[id].gameObject);
            m_enemies[id] = null;

            enemiesDefeated += 1;
            Debug.Log("Enemies defeated: " + enemiesDefeated);

            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                //tell the achivements that you destroyed an enemy 
                //socialManager.UpdateSkilledAttackerAchievement();
            }
            
        }
    }

    public Enemy GetClosestEnemy(Vector3 pos)
    {
        Enemy target = null;
        float distance = int.MaxValue;

        for (int id = 0; id < m_enemies.Count; ++id)
        {
            if (m_enemies[id] != null)
            {
                float dist = Vector3.Distance(pos, m_enemies[id].transform.position);
                if (dist < distance)
                {
                    distance = dist;
                    target = m_enemies[id];
                }
            }
        }

        return target;
    }

    public void BackToMene()
    {
        m_transition.FadeToLevel(0);
    }

    public void CleanUp()
    {
        for (int id = 0; id < m_enemies.Count; ++id)
        {
            DestroyEnemy(id);
        }

        //remove one from the player prefs
        PlayerPrefs.SetInt("explosionCount", PlayerPrefs.GetInt("explosionCount") - 1);

    }
}

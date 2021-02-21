using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum UnitLevel
{
    kLevel1 = 0,
    kLevel2,
    kLevel3
}

public class Unit : MonoBehaviour
{
    private static readonly int[] m_unitUpgradeCost = new int[] {
        100, 150, 200
    };

    public UnitLevel level { get; private set; }
    
    public ProjectileType m_type;
    public Projectile m_projectile;
    public int m_attackRange;
    public float m_cooldown;
  
    // Normal
    public int m_baseDamage;
    // Iceball
    public float m_baseSlowedSpeed;
    public float m_baseMaxSlowingTime;
    // Cannonball
    public float m_baseExplosionScale;

    private UnitUpgradePanelController m_unitPanel;
    private GameObject m_selectionPanel;
    private UnitSpawner m_spawner;
    private Enemy m_enemy;
    private float m_distanceFromUnit;
    private int m_accumulatedCost;

    //public GooglePlayGamesManager socialManager;
    public int upgradesDone;

    // Start is called before the first frame update
    void Start()
    {
        level = UnitLevel.kLevel1;
        if (SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            m_unitPanel = LevelManager.s_instance.m_unitPanel;
            m_selectionPanel = LevelManager.s_instance.m_selectionPanel;

            //socialManager = GameObject.Find("SocialManager").GetComponent<GooglePlayGamesManager>();

        }
        else if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            m_unitPanel = TutorialManager.s_instance.m_unitPanel;
            m_selectionPanel = TutorialManager.s_instance.m_selectionPanel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            if (LevelManager.s_instance.levelCompleted)
                return;

            m_enemy = LevelManager.s_instance.GetClosestEnemy(transform.position);

        }
        else if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            m_enemy = TutorialManager.s_instance.GetClosestEnemy(transform.position);
        }

        if (m_enemy != null)
        {
            m_distanceFromUnit = Vector3.Distance(m_enemy.transform.position, transform.position);

            if (m_distanceFromUnit <= m_attackRange)
            {
                //turn towards the enemy
                transform.LookAt(m_enemy.transform);
                //countdown the cooldown
                m_cooldown -= Time.deltaTime;

                if(m_cooldown <= 0)
                {
                    AttackEnemy();
                    m_cooldown = 3;
                }
            }
        }
    }

    public void Init(UnitSpawner spawner, int cost)
    {
        m_spawner = spawner;
        m_accumulatedCost += cost;
    }

    public void AttackEnemy()
    {
        //spawn the projectile
        Projectile proj = Instantiate(m_projectile, transform.position + (transform.forward), transform.rotation);
        proj.Init(m_type, m_enemy.transform.position, transform, m_baseDamage, m_baseSlowedSpeed, m_baseMaxSlowingTime, m_baseExplosionScale);

        Debug.Log("Attacked enemy");
    }

    public void SellUnit()
    {
        GameManager.s_instance.m_gold += (int)(m_accumulatedCost * 0.5f);
        m_spawner.gameObject.SetActive(true);
        Destroy(gameObject);
    }

    public void UpgradeUnit()
    {
        int upgradeAmount = m_unitUpgradeCost[(int)m_type] * ((int)level + 1);

        if(level < UnitLevel.kLevel3 && GameManager.s_instance.m_gold >= upgradeAmount)
        {
            GameManager.s_instance.m_gold -= upgradeAmount;

            if (m_type == ProjectileType.kNormal)
                UpgradeNormalUnit();
            else if (m_type == ProjectileType.kIceball)
                UpgradeIceballUnit();
            else if (m_type == ProjectileType.kCannonball)
                UpgradeCannonballUnit(); 
            
            level += 1;
            UpdateUpgradeMessage();

            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                //socialManager?.UpdateUpgradeMasterAchivement();

            }
        }
    }

    private void UpgradeNormalUnit()
    {
        m_baseDamage += 50;
    }

    private void UpgradeIceballUnit()
    {
        m_baseMaxSlowingTime *= 1.3f;
        m_baseSlowedSpeed *= 0.8f;
    }

    private void UpgradeCannonballUnit()
    {
        m_baseDamage += (int)((float)m_baseDamage * 1.5f);
        m_baseExplosionScale *= 1.3f;
    }

    private void UpdateUpgradeMessage()
    {
        if (m_type == ProjectileType.kNormal)
            m_unitPanel.SetDescriptionText("Current Damage:  " + m_baseDamage.ToString());
        else if (m_type == ProjectileType.kIceball)
            m_unitPanel.SetDescriptionText("Slow Effect:  " + m_baseSlowedSpeed.ToString() + " & Slow Duration:  " + m_baseMaxSlowingTime.ToString());
        else if (m_type == ProjectileType.kCannonball)
            m_unitPanel.SetDescriptionText("Current Damage:  " + m_baseDamage.ToString() + " & Explosion Dimension:  " + m_baseExplosionScale.ToString());

        if (level < UnitLevel.kLevel3)
           m_unitPanel.SetUpgradeText("Upgrade unit for " + (m_unitUpgradeCost[(int)m_type] * ((int)level + 1)).ToString() + " gold?");
        else
            m_unitPanel.SetUpgradeText("Max level reached!");
    }

    private void ActivateUpgradePanel()
    {
        m_unitPanel.ClearButtonCallbacks();
        UpdateUpgradeMessage();
        m_unitPanel.SetButtonCallback(delegate { UpgradeUnit(); });
        m_unitPanel.gameObject.SetActive(true);
    }

    private void ActivateSelectionPanel()
    {
        m_selectionPanel.SetActive(true);
        foreach (Button but in m_selectionPanel.GetComponentsInChildren<Button>())
        {
            Text butText = but.GetComponentInChildren<Text>();

            if (butText != null)
            {
                if (butText.text == "Upgrade")
                    but.onClick.AddListener(delegate{ ActivateUpgradePanel(); });
                else if (butText.text == "Sell")
                    but.onClick.AddListener(delegate{ SellUnit(); });
            }

            but.onClick.AddListener(delegate{ DeactivateSelectionPanel(); });
        }
    }

    private void DeactivateSelectionPanel()
    {
        foreach (Button but in m_selectionPanel.GetComponentsInChildren<Button>())
        {
            but.onClick.RemoveAllListeners();
        }
        m_selectionPanel.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            TutorialManager.s_instance.TapOnBuiltUnit();
        }

        ActivateSelectionPanel();
    }
}

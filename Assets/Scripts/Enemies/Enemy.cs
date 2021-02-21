using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum EnemyType
{
    kNormal = 0,
    kShield,
    kRanged
}

public class Enemy : MonoBehaviour
{
    public int id { get; private set; }
    public EnemyType type { get; private set; }

    [SerializeField] Color m_normalColor = Color.red;
    [SerializeField] Color m_hitColor = Color.cyan;
    [SerializeField] Color m_slowDownColor = Color.white;
    [SerializeField] Color m_inExplosionZoneColor = Color.magenta;
    [SerializeField] float m_normalSpeed = 0.5f;
    
    private float m_curSlowingTime;
    private NavMeshAgent m_agent;
    private MeshRenderer m_renderer;
    private HealthComponent m_healthComponent;

    private Tower m_tower;
    //public GooglePlayGamesManager socialManager;

    private void Awake()
    {
        //get the nav mesh agent and destination
        m_agent = GetComponent<NavMeshAgent>();
        m_renderer = GetComponent<MeshRenderer>();
        m_healthComponent = GetComponent<HealthComponent>();
        if (SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            //socialManager = GameObject.Find("SocialManager").GetComponent<GooglePlayGamesManager>();
            m_tower = LevelManager.s_instance.m_tower;
        }
        else
        {
            m_tower = TutorialManager.s_instance.m_tower;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        m_agent.speed = m_normalSpeed;
        m_agent.destination = m_tower.transform.position;
        m_renderer.material.color = m_normalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (type == EnemyType.kRanged && Vector3.Distance(m_agent.destination, transform.position) < 8f)
        {
            if (m_agent.enabled == true)
                StopAgent();

            //turn towards the enemy
            transform.LookAt(m_tower.transform);
            GetComponentInChildren<EnemyWeaponComponent>().active = true;
        }

        if (m_curSlowingTime > 0f)
        {
            m_curSlowingTime -= Time.deltaTime;
            if (m_curSlowingTime <= 0f)
            {
                m_curSlowingTime = 0f;
                m_agent.speed = m_normalSpeed;
                m_renderer.material.color = m_normalColor;
            }
        }
    }

    public void Init(EnemyType type, int id)
    {
        this.type = type;
        this.id = id;

        if (type == EnemyType.kShield)
        {
            Shield shieldPrefab = Resources.Load<Shield>("ShieldComponent");
            Shield shield = Instantiate(shieldPrefab, transform.position, transform.rotation);
            shield.transform.parent = transform;
        }
        else if (type == EnemyType.kRanged)
        {
            EnemyWeaponComponent wcPrefab = Resources.Load<EnemyWeaponComponent>("EnemyWeaponComponent");
            EnemyWeaponComponent wc = Instantiate(wcPrefab, transform.position, transform.rotation);
            m_healthComponent.UpgradeMaxHealth(m_healthComponent.m_maxHealth * 1.5f);
            wc.Init(m_tower.gameObject, gameObject);
        }
    }

    public void StopAgent()
    {
        m_agent.enabled = false;
    }

    public void GotHit(int damage)
    {
        m_healthComponent.ChangeHealth(-damage);
        UpdateHealthColor();

        if (SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            //online
            //socialManager?.UpdateHeavyHitterAchievement();

        }

        if (!m_healthComponent.alive)
        {
            //tell the game manager to give gold
            GameManager.s_instance.GiveGold(20);

            // destroy
            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                LevelManager.s_instance.DestroyEnemy(id);
            }
            else if (SceneManager.GetActiveScene().name == "TutorialLevel")
            {
                TutorialManager.s_instance.ResetEnemy();
                Destroy(gameObject);
            }
        }
    }

    private void UpdateHealthColor()
    {
        float healthPercent = m_healthComponent.healthPercentage;
        m_normalColor = Color.Lerp(Color.white, Color.red, healthPercent);

        if (m_curSlowingTime <= 0f && m_renderer.material.color != m_inExplosionZoneColor)
            m_renderer.material.color = m_normalColor;
    }

    private void HitByProjectile(Projectile proj, Vector3 hitPoint)
    {
        switch (proj.type)
        {
            case ProjectileType.kNormal:
                HitByNormalProjectile(proj);
                break;
            
            case ProjectileType.kIceball:
                HitByIceball(proj);
                break;

            case ProjectileType.kCannonball:
                HitByCannoball(proj, hitPoint);
                break;
        }
    }

    private void HitByNormalProjectile(Projectile proj)
    {
        CheckDamageEnemy(proj.damage);
    }

    private void HitByIceball(Projectile proj)
    {
        m_renderer.material.color = m_slowDownColor;
        m_agent.speed = proj.slowedSpeed;
        m_curSlowingTime = proj.maxSlowingTime;

        CheckDamageEnemy(proj.damage);
    }

    private void CheckDamageEnemy(int damage)
    {
        m_healthComponent.ChangeHealth(-damage);
        UpdateHealthColor();

        if(SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            //online
            //socialManager.UpdateHeavyHitterAchievement();

        }

        if (!m_healthComponent.alive)
        {
            // destroy
            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                GameManager.s_instance.GiveGold(35);
                LevelManager.s_instance.DestroyEnemy(id);
            }
            else if (SceneManager.GetActiveScene().name == "TutorialLevel")
            {
                GameManager.s_instance.GiveGold(300);
                TutorialManager.s_instance.ResetEnemy();
                Destroy(gameObject);
            }
        }
    }

    private void HitByCannoball(Projectile proj, Vector3 hitPoint)
    {
        ExplosionZone zone = Instantiate(
            proj.m_explosionZonePrefab, 
            new Vector3(100000000.0f,10000000.0f,10000000.0f), 
            Quaternion.identity
        );
        zone.SetDamage(proj.damage);
        zone.transform.localScale = new Vector3(proj.explosionScale, 0.1f, proj.explosionScale);
        zone.transform.position = hitPoint;

        if (SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            //online
            //socialManager.UpdateHeavyHitterAchievement();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ExplosionZone")
        {
            if (m_curSlowingTime <= 0f)
                m_renderer.material.color = m_inExplosionZoneColor;

            m_healthComponent.ChangeHealth((float)(-other.GetComponent<ExplosionZone>()?.damage));
            UpdateHealthColor();

            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                //online
                //socialManager.UpdateHeavyHitterAchievement();

            }

            if (!m_healthComponent.alive)
            {
                // destroy
                if (SceneManager.GetActiveScene().name != "TutorialLevel")
                {
                    GameManager.s_instance.GiveGold(35);
                    LevelManager.s_instance.DestroyEnemy(id);
                }
                else if (SceneManager.GetActiveScene().name == "TutorialLevel")
                {
                    GameManager.s_instance.GiveGold(300);
                    TutorialManager.s_instance.ResetEnemy();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            ContactPoint contact = other.contacts[0];
            Projectile proj = other.gameObject.GetComponent<Projectile>();
            HitByProjectile(proj, contact.point);
        }
        else if (other.gameObject.tag == "Tower")
        {
            Debug.Log("collide tower");

            // destroy
            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                LevelManager.s_instance.DestroyEnemy(id);
            }
            else if (SceneManager.GetActiveScene().name == "TutorialLevel")
            {
                TutorialManager.s_instance.ResetEnemy();
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ExplosionZone")
        {
            m_renderer.material.color = m_normalColor;
        }
    }
}

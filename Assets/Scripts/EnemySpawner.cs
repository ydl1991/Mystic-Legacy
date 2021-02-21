using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    // Enemy Spawn Weights
    private static ReadOnlyCollection<int> s_kTypeSpawnWeight = new ReadOnlyCollection<int>(new int[] {
        50, 40, 30
    }) ;

    public Enemy m_enemyToSpawn;
    public bool m_canSpawn;
    public float m_currentCooldown;
    public float m_maxCooldown;
    public float m_minCooldown;

    private float m_timer;

    // Start is called before the first frame update
    void Start()
    {
        m_canSpawn = true;
        m_timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            UpdateEnemySpawn();
            UpdateSpawningRate();
        }
    }

    public Enemy TutorialEnemySpawning(EnemyType type)
    {
        // Spawn enemy and initialize it's type
        Enemy enemy = Instantiate(m_enemyToSpawn, transform.position, Quaternion.identity);
        enemy.Init(type, -1);
        enemy.transform.parent = transform;
        
        return enemy;
    }

    private void UpdateSpawningRate()
    {
        m_timer += Time.deltaTime;
        // every 30 seconds, increase spawning rate
        if (m_timer >= 30f)
        {
            m_timer -= 30f;
            ReduceSpawnCooldown(0.5f);
        }
    }

    private void ReduceSpawnCooldown(float reduceAmount)
    {
        m_maxCooldown -= reduceAmount;
        // spawning cooldown cannot be less than minimum spawning cooldown
        if (m_maxCooldown < m_minCooldown)
            m_maxCooldown = m_minCooldown;
    }


    private void UpdateEnemySpawn()
    {

        if (!m_canSpawn || LevelManager.s_instance.levelCompleted)
            return;

        //countdown the cooldown
        m_currentCooldown -= Time.deltaTime;

        if (m_currentCooldown <= 0)
        {
            // Spawn enemy and initialize it's type
            Enemy enemy = Instantiate(m_enemyToSpawn, transform.position, Quaternion.identity);
            enemy.Init(GetRandomEnemyType(), LevelManager.s_instance.AddEnemy(enemy));
            enemy.transform.parent = transform;

            // reset cooldown
            m_currentCooldown = m_maxCooldown;
        }
    }

    private EnemyType GetRandomEnemyType()
    {
        // determine what kinds of enemies are currently spawning by level
        int level = LevelManager.s_instance.m_levelNumber;
        int totalWeight = 0;
        for (int i = 0; i < level; ++i)
        {
            totalWeight += s_kTypeSpawnWeight[i];
        }
        
        // Weighted random type selection
        int roll = Random.Range(0, totalWeight);
        if (roll < s_kTypeSpawnWeight[0])
            return EnemyType.kNormal;
        if (roll < s_kTypeSpawnWeight[0] + s_kTypeSpawnWeight[1])
            return EnemyType.kShield;
    
        return EnemyType.kRanged;
    }
}

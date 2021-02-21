using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitSpawner : MonoBehaviour
{
    private static readonly int[] m_unitSpawnCost = new int[] {
        50, 150, 350
    };

    public UnitSpawnPanelManager m_unitSpawnPanel;
    [SerializeField] GameObject[] m_unitPrefabs;

    public bool m_canSpawn;

    // Start is called before the first frame update
    void Start()
    {
        m_canSpawn = true;
    }

    private void SpawnUnit(ProjectileType type)
    {        
        // Successfully spawn object
        int cost = m_unitSpawnCost[(int)type];
        if (GameManager.s_instance.m_gold >= cost)
        {
            if (SceneManager.GetActiveScene().name == "TutorialLevel")
            {
                TutorialManager.s_instance.BuildUnit();
            }

            GameManager.s_instance.m_gold -= cost;
            GameObject unit = Instantiate(m_unitPrefabs[(int)type], transform.position, Quaternion.identity);
            unit.GetComponent<Unit>().Init(this, m_unitSpawnCost[(int)type]);
            
            m_unitSpawnPanel.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void SetCallbacks()
    {
        m_unitSpawnPanel.SetButtonCallback(ProjectileType.kNormal, delegate { SpawnUnit(ProjectileType.kNormal); });
        m_unitSpawnPanel.SetButtonCallback(ProjectileType.kIceball, delegate { SpawnUnit(ProjectileType.kIceball); });
        m_unitSpawnPanel.SetButtonCallback(ProjectileType.kCannonball, delegate { SpawnUnit(ProjectileType.kCannonball); });
    }

    private void OnMouseDown()
    {
        if(m_canSpawn && !m_unitSpawnPanel.gameObject.activeSelf)
        {
            if (SceneManager.GetActiveScene().name == "TutorialLevel")
            {
                TutorialManager.s_instance.TapOnUnitSpawner();
            }

            m_unitSpawnPanel.ClearButtonCallbacks();
            SetCallbacks();
            m_unitSpawnPanel.gameObject.SetActive(true);
        }
    }
}

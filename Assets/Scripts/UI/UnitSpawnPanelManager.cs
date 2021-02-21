using UnityEngine;
using UnityEngine.UI;


public class UnitSpawnPanelManager : MonoBehaviour
{
    [SerializeField] Text[] m_unitSpawnTexts;
    [SerializeField] Button[] m_unitSpawnButtons;

    private UnitSpawner m_selectingSpawner = null;

    // Update is called once per frame
    void Update()
    {
        int currentGold = GameManager.s_instance.m_gold;
        if (currentGold < 50)
            m_unitSpawnTexts[(int)ProjectileType.kNormal].color = Color.red;
        else
            m_unitSpawnTexts[(int)ProjectileType.kNormal].color = Color.green;
        
        if (currentGold < 150)
            m_unitSpawnTexts[(int)ProjectileType.kIceball].color = Color.red;
        else
            m_unitSpawnTexts[(int)ProjectileType.kIceball].color = Color.green;

        if (currentGold < 350)
            m_unitSpawnTexts[(int)ProjectileType.kCannonball].color = Color.red;
        else
            m_unitSpawnTexts[(int)ProjectileType.kCannonball].color = Color.green;
    }

    public void SetButtonCallback(ProjectileType type, UnityEngine.Events.UnityAction callback)
    {
        m_unitSpawnButtons[(int)type].onClick.AddListener(callback);
    }

    public void ClearButtonCallbacks()
    {
        foreach (Button but in m_unitSpawnButtons)
        {
            but.onClick.RemoveAllListeners();
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPUpgradeManager : MonoBehaviour
{
    public IAPManager IAPmanager;

    public Button explosionButton;
    public Text explosionText;

    public Text roseImage;
    public Text shieldImage;

    public Tower tower;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        CheckingStoreEntitlements();

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("explosionCount") >= 1)
        {

            explosionText.text = ("Explosion count: " + PlayerPrefs.GetInt("explosionCount").ToString());

        }
        else if (PlayerPrefs.GetInt("explosionCount") == 0)
        {

            explosionButton.gameObject.SetActive(false);
        }

        if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Swipe damage: " + levelManager.m_swipeDamage);
            Debug.Log("Tower health: " + tower.GetComponent<HealthComponent>().health);
        }
    }

    public void CheckingStoreEntitlements()
    {
        //if it is true that there is a shield
        if (PlayerPrefs.GetInt("hasShield") == 1)
        {
            tower.TowerIAPUpgrade();
        }
        else
        {
            shieldImage.gameObject.SetActive(false);
        }

        //if it is true that there is a rose
        if (PlayerPrefs.GetInt("hasRose") == 1)
        {
            levelManager.UpgradePlayerDamage();

        }
        else
        {
            roseImage.gameObject.SetActive(false);
        }

  




    }
}

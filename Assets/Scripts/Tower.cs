using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour
{
    public UnitLevel level { get; private set; }

    public GameObject m_towerPanel;

    public Vector3 m_towerPosition;
    public Slider m_healthWheel;
    public Image m_healthFillImage;

    public HealthComponent m_healthComponent;
    public int m_towerUpgradeCost;

    public Text m_healthText;
    public Text m_towerUpgradeText;

    public bool tookDamage;

    // Start is called before the first frame update
    void Start()
    {
        m_towerPanel.SetActive(false);
        level = UnitLevel.kLevel1;
        m_healthComponent = GetComponent<HealthComponent>();
        Color green = Color.green;
        green.a = 0.4f;
        m_healthFillImage.color = green;

        tookDamage = false;
        UpdateUpgradeMessage();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_towerPosition;

        if (!m_healthComponent.alive)
        {
            //LevelManager.s_instance.BackToMene();
            //reset the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

       
    }

    //show upgrade panel for tower
    public void ShowTowerPanel()
    {
        if(m_towerPanel.activeSelf == false)
        {
            m_towerPanel.SetActive(true);
        }
    }

    //upgrade tower
    public void UpgradeTower()
    {
        int upgradeAmount = m_towerUpgradeCost * ((int)level + 1);
        if(level < UnitLevel.kLevel3 && GameManager.s_instance.m_gold >= upgradeAmount)
        {
            GameManager.s_instance.m_gold -= upgradeAmount;
            m_healthComponent.UpgradeMaxHealth(m_healthComponent.m_maxHealth + 100 * ((int)level + 1));
            level += 1;

            UpdateUpgradeMessage();
        }
    }

    public void TowerIAPUpgrade()
    {
        if(m_healthComponent != null)
        {
            Debug.Log("uh");

        }
        else
        {
            Debug.Log("nuh");

        }

        //A one-time addition to max health for the IAP
        m_healthComponent.UpgradeMaxHealth(m_healthComponent.m_maxHealth + 150);

    }


    private void AdjustHealthWheelValueAndColor()
    {
        // get health component from target object, note: HealthComponent might be attouched to 
        // child object, so using GetComponentInChildren to search the object as well as its children
        // to find the first HealthComponent. Also we might note that to only assign one health component
        // to an object.
        float healthPercent = GetComponentInChildren<HealthComponent>().healthPercentage;
        
        if (m_healthWheel.value != healthPercent * 100f)
        {
            m_healthWheel.value = healthPercent * 100f;
            Color newColor = Color.Lerp(Color.red, Color.green, healthPercent);
            newColor.a = 0.4f;
            m_healthFillImage.color = newColor;
        }
    }

    private void UpdateUpgradeMessage()
    {
        m_healthText.text = "Health:  " + m_healthComponent.health.ToString() + " / " + m_healthComponent.m_maxHealth.ToString();

        if (level < UnitLevel.kLevel3)
            m_towerUpgradeText.text = "Upgrade tower for " + (m_towerUpgradeCost * ((int)level + 1)).ToString() + " gold?";
        else
            m_towerUpgradeText.text = "Max level reached!";
    }
    
    private void OnMouseDown()
    {
        if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            TutorialManager.s_instance.TapOnTower();
        }

        ShowTowerPanel();   
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("collide enemy");
            if (SceneManager.GetActiveScene().name == "TutorialLevel")
            {
                TutorialManager.s_instance.ResetLevel();
            }
            else
            {
                m_healthComponent.ChangeHealth(-15f);
                AdjustHealthWheelValueAndColor();

                if (SceneManager.GetActiveScene().name != "TutorialLevel")
                {
                    //tell the game manager that the tower took damage
                    if (GameManager.s_instance.towerTookDamage == false)
                    {
                        GameManager.s_instance.towerTookDamage = true;
                        Debug.Log("Took damage");

                    }
                }
            }
        }
        else if (other.gameObject.tag == "EnemyProjectile")
        {
            m_healthComponent.ChangeHealth(-5f);
            AdjustHealthWheelValueAndColor();

            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                //tell the game manager that the tower took damage
                if (tookDamage == false)
                {
                    tookDamage = true;
                    Debug.Log("Took damage");

                }
            }
        }
    }
}

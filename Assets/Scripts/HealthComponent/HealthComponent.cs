using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Health Component can be attached to any game object or its children object
// but note that one object tree should only contain one health component
public class HealthComponent : MonoBehaviour
{
    // properties
    public float health { get; private set; }
    public float healthPercentage { get; private set; }
    public bool alive { get { return health > 0f; } }

    public float m_maxHealth;
    public AudioSource m_hurtSound;

    void Awake()
    {
        health = m_maxHealth;
        Refresh();
    }

    public void UpgradeMaxHealth(float newMaxHealth)
    {
        m_maxHealth = newMaxHealth;
        health = m_maxHealth;

        Refresh();
    }

    // if taking damage, delta should be a negative number
    // if healing, delta should be positive
    public void ChangeHealth(float delta)
    {
        if (delta < 0 && m_hurtSound != null && !m_hurtSound.isPlaying)
            m_hurtSound.Play();

        health += delta;

        if (health > m_maxHealth)
            health = m_maxHealth;
        else if (health < 0f)
            health = 0f;

     
        Refresh();
    }

    // refresh health scale
    private void Refresh()
    {
        healthPercentage = health / m_maxHealth;
    }
}

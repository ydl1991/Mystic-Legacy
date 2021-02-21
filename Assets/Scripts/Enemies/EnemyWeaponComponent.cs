using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponComponent : MonoBehaviour
{
    public bool active { get; set; }

    public BasicProjectile m_bulletPrefab = null;
    public float m_cooldown;

    private GameObject m_target;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        m_cooldown = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            //countdown the cooldown
            m_cooldown -= Time.deltaTime;

            if(m_cooldown <= 0)
            {
                ShootTarget();
                m_cooldown = 2f;
            }
        }
    }

    public void Init(GameObject target, GameObject parent)
    {
        m_target = target;
        transform.parent = parent.transform;
    }

    public void ShootTarget()
    {
        //spawn the projectile
        BasicProjectile proj = Instantiate(m_bulletPrefab, transform.position + (transform.forward * 2), Quaternion.identity);
        proj.Init(m_target.transform.position, transform);
    }
}

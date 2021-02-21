using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public float m_speed = 8f;
    protected Rigidbody m_rb;
    protected Vector3 m_direction;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.freezeRotation = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(10, 8);
        Physics.IgnoreLayerCollision(10, 9);
        StartCoroutine(DelayDestroyer());
    }

    public void Init(Vector3 target, Transform parent)
    {
        m_direction = (target - transform.position).normalized * m_speed;
        m_rb.velocity = new Vector3(m_direction.x, m_direction.y, m_direction.z);

        transform.parent = parent;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Tower")
        {
            //destroy the projectile upon impact
            Destroy(gameObject);
        }
    }

    IEnumerator DelayDestroyer()
    {
        yield return new WaitForSeconds(10.0f);

        if (gameObject != null)
            Destroy(gameObject);
    }
}

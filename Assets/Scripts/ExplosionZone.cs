using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionZone : MonoBehaviour
{
    public int damage { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayDestroyer());
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    IEnumerator DelayDestroyer()
    {
        yield return new WaitForSeconds(1.0f);
        transform.position = new Vector3(100000000.0f,10000000.0f,10000000.0f);
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}

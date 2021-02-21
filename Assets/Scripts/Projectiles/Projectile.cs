using System.Collections;
using UnityEngine;

public enum ProjectileType
{
    kNormal = 0,
    kIceball,
    kCannonball
}

public class Projectile : MonoBehaviour
{
    public ProjectileType type { get; private set; }
    // Normal
    public int damage { get; private set; }
    // Iceball
    public float slowedSpeed { get; private set; }
    public float maxSlowingTime { get; private set; }
    // Cannonball
    public float explosionScale { get; private set; }

    public ExplosionZone m_explosionZonePrefab;
    public float m_speed = 5f;

    private Rigidbody m_rb;
    private Vector3 m_direction;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.freezeRotation = true;
        damage = 0;
        slowedSpeed = 0f;
        maxSlowingTime = 0f;
        explosionScale = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayDestroyer());
    }

    public void Init(ProjectileType type, Vector3 target, Transform parent, int damage, float slowSpeed, float slowTime, float explosionScale)
    {
        this.type = type;
        LoadMaterial();

        m_direction = (target - transform.position).normalized * m_speed;
        m_rb.velocity = new Vector3(m_direction.x, m_direction.y, m_direction.z);
        transform.parent = parent;

        if (type == ProjectileType.kNormal)
        {
            this.damage = damage;
        }
        else if (type == ProjectileType.kIceball)
        {
            slowedSpeed = slowSpeed;
            maxSlowingTime = slowTime;
        }
        else if (type == ProjectileType.kCannonball)
        {
            this.explosionScale = explosionScale;
        }
    }

    private void LoadMaterial()
    {
        string matName = string.Empty;

        switch (type)
        {
            case ProjectileType.kIceball:
                matName = "IcyMat";
                break;

            case ProjectileType.kCannonball:
                matName = "FireMat";
                break;

            default:
                return;
        }

        Material mat = Resources.Load<Material>(matName);
        GetComponent<MeshRenderer>().sharedMaterial = mat;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Shield")
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

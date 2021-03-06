using UnityEngine;

public class GamabuntaProjectile : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;

    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private bool exploded = false;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        // if (collisions > maxCollisions) Explode();

        // maxLifetime -= Time.deltaTime;
        // if (maxLifetime <= 0) Explode();
    }

    private void Explode()
    {
        exploded = true;

        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies); 
        for (int i = 0; i < enemies.Length; i++)
        {
            Debug.Log(enemies[i].name);
            enemies[i].GetComponent<Player>().TakeDamage(explosionDamage);
            
        }
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.collider.CompareTag("Player") || !collision.collider.CompareTag("Gamabunta")) && explodeOnTouch && !exploded) Explode();
    }

    private void Setup()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
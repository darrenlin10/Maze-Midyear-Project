using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 5f;
    public float lifetime = 2f;
    private float timer;
    void Start()
    {

        // Set the velocity of the projectile

        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth bh = collision.gameObject.GetComponent<BossHealth>();
            if (bh != null) bh.TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag("Ground")){Destroy(gameObject);}
        Destroy(gameObject);
    }
}

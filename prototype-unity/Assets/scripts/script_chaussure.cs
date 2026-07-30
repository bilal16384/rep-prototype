using UnityEngine;

public class script_projectile_attaque_spéciale : MonoBehaviour
{
    public float vitesseProjectileX;
    public float dégâtsProjectile;
    public float vitesseProjectileY;
    private float duréeVieProjectile = 5f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField] private BoxCollider2D boxColliderEnnemi;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(vitesseProjectileX, vitesseProjectileY);
        Destroy(gameObject, duréeVieProjectile);
    }

    // Update is called once per frame
    void Update()
    {
        
        

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == boxColliderEnnemi)
        {
            
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("Sol"))
        {
            Destroy(gameObject);
            Debug.Log("Projectile détruit en touchant le sol.");
        }
    }


}

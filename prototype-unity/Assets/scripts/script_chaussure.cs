using UnityEngine;

public class script_projectile_attaque_spéciale : MonoBehaviour
{
    public Vector3 positionDépart;
    public float vitesseProjectileX;
    public int dégâtsProjectile;
    public float vitesseProjectileY;
    private float duréeVieProjectile = 5f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        Destroy(gameObject, duréeVieProjectile);

        transform.position = positionDépart;   // Position de départ à défiir selon les règles...
        rb.linearVelocity = new Vector2(vitesseProjectileX, vitesseProjectileY);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
        

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent != null && collision.transform == transform.parent)
            {
                Debug.Log("Collision ignorée avec le parent : " + collision.gameObject.name);
                return; 
            }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Sol"))
        {
            Destroy(gameObject);
            Debug.Log("Projectile détruit en touchant le sol.");
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Personnage"))
        {
            infligerDégâts(collision.gameObject, dégâtsProjectile);
            Destroy(gameObject);
            Debug.Log("Projectile détruit en touchant un personnage.");
        }
    }


    private void infligerDégâts(GameObject cible, int dégâts)
    {
        script_personnage personnageCible = cible.GetComponent<script_personnage>();
        if (personnageCible != null)
        {
            personnageCible.prendreDégâts(dégâts);
            Debug.Log("Dégâts infligés à " + cible.name + " : " + dégâts);
        }
        else
        {
            Debug.Log("Aucun script_personnage trouvé sur " + cible.name);
        }
    }

}

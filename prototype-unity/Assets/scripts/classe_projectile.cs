using UnityEngine;


public class classe_projectile : MonoBehaviour
{
    //attribus du projectile
    protected Vector3 positionDépart;
    protected float vitesseProjectileX;
    protected int dégâtsProjectile;
    protected float vitesseProjectileY;
    protected float duréeVieProjectile;
    protected int layerPersonnageEnnemi;

    //références aux composants du projectile
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        Destroy(gameObject, duréeVieProjectile+3);

        rb.linearVelocity = new Vector2(vitesseProjectileX, vitesseProjectileY);
        transform.position = positionDépart;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
    }

    //Détecte les collisions avec d'autres objets
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) // vérifie si le projectile touche le terrain
        {
            Destroy(gameObject);
            
        }
        if (collisionAvecEnnemi(collision)) // vérifie si le projectile touche un ennemi
        {
            if (infligerDégâts(collision.gameObject, dégâtsProjectile))
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual bool infligerDégâts(GameObject cible, int dégâts)
    {
        if (cible.TryGetComponent<In_prendre_dégâts>(out In_prendre_dégâts personnageCible)) // Vérifie si le GameObject cible a un composant qui implémente l'interface In_prendre_dégâts et l'assigne à la variable personnageCible
        {
            personnageCible.prendreDégâts(dégâts);
            Debug.Log("Dégâts infligés à " + cible.name + " : " + dégâts);
            return true; // Retourne true si les dégâts ont été infligés avec succès
        }
        else
        {
            Debug.Log("Aucune interface In_prendre_dégâts trouvé sur " + cible.name);
            return false; // Retourne false si le GameObject cible n'a pas de composant qui implémente l'interface In_prendre_dégâts
        }
    }

    protected virtual bool collisionAvecEnnemi(Collider2D collision) // Vérifie si le GameObject avec lequel il y a collision est un ennemi
    {
        return collision.gameObject.layer == layerPersonnageEnnemi;
    }

    public void initialiserProjectile(Vector3 position, float vitesseX, float vitesseY, int dégâts, float duréeVie, int layerEnnemi)
    {
        positionDépart = position;
        vitesseProjectileX = vitesseX;
        vitesseProjectileY = vitesseY;
        dégâtsProjectile = dégâts;
        layerPersonnageEnnemi = layerEnnemi;
        duréeVieProjectile = duréeVie;
    }


}

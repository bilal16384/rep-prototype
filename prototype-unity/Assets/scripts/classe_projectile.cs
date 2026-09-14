using UnityEngine;
using UnityEngine.InputSystem;


public class classe_projectile : classe_attaque
{
    //attribus du projectile
    protected Vector3 positionDépart;
    protected float vitesseProjectileX;
    protected int dégâtsProjectile;
    protected float vitesseProjectileY;
    protected float duréeVieProjectile = 5f; // Durée de vie par défaut du projectile en secondes
    protected float gravitéProjectile = 0f; // Gravité par défaut du projectile A !!!! DÉFINIR !!!!
    protected float pourcentageAffaiblissementProjectile = 0f; // Pourcentage d'affaiblissement par défaut du projectile
    protected float duréeAffaiblissementProjectile = 0f; // Durée d'affa
    protected float pourcentageRalentiProjectile = 0f; // Pourcentage de ralentissement par défaut du projectile
    protected float duréeRalentiProjectile = 0f; // Durée de ralentissement par défaut du projectile
    //touches
    protected Key toucheActivationProjectile;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        rb.gravityScale = gravitéProjectile; // définit la gravité du projectile
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        Destroy(gameObject, duréeVieProjectile); // détruit le projectile après la durée de vie spécifiée

        rb.linearVelocity = new Vector2(vitesseProjectileX, vitesseProjectileY);
        transform.position = positionDépart;
    }

    // Update is called once per frame
    protected override void Update()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
    }

    //Détecte les collisions avec d'autres objets
    protected virtual void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) // vérifie si le projectile touche le terrain
        {
            Destroy(gameObject);
            
        }
        if (collisionAvecEnnemi(collision)) // vérifie si le projectile touche un ennemi
        {
            if (infligerEffet(collision.gameObject, dégâtsProjectile, pourcentageAffaiblissementProjectile, duréeAffaiblissementProjectile, pourcentageRalentiProjectile, duréeRalentiProjectile))
            {
                Destroy(gameObject);
            }
        }
    }


    public virtual void initialiserProjectile // Méthode pour initialiser les paramètres du projectile
    (
        Vector3 position, 
        float vitesseX, 
        float vitesseY, 
        int dégâts, 
        int layerEnnemi, 
        float gravité = 0f,
        float pourcentageAffaiblissement = 0f,
        float duréeAffaiblissement = 0f,
        float pourcentageRalenti = 0f,
        float duréeRalenti = 0f,
        Key toucheActivation = Key.None // touche pour activer l'effet de la bombe magique
    )
    {
        positionDépart = position;
        vitesseProjectileX = vitesseX;
        vitesseProjectileY = vitesseY;
        gravitéProjectile = gravité;
        dégâtsProjectile = dégâts;
        layerPersonnageEnnemi = layerEnnemi;
        pourcentageAffaiblissementProjectile = pourcentageAffaiblissement;
        duréeAffaiblissementProjectile = duréeAffaiblissement;
        pourcentageRalentiProjectile = pourcentageRalenti;
        duréeRalentiProjectile = duréeRalenti;
        toucheActivationProjectile = toucheActivation;
        
    }

    
}

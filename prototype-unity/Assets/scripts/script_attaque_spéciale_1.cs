using UnityEngine;
using UnityEngine.InputSystem;
public class script_attaque_spéciale_1 : classe_attaque_spéciale
{
    //attribus du coup de l'attaque spéciale
    protected bool enAttaqueSpécialeCoup = false; // Indique si l'attaque spéciale coup est en cours
    [SerializeField] protected int dégâtsAttaqueSpécialeCoup; // Dégâts infligés par l'attaque spéciale coup

    //attribus des projectiles de l'attaque spéciale
    [SerializeField] protected GameObject prefabChaussure; // Préfabriqué du projectile de l'attaque spéciale
    protected bool enAttaqueSpécialeProjectile = false; // Indique si l'attaque spéciale projectile est en cours
    [SerializeField] protected int quantitéProjetiles = 3; // Nombre de projectiles à tirer
    [SerializeField] protected int dégâtsAttaqueSpécialeProjectile; // Dégâts infligés par chaque projectile
    [SerializeField] protected float vitesseProjectileX; // Vitesse du projectile sur l'axe X
    [SerializeField] protected float vitesseProjectileY; // Vitesse initiale du projectile sur l'axe Y
    [SerializeField] protected float variationVitesseProjectileY = 0.5f; // Variation de la vitesse du projectile sur l'axe Y
    [SerializeField] protected float gravitéProjectile = 2.5f; // Gravité appliquée au projectile
    [SerializeField] protected float pourcentageRalentiProjectile = 0.5f; // Pourcentage de ralenti appliqué par le projectile
    [SerializeField] protected float duréeRalentiProjectile = 1f; // Durée du ralenti appliqué par le projectile
    protected float vitesseProjectileYActuelle = 0.5f; // Vitesse actuelle du projectile sur l'axe Y
    protected bool regardeDroite = true; // Indique si le personnage regarde vers la droite et donc la direction des projectiles
    protected float tempsAttaqueProjectile = 0.1f; // Temps entre chaque projectile
    protected Vector3 positionProjectile; // Position de départ du projectile   
    protected int quantitéProjetilesRestante; // Nombre de projectiles restants à tirer


    // Méthode appelée lors de l'initialisation de l'objet (avant Start)
    protected override void Awake()
    {
        base.Awake(); // Appelle la méthode Awake de la classe parente
    }

    // Méthode appelée avant la première frame de mise à jour
    protected override void Start()
    {
        base.Start(); // Appelle la méthode Start de la classe parente
    }

    // Méthode appelée à chaque frame
    protected override void Update()
    {
        base.Update(); // Appelle la méthode Update de la classe parente

        //attaque spéciale (transition entre le coup et les projectiless)

        if (enAttaqueSpécialeCoup)
        {
            if (Time.time - tempsDernièreAttaqueSpéciale >= duréeAttaqueSpéciale) // vérifie si la durée de l'attaque spéciale coup est écoulée
            {
                Debug.Log("Transition vers l'attaque spéciale par projectiles.");
                transitionAttaqueProjectile(); // Transition de l'attaque spéciale coup vers l'attaque spéciale projectile
            }
        }
        if (enAttaqueSpécialeProjectile)
        {
            if (quantitéProjetilesRestante > 0)
            {
                tirerProjectiles(); // Tire les projectiles restants
                finAttaqueSpéciale(); // Termine l'attaque spéciale après avoir tiré les projectiles
            }
        }
    }

    
    protected void OnTriggerStay2D(Collider2D collision) // Méthode appelée lorsque le collider reste en contact avec un autre collider 2D
    {
        //attaque spéciale coup
        if (enAttaqueSpécialeCoup)
        {
            Debug.Log("Collision détectée avec : " + collision.gameObject.name + "enAttaqueSpécialeCoup : " + enAttaqueSpécialeCoup);
            if(collisionAvecEnnemi(collision)) // Vérifie si la collision est avec un ennemi
            {
                if (infligerDégâts(collision.gameObject, dégâtsAttaqueSpécialeCoup)) // Vérifie si les dégâts ont été infligés avec succès
                {
                    finAttaqueSpéciale(); // Termine l'attaque spéciale après avoir infligé des dégâts avec le coup
                    Debug.Log("fin de l'attaque spéciale après avoir infligé des dégâts avec le coup.");
                }
            }
        }
    }

    protected override void attaqueSpéciale() // Méthode appelée pour déclencher l'attaque spéciale
    {
        base.attaqueSpéciale();
        enAttaqueSpécialeCoup = true;
    }
    protected override void finAttaqueSpéciale() // Méthode appelée pour terminer l'attaque spéciale
    {
        enAttaqueSpécialeCoup = false;
        enAttaqueSpécialeProjectile = false;
        base.finAttaqueSpéciale();
    }


    protected void transitionAttaqueProjectile() // Transition de l'attaque spéciale coup vers l'attaque spéciale projectile
    {
        enAttaqueSpécialeCoup = false;
        enAttaqueSpécialeProjectile = true;
        quantitéProjetilesRestante = quantitéProjetiles;
    }
    protected void tirerProjectiles() // Méthode appelée pour tirer les projectiles de l'attaque spéciale
    {
        if (quantitéProjetilesRestante > 0) // Vérifie s'il reste des projectiles à instancier
        {
            
            regardeDroite = transform.parent.localScale.x > 0; // vérifie la direction du personnage pour déterminer la direction du projectile
            positionProjectile = boxCollider.bounds.center; // récupère la position de la zone d'attaque pour instancier le projectile
            if (!regardeDroite) // si le personnage regarde vers la gauche, on inverse la vitesse horizontale du projectile
            {
                vitesseProjectileX = -Mathf.Abs(vitesseProjectileX); // Inverse la vitesse horizontale du projectile si le personnage regarde vers la gauche
            }
            else
            {
                vitesseProjectileX = Mathf.Abs(vitesseProjectileX); // Assure que la vitesse horizontale du projectile est positive si le personnage regarde vers la droite
            }
            génererProjectile( 
                prefabChaussure, 
                positionProjectile, 
                vitesseProjectileX, 
                vitesseProjectileYActuelle, 
                dégâtsAttaqueSpécialeProjectile, 
                gravitéProjectile,
                0,
                0,
                pourcentageRalentiProjectile,
                duréeRalentiProjectile
            ); // Instancie le projectile avec les paramètres spécifiés
            // Instancie le projectile avec la vitesse actuelle
            vitesseProjectileYActuelle += variationVitesseProjectileY; // Modification de la vitesse verticale du projectile pour créer un effet de dispersion
            quantitéProjetilesRestante -= 1; // Décrémente le nombre de projectiles restants
            Invoke("tirerProjectiles", tempsAttaqueProjectile); // Rappel de la fonction après un certain temps pour tirer le prochain projectile
        }
        else
        {
            Debug.Log("Fin de l'attaque spéciale par projectiles.");
            vitesseProjectileYActuelle = vitesseProjectileY; // Réinitialisation de la variation de vitesse verticale pour le prochain tir :)
        }
    }
}



using UnityEngine;
using UnityEngine.InputSystem;
public class script_attaque : classe_attaque
{
    //attribus du coup de l'attaque spéciale
    protected bool enAttaqueSpécialeCoup = false;
    [SerializeField] protected int dégâtsAttaqueSpécialeCoup;

    //attribus des projectiles de l'attaque spéciale]
    [SerializeField] protected GameObject prefabChaussure;
    protected bool enAttaqueSpécialeProjectile = false;
    [SerializeField] protected int quantitéProjetiles = 3;
    [SerializeField] protected int dégâtsAttaqueSpécialeProjectile;
    [SerializeField] protected float vitesseProjectileX;
    [SerializeField] protected float duréeVieProjectile;
    [SerializeField] protected float vitesseProjectileY;
    [SerializeField] protected float variationVitesseProjectileY = 0.5f;
    protected float vitesseProjectileYActuelle = 0.5f;
    protected bool 
    regardeDroite = true; // Variable pour déterminer la direction du personnage et donc des projectiles
    protected float tempsAttaqueProjectile = 0.1f; //temps entre chaque projectile
    protected Vector3 positionProjectile; //position de départ du projectile
    
    

    
    protected int quantitéProjetilesRestante;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //attaque spéciale (transition entre le coup et les projectiless)

        if (enAttaqueSpécialeCoup)
        {
            if (Time.time - tempsDernièreAttaqueSpéciale >= duréeAttaqueSpéciale)
            {
                transitionAttaqueProjectile();
            }
        }
        if (enAttaqueSpécialeProjectile)
        {
            if (quantitéProjetilesRestante > 0)
            {
                tirerProjectiles();
                finAttaqueSpéciale();
            }
        }
    }

    // 
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("colizion détékté, enAttaqueSpécialeCoup : " + enAttaqueSpécialeCoup);
        //attaque spéciale coup
        if (enAttaqueSpécialeCoup)
        {
            Debug.Log("Collision détectée avec : " + collision.gameObject.name + "enAttaqueSpécialeCoup : " + enAttaqueSpécialeCoup);
            if (collisionAvecEnnemi(collision))
            {
                infligerDégâts(collision.gameObject, dégâtsAttaqueSpécialeCoup);
                finAttaqueSpéciale();
            }
        }
    }

    protected override void attaqueSpéciale()
    {
        base.attaqueSpéciale();
        Debug.Log("attaque spéciale ++ activée");
        enAttaqueSpécialeCoup = true;
        
    }
    protected override void finAttaqueSpéciale()
    {
        enAttaqueSpécialeCoup = false;
        enAttaqueSpécialeProjectile = false;
        base.finAttaqueSpéciale();
    }


    protected void transitionAttaqueProjectile()
    {
        enAttaqueSpécialeCoup = false;
        enAttaqueSpécialeProjectile = true;
        quantitéProjetilesRestante = quantitéProjetiles;
    }
    protected void tirerProjectiles()
    {
        if (quantitéProjetilesRestante > 0) // vérifie s'il reste des projectiles à instancier
        {
            
            regardeDroite = transform.parent.localScale.x > 0; // vérifie la direction du personnage pour déterminer la direction du projectile
            positionProjectile = boxCollider.bounds.center; // récupère la position de la zone d'attaque pour instancier le projectile
            if (!regardeDroite) // si le personnage regarde vers la gauche, on inverse la vitesse horizontale du projectile
            {
                vitesseProjectileX = -Mathf.Abs(vitesseProjectileX);
            }
            else
            {
                vitesseProjectileX = Mathf.Abs(vitesseProjectileX);
            }
            génererProjectile(prefabChaussure, positionProjectile, vitesseProjectileX, vitesseProjectileYActuelle, dégâtsAttaqueSpécialeProjectile, duréeVieProjectile);
            vitesseProjectileYActuelle += variationVitesseProjectileY; //modification de la vitesse verticale du projectile pour créer un effet de dispersion
            quantitéProjetilesRestante-=1;
            Invoke("tirerProjectiles", tempsAttaqueProjectile); // rappel de la fonction après un certain temps pour tirer le prochain projectile
        }
        else
        {
            Debug.Log("Fin de l'attaque spéciale par projectiles.");
            vitesseProjectileYActuelle = vitesseProjectileY; //réinitialisation de la variation de vitesse verticale pour le prochain tir :)
        }
    }

    protected void génererProjectile(GameObject projectile, Vector3 position, float vitesseX, float vitesseY, int dégâts, float duréeVie)
    {
        // Instancie le projectile à la position spécifiée sans rotation
        GameObject nouveauProjectile = Instantiate(projectile, position, Quaternion.identity); 
        script_projectile_attaque_spéciale scriptProjectile = nouveauProjectile.GetComponent<script_projectile_attaque_spéciale>();
        if (scriptProjectile != null)
        {
            scriptProjectile.initialiserProjectile(position, vitesseX, vitesseY, dégâts, duréeVie, layerPersonnageEnnemi);

        }
        else
        {
            Debug.LogError("Le GameObject n'a pas de script_projectile_attaque_spéciale attaché.");
            
        }
    }
}



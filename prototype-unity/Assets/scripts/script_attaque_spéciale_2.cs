using UnityEngine;
using UnityEngine.InputSystem;
public class script_attaque_spéciale_2 : classe_attaque_spéciale
{
    //attribus des projectiles de l'attaque spéciale
    [SerializeField] protected GameObject prefabBombeMagique;
    [SerializeField] protected float vitesseProjectileX;
    [SerializeField] protected float vitesseProjectileY;
    [SerializeField] protected int dégâtsAttaqueSpécialeProjectile;
    [SerializeField] protected float pourcentageAffaiblissementProjectile;
    [SerializeField] protected float duréeAffaiblissementProjectile;
    [SerializeField] protected float rayonExplosionBombeMagique; 
    protected bool regardeDroite = true; // Variable pour déterminer la direction du personnage et donc des projectiles
    protected Vector3 positionProjectile; //position de départ du projectile
    protected float gravitéProjectile = 2.5f;
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
        Keyboard clavier = Keyboard.current;
        if (clavier != null)
        {
            if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame && Time.time - tempsDernièreAttaqueSpéciale >= rechargeAttaqueSpéciale)
            {
                attaqueSpéciale();
                tempsDernièreAttaqueSpéciale = Time.time;
            }
            else if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame)
            {
                Debug.Log("Attaque spéciale en recharge. Temps restant : " + (rechargeAttaqueSpéciale - (Time.time - tempsDernièreAttaqueSpéciale)) + " secondes.");
            }
        }        
    }


    protected void tirerProjectile()
    {
        Debug.Log("Tir du projectile de l'attaque spéciale !" + "toucheAttaqueSpeciale: " + toucheAttaqueSpeciale);
        positionProjectile = boxCollider.bounds.center; // récupère la position de la zone d'attaque pour instancier le projectile
        regardeDroite = transform.parent.localScale.x > 0; // Met à jour la direction du personnage avant de tirer le projectile
        Debug.Log("Direction du personnage : " + (regardeDroite ? "Droite" : "Gauche"));
        if (regardeDroite)
        {
            vitesseProjectileX = Mathf.Abs(vitesseProjectileX); // Assure que la vitesse du projectile est positive si le personnage regarde à droite
        }
        else
        {
            vitesseProjectileX = -Mathf.Abs(vitesseProjectileX); // Inverse la vitesse du projectile si le personnage regarde à gauche
        }
        génererProjectile
        (
            prefabBombeMagique,
            positionProjectile, 
            vitesseProjectileX, 
            vitesseProjectileY, 
            dégâtsAttaqueSpécialeProjectile, 
            gravitéProjectile,
            pourcentageAffaiblissementProjectile,
            duréeAffaiblissementProjectile,
            0f,
            0f,
            toucheAttaqueSpeciale
        );
        

    }
    protected override void génererProjectile(GameObject prefabProjectile, Vector3 position, float vitesseX, float vitesseY, int dégâts, float gravité = 0f, float pourcentageAffaiblissement = 0f, float duréeAffaiblissement = 0f, float pourcentageRalenti = 0f, float duréeRalenti = 0f, Key toucheActivation = Key.None)
    {
        base.génererProjectile(prefabProjectile, position, vitesseX, vitesseY, dégâts, gravité, pourcentageAffaiblissement, duréeAffaiblissement, pourcentageRalenti, duréeRalenti, toucheActivation);
        if (prefabProjectile == prefabBombeMagique)
        {
            script_bombe_magique scriptBombeMagique = prefabProjectile.GetComponent<script_bombe_magique>();
            if (scriptBombeMagique != null)
            {
                scriptBombeMagique.initialiserRayonExplosion(rayonExplosionBombeMagique);
            }
            else
            {
                Debug.LogError("Le script 'script_bombe_magique' n'a pas été trouvé sur le projectile instancié.");
            }
        }
    }
    protected override void attaqueSpéciale()
    {
        tirerProjectile();

        finAttaqueSpéciale();
    }
    



}



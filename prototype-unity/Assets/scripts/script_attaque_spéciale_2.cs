using UnityEngine;
using UnityEngine.InputSystem;
public class script_attaque_spéciale_2 : classe_attaque_spéciale
{
    //attribus des projectiles de l'attaque spéciale
    [SerializeField] protected GameObject prefabBombeMagique; // Référence au prefab de la bombe magique
    [SerializeField] protected float vitesseProjectileX; // Vitesse horizontale du projectile
    [SerializeField] protected float vitesseProjectileY; // Vitesse verticale du projectile
    [SerializeField] protected int dégâtsAttaqueSpécialeProjectile; // Dégâts infligés par le projectile
    [SerializeField] protected float pourcentageAffaiblissementProjectile; // Pourcentage d'affaiblissement appliqué par le projectile
    [SerializeField] protected float duréeAffaiblissementProjectile; // Durée de l'affaiblissement appliqué par le projectile
    [SerializeField] protected float rayonExplosionBombeMagique; // Rayon d'explosion de la bombe magique
    protected bool regardeDroite = true; // Variable pour déterminer la direction du personnage et donc des projectiles
    protected Vector3 positionProjectile; //position de départ du projectile
    protected float gravitéProjectile = 2.5f; // Gravité appliquée au projectile
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake() // Méthode appelée lors de l'initialisation de l'objet
    {
        base.Awake();
    }

    protected override void Start() // Méthode appelée avant la première mise à jour de l'objet
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() // Méthode appelée une fois par frame
    {
        base.Update(); // Appelle la mise à jour de la classe parente pour gérer les attaques spéciales
    }


    protected void tirerProjectile() // Méthode pour tirer un projectile de la bombe magique
    {
        positionProjectile = boxCollider.bounds.center; // récupère la position de la zone d'attaque pour instancier le projectile
        regardeDroite = transform.parent.localScale.x > 0; // Met à jour la direction du personnage avant de tirer le projectile
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
        // Appelle la génération du projectile avec les paramètres spécifiés

    }
    protected override void génererProjectile
    (GameObject prefabProjectile, 
    Vector3 position, 
    float vitesseX, 
    float vitesseY, 
    int dégâts, 
    float gravité = 0f, 
    float pourcentageAffaiblissement = 0f, 
    float duréeAffaiblissement = 0f, 
    float pourcentageRalenti = 0f, 
    float duréeRalenti = 0f, 
    Key toucheActivation = Key.None)
    {
        base.génererProjectile // Appelle la méthode de génération de projectile de la classe parente
        (prefabProjectile, 
        position, 
        vitesseX, 
        vitesseY, 
        dégâts, 
        gravité, 
        pourcentageAffaiblissement, 
        duréeAffaiblissement, 
        pourcentageRalenti, 
        duréeRalenti, 
        toucheActivation);
        if (prefabProjectile == prefabBombeMagique)
        {
            script_bombe_magique scriptBombeMagique = prefabProjectile.GetComponent<script_bombe_magique>(); // Récupère le script attaché au prefab de la bombe magique pour initialiser son rayon d'explosion
            if (scriptBombeMagique != null)
            {
                scriptBombeMagique.initialiserRayonExplosion(rayonExplosionBombeMagique); // Initialise le rayon d'explosion de la bombe magique
            }
            else
            {
                Debug.LogError("Le script 'script_bombe_magique' n'a pas été trouvé sur le projectile instancié.");
            }
        }
    }
    protected override void attaqueSpéciale() // Surcharge de la méthode d'attaque spéciale pour tirer le projectile spécifique
    {
        base.attaqueSpéciale(); // Appelle la méthode d'attaque spéciale de la classe parente
        tirerProjectile();

        finAttaqueSpéciale();
    }
    



}



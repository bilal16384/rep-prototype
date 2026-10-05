using UnityEngine;
using UnityEngine.InputSystem;
public class script_bombe_magique : classe_projectile
{
    //attributs spécifiques à la bombe magique
    protected float rayonExplosion = 3f; // Rayon de l'explosion de la bombe magique
    protected int dégâtsExplosion; // Dégâts infligés par l'explosion de la bombe magique
    protected bool estExplosé = false; // Indique si la bombe magique a explosé ou non
    protected LayerMask layerPersonnageAllié; // Layer du personnage allié
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
       
    }
    protected override void Start()
    {
        base.Start();
        layerPersonnageAllié = obtenirLayerEnnemi(layerPersonnageEnnemi); // Obtient le layer du personnage allié en fonction du layer du personnage ennemi
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Keyboard clavier = Keyboard.current;
        if (clavier != null)
        {
            if (clavier[toucheActivationProjectile].wasPressedThisFrame)
            {
                Debug.Log("Touche d'activation de la bombe magique pressée !");
                exploserBombeMagique();
            }
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Terrain") || collision.gameObject.layer == layerPersonnageEnnemi) && !estExplosé) // Explose la bombe magique si elle touche le terrain ou un ennemi et qu'elle n'a pas encore explosé
        {
            exploserBombeMagique();
        }
    }
    public override void initialiserProjectile // Méthode pour initialiser les paramètres du projectile
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
        base.initialiserProjectile(position, vitesseX, vitesseY, dégâts, layerEnnemi, gravité, pourcentageAffaiblissement, duréeAffaiblissement, pourcentageRalenti, duréeRalenti, toucheActivation);
        dégâtsExplosion = dégâts; // Initialise les dégâts de l'explosion avec les dégâts du projectile
        dégâtsProjectile = 0; // Réinitialise les dégâts du projectile à 0 car ils sont maintenant gérés par l'explosion
        Debug.Log("Dégâts de l'explosion initialisés à : " + dégâtsExplosion);

    }
    public void initialiserRayonExplosion(float rayon)
    {
        rayonExplosion = rayon;
    }

    protected void exploserBombeMagique()
    {
        estExplosé = true;
        Debug.Log("La bombe magique a explosé !");
        Collider2D[] ciblesTouchées = Physics2D.OverlapCircleAll(transform.position, rayonExplosion, 1 << layerPersonnageEnnemi | 1 << layerPersonnageAllié); // Récupère tous les colliders des ennemis et des alliés dans le rayon d'explosion
        foreach (Collider2D cible in ciblesTouchées)
        {
            if (collisionAvecEnnemi(cible)) // Vérifie si le GameObject avec lequel il y a collision est un ennemi
            {
                infligerDégâts(cible.gameObject, dégâtsExplosion); // Inflige les dégâts de la bombe magique à l'ennemi
            }
            
            if (collisionAvecAllié(cible)) // Vérifie si le GameObject avec lequel il y a collision est un allié
            {
                infligerSoins(cible.gameObject, dégâtsExplosion); // Soigne l'allié avec les points de soin de la bombe magique
            }
        }
        
        Destroy(gameObject); // Détruit la bombe magique après l'explosion
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
public class script_bombe_magique : classe_projectile
{
    //attributs spécifiques à la bombe magique
    protected float rayonExplosion = 3f; // Rayon de l'explosion de la bombe magique
    protected float delaiExplosion = 0.5f; // Délai avant de pouvoir exploser la bombe magique après son instantiation
    protected float tempsLancement; // Temps auquel la bombe magique a été lancée
    protected int dégâtsExplosion; // Dégâts infligés par l'explosion de la bombe magique
    protected bool estExplosé = false; // Indique si la bombe magique a explosé ou non
    protected LayerMask layerPersonnageAllié; // Layer du personnage allié
    
    protected override void Awake() // Méthode appelée lors de l'initialisation de la bombe magique
    {
        base.Awake();
        tempsLancement = Time.time; // Initialise le temps de lancement de la bombe magique
    }
    protected override void Start() // Méthode appelée au début de la vie de la bombe magique
    {
        base.Start();
        layerPersonnageAllié = obtenirLayerEnnemi(layerPersonnageEnnemi); // Obtient le layer du personnage allié en fonction du layer du personnage ennemi
    }

    // Update is called once per frame
    protected override void Update() // Méthode appelée à chaque frame pour mettre à jour la bombe magique
    {
        base.Update();
        Keyboard clavier = Keyboard.current; // Récupère l'état actuel du clavier
        if (clavier != null) // Vérifie si le clavier est disponible
        {
            if (clavier[toucheActivationProjectile].wasPressedThisFrame && Time.time - tempsLancement >= delaiExplosion) // Vérifie si la touche d'activation est pressée et si le délai d'explosion est écoulé avant d'exploser la bombe magique
            {
                Debug.Log("Touche d'activation de la bombe magique pressée !");
                exploserBombeMagique(); // Explose la bombe magique
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

    }
    public void initialiserRayonExplosion(float rayon) // Méthode pour initialiser le rayon de l'explosion de la bombe magique
    {
        rayonExplosion = rayon;
    }

    protected void exploserBombeMagique() // Méthode pour gérer l'explosion de la bombe magique
    {
        estExplosé = true;
        Debug.Log("La bombe magique a explosé !");
        Collider2D[] ciblesTouchées = Physics2D.OverlapCircleAll(transform.position, rayonExplosion, 1 << layerPersonnageEnnemi | 1 << layerPersonnageAllié); // Récupère tous les colliders des ennemis et des alliés dans le rayon d'explosion
        foreach (Collider2D cible in ciblesTouchées) // Parcourt tous les colliders touchés par l'explosion
        {
            if (collisionAvecEnnemi(cible)) // Vérifie si le GameObject avec lequel il y a collision est un ennemi et lui inflige des dégâts
            {
                infligerDégâts(cible.gameObject, dégâtsExplosion); // Inflige les dégâts de la bombe magique à l'ennemi
            }
            
            if (collisionAvecAllié(cible)) // Vérifie si le GameObject avec lequel il y a collision est un allié et lui applique des soins
            {
                infligerSoins(cible.gameObject, dégâtsExplosion); // Soigne l'allié avec les points de soin de la bombe magique
            }
        }
        
        Destroy(gameObject); // Détruit la bombe magique après l'explosion pour libérer les ressources
    }
}

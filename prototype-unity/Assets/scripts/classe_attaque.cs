using UnityEngine;
using UnityEngine.InputSystem;
public class classe_attaque : MonoBehaviour // Classe de base pour les attaques utilisée par la classe projectile et les classes d'attaque basique et spéciale
{
    //attributs des attaques
    protected LayerMask layerPersonnageEnnemi = 0; // Layer du personnage ennemi, à définir dans les classes héritières
    protected float pourcentageRéduction = 0f; // Variable pour stocker le pourcentage de réduction des dégâts
    protected float duréeAffaiblissement = 0f; // Variable pour stocker la durée de l'affaiblissement
    protected int dégâts; // Dégâts infligés par l'attaque
    protected bool enElimination = false; // Indique si l'attaque est en phase d'élimination après le temps imparti, utilisé pour gérer la suppression de l'attaque après son effet

    //références aux composants de l'attaque
    protected BoxCollider2D boxCollider; // Référence au BoxCollider2D de l'attaque

    protected Rigidbody2D rb; // Référence au Rigidbody2D de l'attaque

    // références au script du visuel du joueur
    protected classe_visuel_joueur scriptVisuelJoueur;

    protected virtual void Awake() // Méthode appelée lors de l'initialisation de l'attaque, avant le Start
    {
        rb = GetComponent<Rigidbody2D>(); // Récupère la référence au Rigidbody2D attaché à l'attaque
        boxCollider = GetComponent<BoxCollider2D>(); // Récupère la référence au BoxCollider2D attaché à l'attaque
    }




    protected virtual void définirLayerPersonnageEnnemi(int layer)// Définit le layer du personnage ennemi en fonction du layer du personnage actuel
    {
        if (layer == LayerMask.NameToLayer("Personnage_1")) // Layer "Personnage_1"
            layerPersonnageEnnemi = LayerMask.NameToLayer("Personnage_2"); // Layer "Personnage_2"
        else if (layer == LayerMask.NameToLayer("Personnage_2")) // Layer "Personnage_2"
            layerPersonnageEnnemi = LayerMask.NameToLayer("Personnage_1"); // Layer "Personnage_1"
        else
            Debug.LogWarning("Le personnage " + gameObject.name + " n'est pas sur un layer valide pour définir le layer ennemi.");
    }
    protected virtual int obtenirLayerEnnemi(int layer) // Retourne le layer du personnage ennemi en fonction du layer du personnage actuel
    {
        if (layer == LayerMask.NameToLayer("Personnage_1")) // Layer "Personnage_1"
            return LayerMask.NameToLayer("Personnage_2"); // Layer "Personnage_2"
        else if (layer == LayerMask.NameToLayer("Personnage_2")) // Layer "Personnage_2"
            return LayerMask.NameToLayer("Personnage_1"); // Layer "Personnage_1"
        else
        {
            Debug.LogWarning("Le personnage " + gameObject.name + " n'est pas sur un layer valide pour obtenir le layer ennemi." + " Layer actuel : " + LayerMask.LayerToName(layer));
            return 0; // Retourne 0 si le layer n'est pas valide
        }
    }

    protected virtual bool collisionAvecEnnemi(Collider2D collision) // Vérifie si le GameObject avec lequel il y a collision est un ennemi
    {
        bool collisionEnnemi = collision.gameObject.layer == layerPersonnageEnnemi;
        return collisionEnnemi;
    }
    protected virtual bool collisionAvecAllié(Collider2D collision) // Vérifie si le GameObject avec lequel il y a collision est un allié
    {
        bool collisionAllié = collision.gameObject.layer == obtenirLayerEnnemi(layerPersonnageEnnemi);
        return collisionAllié;
    }



    // Méthodes pour infliger des effets
    protected virtual bool infligerEffet(GameObject cible, int dégâts = 0, float pourcentageAffaiblissement = 0f, float duréeAffaiblissement = 0f, float pourcentageRalenti = 0f, float duréeRalenti = 0f) // Méthode pour infliger des dégâts, un affaiblissement et un ralentissement à un GameObject cible
    {
        if (cible.TryGetComponent<In_prendre_dégâts>(out In_prendre_dégâts personnageCible)) // Vérifie si le GameObject cible a un composant qui implémente l'interface In_prendre_dégâts et l'assigne à la variable personnageCible
        {
            personnageCible.prendreDégâts(dégâts); // Applique les dégâts au personnage cible
            personnageCible.affaiblir(pourcentageAffaiblissement, duréeAffaiblissement); // Applique l'affaiblissement au personnage cible avec la durée spécifiée
            personnageCible.ralentir(duréeRalenti, pourcentageRalenti); // Applique le ralentissement au personnage cible avec la durée et le pourcentage spécifiés
            return true; // Retourne true si les dégâts ont été infligés avec succès
        }
        else
        {
            return false; // Retourne false si le GameObject cible n'a pas de composant qui implémente l'interface In_prendre_dégâts
        }
    }

    protected virtual bool infligerDégâts(GameObject cible, int dégâts) // Méthode pour infliger uniquement des dégâts à un GameObject cible
    {
        return infligerEffet(cible, dégâts); // Appelle infligerEffet avec un pourcentage de réduction de 0
    }
    protected virtual bool infligerSoins(GameObject cible, int pointsDeSoin) // Méthode pour infliger des soins à un GameObject cible
    {
        if (cible.TryGetComponent<In_prendre_dégâts>(out In_prendre_dégâts personnageCible)) // Vérifie si le GameObject cible a un composant qui implémente l'interface In_prendre_dégâts et l'assigne à la variable personnageCible
        {
            personnageCible.soigner(pointsDeSoin); // Applique les soins au personnage cible
            return true; // Retourne true si les soins ont été infligés avec succès
        }
        else
        {
            return false; // Retourne false si le GameObject cible n'a pas de composant qui implémente l'interface In_prendre_dégâts
        }
    }
    protected virtual bool infligerAffaiblissement(GameObject cible, float pourcentageAffaiblissement, float duréeAffaiblissement) // Méthode pour infliger un affaiblissement à un GameObject cible
    {
        return infligerEffet(cible, 0, pourcentageAffaiblissement, duréeAffaiblissement); // Appelle infligerEffet avec des dégâts de 0
    }
    protected virtual bool infligerRalenti(GameObject cible, float pourcentageRalenti, float duréeRalenti) // Méthode pour infliger un ralentissement à un GameObject cible
    {
        return infligerEffet(cible, 0 , 0, 0, pourcentageRalenti, duréeRalenti); // Appelle infligerEffet avec des dégâts de 0, un pourcentage d'affaiblissement de 0 et applique le ralentissement au personnage cible
    }




    protected virtual void génererProjectile
    (GameObject projectile, 
    Vector3 position, 
    float vitesseX, float vitesseY, 
    int dégâts, 
    float gravité = 0f, 
    float pourcentageAffaiblissement = 0f, 
    float duréeAffaiblissement = 0f, 
    float pourcentageRalenti = 0f, 
    float duréeRalenti = 0f, 
    Key toucheActivation = Key.None)
    {
        // Instancie le projectile à la position spécifiée sans rotation
        GameObject nouveauProjectile = Instantiate(projectile, position, Quaternion.identity); 
        classe_projectile scriptProjectile = nouveauProjectile.GetComponent<classe_projectile>(); // Récupère le script classe_projectile attaché au nouveau projectile
        if (scriptProjectile != null)
        {
            scriptProjectile.initialiserProjectile // Appelle la méthode d'initialisation du projectile avec les paramètres spécifiés
            (
                position, 
                vitesseX, 
                vitesseY, 
                dégâts, 
                layerPersonnageEnnemi, 
                gravité, 
                pourcentageAffaiblissement, 
                duréeAffaiblissement, 
                pourcentageRalenti, 
                duréeRalenti,
                toucheActivation
            );

        }
        else
        {
            Debug.LogError("Le GameObject n'a pas de script_projectile_attaque_spéciale attaché.");
        }
    }
    public virtual void mettreEnElimination() // Méthode publique pour mettre le personnage en élimination
    {
        enElimination = true;
    }

    



}

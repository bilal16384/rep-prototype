using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class classe_attaque_base : classe_attaque
{
    //attribus des attaques

    [SerializeField] protected int dégâtsAttaqueBase; // Dégâts infligés par l'attaque de base
    [SerializeField] protected float rechargeAttaqueBase; // Temps de recharge de l'attaque de base


    //touches
    [SerializeField] protected Key toucheAttaqueBase; // Touche assignée pour l'attaque de base


    //variables d'attaque
    protected float duréeAttaqueBase = 0.05f; // Durée pendant laquelle l'attaque de base est active
    
    protected bool enAttaque = false; // Indique si l'attaque de base est en cours

    protected float tempsDernièreAttaque = 0f; // Temps écoulé depuis la dernière attaque de base

    


    // Référence au script du personnage
    protected classe_personnage scriptPersonnage;





    protected override void Awake() // Appelé lors de l'initialisation du script
    {
        base.Awake();
        scriptPersonnage = transform.parent.GetComponent<classe_personnage>(); // Récupère le script du personnage attaché au parent du GameObject de l'attaque
    }



    protected virtual void Start() // Appelé avant la première frame de mise à jour
    {
        scriptVisuelJoueur = scriptPersonnage.récupérerScriptVisuelJoueur();
        this.gameObject.layer = transform.parent.gameObject.layer; // Assigne le layer du GameObject de l'attaque au même layer que le parent (personnage)
        enAttaque = false;
        tempsDernièreAttaque = -(rechargeAttaqueBase); // permet d'attaquer dès le début du jeu
        if(layerPersonnageEnnemi == 0)
        {
            définirLayerPersonnageEnnemi(gameObject.layer); // Définit le layer du personnage ennemi en fonction du layer du personnage actuel
        }
    }

    // Update is called once per frame
    protected virtual void Update() // Appelé une fois par frame
    {
        Keyboard clavier = Keyboard.current; // Récupère le clavier actuel
        if (clavier != null) // Vérifie si le clavier est disponible
        {
            if (clavier[toucheAttaqueBase].wasPressedThisFrame && Time.time - tempsDernièreAttaque >= rechargeAttaqueBase && !enElimination) // Vérifie si la touche d'attaque de base est pressée, si le temps de recharge est écoulé et si le personnage n'est pas en élimination
            {
                attaqueBase();
                if (scriptVisuelJoueur != null) // Vérifie si le script visuel du joueur est disponible
                {
                    scriptVisuelJoueur.ViderRechargeAttaqueBase(rechargeAttaqueBase); // Met à jour la barre de recharge de l'attaque de base du joueur visuel
                }
                tempsDernièreAttaque = Time.time; // Met à jour le temps de la dernière attaque de base
            }
        }

        if (enAttaque) // Vérifie si le personnage est actuellement en train d'attaquer
        {
            if (Time.time - tempsDernièreAttaque >= duréeAttaqueBase) // Vérifie si la durée de l'attaque de base est écoulée
            {
                finAttaqueBase();
            }
        }
    }

    protected virtual void attaqueBase() // Déclenche l'attaque de base du personnage
    {
        enAttaque = true;
        Debug.Log("Attaque de base effectuée !");
    }

    protected virtual void finAttaqueBase() // Termine l'attaque de base du personnage
    {
        enAttaque = false;
        Debug.Log("Fin de l'attaque de base.");
    }





    protected virtual void OnTriggerStay2D(Collider2D collision) // Appelé lorsque le collider reste en contact avec un autre collider 2D
    {
        if(enAttaque && collisionAvecEnnemi(collision)) // Vérifie si le personnage est en attaque et si le collider est celui d'un ennemi
        {
            GameObject cible = collision.gameObject; // Récupère le GameObject de la cible avec laquelle le collider est en contact
                
            if (infligerDégâts(cible, dégâtsAttaqueBase)) // Tente d'infliger des dégâts à la cible
            {
                finAttaqueBase(); // Fin de l'attaque après avoir infligé des dégâts à la cible :)
            }
                
        }
    }




  

    protected virtual float obtenirDégâts(float dégâts) // Calcule les dégâts après application des réductions éventuelles du personnage
    {
        if (scriptPersonnage == null) // Vérifie si le script du personnage est assigné
        {
            Debug.LogError("Le script du personnage n'est pas assigné dans " + gameObject.name);
            return dégâts; // Retourne les dégâts d'origine si le script du personnage n'est pas assigné ou inexistant
        }
        pourcentageRéduction = scriptPersonnage.obtenirEstAffaibli(); // Obtient le pourcentage de réduction des dégâts du personnage
        if (pourcentageRéduction > 0f) // Vérifie si une réduction est applicable
        {
            float dégâtsRéduits = dégâts * (1f - pourcentageRéduction); // Calcule les dégâts après réduction
            return dégâtsRéduits; // Retourne les dégâts réduits
        }
        else // Si aucune réduction n'est applicable
        {
            return dégâts; // Retourne les dégâts d'origine si aucune réduction n'est applicable
        }
    }




    public virtual void assignerTouches(Key toucheAttaqueBase) // Assigne la touche pour l'attaque de base
    {
        this.toucheAttaqueBase = toucheAttaqueBase;
    }
}

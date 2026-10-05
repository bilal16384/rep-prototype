using UnityEngine;
using UnityEngine.InputSystem;

public class classe_attaque_spéciale : classe_attaque
{
    //attributs des attaques spéciales
    [SerializeField] protected float rechargeAttaqueSpéciale; // Temps de recharge de l'attaque spéciale en secondes
    //touches
    [SerializeField] protected Key toucheAttaqueSpeciale; // Touche pour activer l'attaque spéciale

    protected float duréeAttaqueSpéciale = 0.05f; // Durée de l'attaque spéciale en secondes
    protected float tempsDernièreAttaqueSpéciale = 0f; // Temps écoulé depuis la dernière attaque spéciale
    protected bool enAttaqueSpéciale = false; // Indique si le personnage est actuellement en attaque spéciale

    //script du personnage
    protected classe_personnage scriptPersonnage; // Référence au script du personnage parent

    protected override void Awake() // Méthode appelée lors de l'initialisation du script, avant Start()
    {
        base.Awake(); // Appelle la méthode Awake() de la classe parente (classe_attaque)
        scriptPersonnage = transform.parent.GetComponent<classe_personnage>(); // Récupère le script du personnage parent
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start() // Méthode appelée avant la première frame Update() du script
    {
        scriptVisuelJoueur = scriptPersonnage.récupérerScriptVisuelJoueur();
        this.gameObject.layer = transform.parent.gameObject.layer; // Assigne le layer du GameObject de l'attaque au même layer que le parent (personnage)
        enAttaqueSpéciale = false;
        tempsDernièreAttaqueSpéciale = -(rechargeAttaqueSpéciale); // permet d'attaquer dès le début du jeu
        if(layerPersonnageEnnemi == 0)
        {
            définirLayerPersonnageEnnemi(gameObject.layer); // Définit le layer du personnage ennemi en fonction du layer du personnage actuel
        }
    }

    // Update is called once per frame
    protected virtual void Update() // Méthode appelée à chaque frame
    {
        Keyboard clavier = Keyboard.current; // Récupère l'état actuel du clavier
        if (clavier != null) // Vérifie si le clavier est disponible
        {
            if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame && Time.time - tempsDernièreAttaqueSpéciale >= rechargeAttaqueSpéciale && !enElimination) // Vérifie si la touche de l'attaque spéciale a été pressée, si le temps de recharge est écoulé et si le personnage n'est pas en élimination
            {
                attaqueSpéciale(); // Appelle la méthode pour effectuer l'attaque spéciale
                if (scriptVisuelJoueur != null) // Vérifie si le script visuel du joueur est disponible
                {
                    scriptVisuelJoueur.ViderRechargeAttaqueSpeciale(rechargeAttaqueSpéciale); // Vide la recharge de l'attaque spéciale dans le script visuel du joueur
                }
                tempsDernièreAttaqueSpéciale = Time.time; // Met à jour le temps de la dernière attaque spéciale
            }
        }
    
    }





    protected virtual void attaqueSpéciale() // Méthode pour effectuer l'attaque spéciale
    {
        enAttaqueSpéciale = true;
        Debug.Log("Attaque spéciale effectuée.");
    }
    protected virtual void finAttaqueSpéciale() // Méthode pour terminer l'attaque spéciale
    {
        enAttaqueSpéciale = false;
        Debug.Log("Fin de l'attaque spéciale.");
    }

    public virtual void assignerTouches(Key toucheAttaqueSpéciale) // Méthode pour assigner la touche de l'attaque spéciale
    {
        this.toucheAttaqueSpeciale = toucheAttaqueSpéciale; // Assigne la touche de l'attaque spéciale
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

}

using UnityEngine;
using UnityEngine.UIElements;

public class script_GameManager : MonoBehaviour
{
    //attributs du GameManager

    // Référence aux données des joueurs
    [SerializeField] private Données_joueurs données_joueurs;


    // Référence aux scripts des personnages
    protected classe_personnage scriptPersonnageJoueur;
    protected classe_attaque_base scriptAttaqueBaseJoueur;

    protected classe_attaque_spéciale scriptAttaqueSpécialeJoueur;
    

    // Caméra
    protected Camera mainCamera; // Référence à la caméra principale du jeu
    [SerializeField] protected float zoom; // Distance de zoom de la caméra

    //attribus des personnages
    protected int personnageChoisi1; // Indique le personnage choisi par le joueur 1
    protected int personnageChoisi2; // Indique le personnage choisi par le joueur 2

    [SerializeField] protected GameObject personnage1; // Référence au prefab du personnage 1
    [SerializeField] protected GameObject personnage2; // Référence au prefab du personnage 2

    protected GameObject personnageJoueur1; // Référence au personnage du joueur 1
    protected GameObject personnageJoueur2; // Référence au personnage du joueur 2

    protected BoxCollider2D boxCollider1;
    protected BoxCollider2D boxCollider2;

    
    [SerializeField] protected Vector3 positionInitialeJoueur1; // Position initiale du personnage du joueur 1
    [SerializeField] protected Vector3 positionInitialeJoueur2; // Position initiale du personnage du joueur 2
    [SerializeField] protected Vector3 positionRéapparition; // Position de réapparition des personnages après leur élimination

    //attribus de partie
    protected int duréePartie = 300; // Durée de la partie en secondes
    protected float tempsRestant; // Temps restant de la partie en secondes
    protected int tempsAffiché;

    



    protected void Awake()
    {
        // Initialisation du GameManager

        // Instanciation des personnages
        personnageChoisi1 = donnéesDePartie.Instance.obtenirPersonnageChoisi1();
        personnageChoisi2 = donnéesDePartie.Instance.obtenirPersonnageChoisi2();
        instancierPersonnages(personnageChoisi1, personnageChoisi2, personnage1, personnage2); // Instancie les personnages en fonction des choix des joueurs

        boxCollider1 = personnageJoueur1.GetComponent<BoxCollider2D>(); // Récupère le BoxCollider2D du personnage du joueur 1
        boxCollider2 = personnageJoueur2.GetComponent<BoxCollider2D>(); // Récupère le BoxCollider2D du personnage du joueur 2
        initialiserPersonnage(personnageJoueur1, positionInitialeJoueur1, boxCollider2, 6); // 6 est le layer personnage_1
        initialiserPersonnage(personnageJoueur2, positionInitialeJoueur2, boxCollider1, 7); // 7 est le layer personnage_2

        // Assignation des touches aux personnages
        assignerTouchesPersonnage(personnageJoueur1, 6); // Assigne les touches au personnage du joueur 1
        assignerTouchesPersonnage(personnageJoueur2, 7); // Assigne les touches au personnage du joueur 2
        
        //assignation de la caméra principale
        mainCamera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera.orthographicSize = zoom; // Définit la distance de zoom de la caméra
        tempsRestant = duréePartie; // Initialise le temps restant à la durée de la partie
        tempsAffiché = duréePartie;
    }

    // Update is called once per frame
    void Update()
    {
        tempsRestant -= Time.deltaTime; // Décrémente le temps restant en fonction du temps écoulé depuis la dernière frame

        tempsAffiché = Mathf.CeilToInt(tempsRestant); // Initialise le temps affiché à la valeur entière du temps restant
    }


    void initialiserPersonnage(GameObject personnageJoueur, Vector3 positionInitiale, BoxCollider2D boxColliderEnnemi, int layerPersonnage)
    {
        personnageJoueur.GetComponent<classe_personnage>().initialiserPersonnage(positionInitiale, boxColliderEnnemi, layerPersonnage);
        if (layerPersonnage == 6) personnageJoueur1 = personnageJoueur;
        if (layerPersonnage == 7) personnageJoueur2 = personnageJoueur;
    }

    void instancierPersonnages(int choix1, int choix2, GameObject personnage1, GameObject personnage2)
    {
        if (choix1 == 1)
        {
            personnageJoueur1 = Instantiate(personnage1); // Instancie le personnage du joueur 1
        }
        else if (choix1 == 2)
        {
            personnageJoueur1 = Instantiate(personnage2); // Instancie le personnage du joueur 1
        }
        if (choix2 == 1)
        {
            personnageJoueur2 = Instantiate(personnage1); // Instancie le personnage du joueur 2
        }
        else if (choix2 == 2)
        {
            personnageJoueur2 = Instantiate(personnage2); // Instancie le personnage du joueur 2
        }

        personnageJoueur1.layer = 6; // Assigne le layer du personnage du joueur 1
        personnageJoueur2.layer = 7; // Assigne le layer du personnage du joueur 2
    }

    void assignerTouchesPersonnage(GameObject personnageJoueur, int layerPersonnage) // Méthode pour assigner toutes les touches du personnage à partir des données des joueurs
    {
        scriptPersonnageJoueur = personnageJoueur.GetComponent<classe_personnage>();


        scriptAttaqueBaseJoueur = personnageJoueur.GetComponentInChildren<classe_attaque_base>();


        scriptAttaqueSpécialeJoueur = personnageJoueur.GetComponentInChildren<classe_attaque_spéciale>();


        if (layerPersonnage == 6) //si c'est le joueur 1
        {
            scriptPersonnageJoueur.assignerTouches(données_joueurs.toucheDroite1, données_joueurs.toucheGauche1, données_joueurs.toucheSaut1, données_joueurs.toucheDashBas1);
            scriptAttaqueBaseJoueur.assignerTouches(données_joueurs.toucheAttaqueBase1);
            scriptAttaqueSpécialeJoueur.assignerTouches(données_joueurs.toucheAttaqueSpéciale1);
        }
        if (layerPersonnage == 7) //si c'est le joueur 2
        {
            scriptPersonnageJoueur.assignerTouches(données_joueurs.toucheDroite2, données_joueurs.toucheGauche2, données_joueurs.toucheSaut2, données_joueurs.toucheDashBas2);
            scriptAttaqueBaseJoueur.assignerTouches(données_joueurs.toucheAttaqueBase2);
            scriptAttaqueSpécialeJoueur.assignerTouches(données_joueurs.toucheAttaqueSpéciale2);
        }

    }
    
}

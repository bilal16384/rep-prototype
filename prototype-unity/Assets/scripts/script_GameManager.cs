using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class script_GameManager : MonoBehaviour
{
    //attributs du GameManager

    [SerializeField] private GameObject menuFinDePartie; // Référence au menu de fin de partie
    [SerializeField] private Texture iconeJoueur1; // Icône du personnage 1
    [SerializeField] private Texture iconeJoueur2; // Icône du personnage 2
    [SerializeField] private RawImage imageGagnant; // Image affichant le personnage gagnant
    [SerializeField] private TextMeshProUGUI nomGagnant; // Texte affichant le gagnant de la partie
    [SerializeField] private TextMeshProUGUI titreGagnant; // Texte affichant le titre du gagnant de la partie

    [SerializeField] private string nomJoueur1 = "Joueur 1";
    [SerializeField] private string nomJoueur2 = "Joueur 2";

    // Référence aux données des joueurs
    [SerializeField] private Données_joueurs données_joueurs;


    // Référence aux scripts des personnages
    protected classe_personnage scriptPersonnageJoueur;
    protected classe_attaque_base scriptAttaqueBaseJoueur;
    protected classe_attaque_spéciale scriptAttaqueSpécialeJoueur;
    

   
    
    

    //attribus des personnages
    protected int personnageChoisi1; // Indique le personnage choisi par le joueur 1
    protected int personnageChoisi2; // Indique le personnage choisi par le joueur 2

    [SerializeField] protected GameObject personnage1; // Référence au prefab du personnage 1
    [SerializeField] protected GameObject personnage2; // Référence au prefab du personnage 2


    protected GameObject personnageJoueur1; // Référence au personnage du joueur 1
    protected GameObject personnageJoueur2; // Référence au personnage du joueur 2

    protected BoxCollider2D boxCollider1; // BoxCollider2D du personnage du joueur 1
    protected BoxCollider2D boxCollider2; // BoxCollider2D du personnage du joueur 2

    protected classe_personnage scriptPersonnageJoueur1; // Script du personnage du joueur 1
    protected classe_personnage scriptPersonnageJoueur2; // Script du personnage du joueur 2
    protected classe_attaque_base scriptAttaqueBaseJoueur1; // Script de l'attaque de base du joueur 1
    protected classe_attaque_base scriptAttaqueBaseJoueur2; // Script de l'attaque de base du joueur 2

    protected classe_attaque_spéciale scriptAttaqueSpécialeJoueur1; // Script de l'attaque spéciale du joueur 1
    protected classe_attaque_spéciale scriptAttaqueSpécialeJoueur2; // Script de l'attaque spéciale du joueur 2

    // Visuel des joueurs
    [SerializeField] protected classe_visuel_joueur scriptVisuelJoueurJoueur1;
    [SerializeField] protected classe_visuel_joueur scriptVisuelJoueurJoueur2;

    
    [SerializeField] protected Vector3 positionInitialeJoueur1; // Position initiale du personnage du joueur 1
    [SerializeField] protected Vector3 positionInitialeJoueur2; // Position initiale du personnage du joueur 2
    [SerializeField] protected Vector3 positionRéapparition; // Position de réapparition des personnages après leur élimination

    //attribus de partie
    protected int duréePartie = 120; // Durée de la partie en secondes
    protected float tempsRestant; // Temps restant de la partie en secondes
    protected int tempsAffiché;
    protected bool enElimination = false; // Indique si la partie est en phase d'élimination après le temps imparti
    protected bool enFinDePartie = false; // Indique si la partie est terminée

    [SerializeField] protected TextMeshProUGUI texteTempsRestant; // Référence au texte affichant le temps restant de la partie

    



    protected void Awake() // Méthode appelée lors de l'initialisation du GameManager
    {
        // Initialisation du GameManager
        menuFinDePartie.SetActive(false); // Masque le menu de fin de partie au démarrage

        
        // Instanciation des personnages
        if (donnéesDePartie.Instance != null) // Vérifie si l'instance des données de partie existe avant de continuer
        {
            personnageChoisi1 = donnéesDePartie.Instance.obtenirPersonnageChoisi1(); // Récupère le personnage choisi par le joueur 1
            personnageChoisi2 = donnéesDePartie.Instance.obtenirPersonnageChoisi2(); // Récupère le personnage choisi par le joueur 2
            instancierPersonnages(personnageChoisi1, personnageChoisi2, personnage1, personnage2); // Instancie les personnages en fonction des choix des joueurs
            boxCollider1 = personnageJoueur1.GetComponent<BoxCollider2D>(); // Récupère le BoxCollider2D du personnage du joueur 1
            boxCollider2 = personnageJoueur2.GetComponent<BoxCollider2D>(); // Récupère le BoxCollider2D du personnage du joueur 2
            scriptPersonnageJoueur1 = personnageJoueur1.GetComponent<classe_personnage>(); // Récupère le script du personnage du joueur 1
            scriptPersonnageJoueur2 = personnageJoueur2.GetComponent<classe_personnage>(); // Récupère le script du personnage du joueur 2
            scriptAttaqueBaseJoueur1 = personnageJoueur1.GetComponentInChildren<classe_attaque_base>(); // Récupère le script de l'attaque de base du joueur 1
            scriptAttaqueBaseJoueur2 = personnageJoueur2.GetComponentInChildren<classe_attaque_base>(); // Récupère le script de l'attaque de base du joueur 2
            scriptAttaqueSpécialeJoueur1 = personnageJoueur1.GetComponentInChildren<classe_attaque_spéciale>(); // Récupère le script de l'attaque spéciale du joueur 1
            scriptAttaqueSpécialeJoueur2 = personnageJoueur2.GetComponentInChildren<classe_attaque_spéciale>(); // Récupère le script de l'attaque spéciale du joueur 2
            initialiserPersonnage(personnageJoueur1, positionInitialeJoueur1, boxCollider2, 6, scriptVisuelJoueurJoueur1); // 6 est le layer personnage_1
            initialiserPersonnage(personnageJoueur2, positionInitialeJoueur2, boxCollider1, 7, scriptVisuelJoueurJoueur2); // 7 est le layer personnage_2

            // Assignation des touches aux personnages
            assignerTouchesPersonnage(personnageJoueur1, 6); // Assigne les touches au personnage du joueur 1
            assignerTouchesPersonnage(personnageJoueur2, 7); // Assigne les touches au personnage du joueur 2
            // Assignation des noms aux personnages
            scriptPersonnageJoueur1.mettreAJourNom(nomJoueur1);
            scriptPersonnageJoueur2.mettreAJourNom(nomJoueur2);
            
        
        }

        
        
        
        
    }

    // Méthode Start appelée une fois avant la première exécution de Update
    void Start()
    {
        tempsRestant = duréePartie; // Initialise le temps restant à la durée de la partie
        tempsAffiché = duréePartie;
    }

    // Méthode Update appelée une fois par frame
    void Update()
    {
        if (enFinDePartie)
        {
            return; // Ne fait rien si la partie est déjà terminée
        }
        
        if (tempsRestant > 0)
        {
            tempsRestant -= Time.deltaTime; // Décrémente le temps restant en fonction du temps écoulé depuis la dernière frame
            tempsAffiché = Mathf.CeilToInt(tempsRestant); // Initialise le temps affiché à la valeur entière du temps restant
        }
        texteTempsRestant.text = tempsAffiché.ToString(); // Met à jour le texte affichant le temps restant
        if (tempsRestant <= 0 || scriptPersonnageJoueur1.obtenirEstEliminé() || scriptPersonnageJoueur2.obtenirEstEliminé()) // Vérifie si le temps est écoulé ou si l'un des personnages est éliminé
        {
            if (tempsRestant <= 0) // Vérifie si le temps est écoulé
            {
                mettreEnElimination(scriptPersonnageJoueur1, scriptAttaqueBaseJoueur1, scriptAttaqueSpécialeJoueur1); // Met le personnage du joueur 1 en élimination
                mettreEnElimination(scriptPersonnageJoueur2, scriptAttaqueBaseJoueur2, scriptAttaqueSpécialeJoueur2); // Met le personnage du joueur 2 en élimination
                enElimination = true;
            }
            if (scriptPersonnageJoueur1.obtenirEstEliminé() || scriptPersonnageJoueur2.obtenirEstEliminé()) // Vérifie si l'un des personnages est éliminé
            {
                finDePartie(); // Appelle la méthode pour gérer la fin de la partie lorsque l'un des personnages est éliminé
            }


            
        }


        if (enElimination) // Vérifie si la phase d'élimination est en cours
        {
            infligerDégâtsAuxPersonnages(1); // Inflige des dégâts aux personnages pendant la phase d'élimination pour déterminer le vainqueur

        }
    }


    void initialiserPersonnage(GameObject personnageJoueur, Vector3 positionInitiale, BoxCollider2D boxColliderEnnemi, int layerPersonnage, classe_visuel_joueur visuelJoueur) // Méthode pour initialiser un personnage avec ses paramètres de position, collider, layer et visuel
    {
        personnageJoueur.GetComponent<classe_personnage>().initialiserPersonnage(positionInitiale, boxColliderEnnemi, layerPersonnage, visuelJoueur);
        if (layerPersonnage == 6) personnageJoueur1 = personnageJoueur; // Assigne le personnage du joueur 1 si le layer correspond
        if (layerPersonnage == 7) personnageJoueur2 = personnageJoueur; // Assigne le personnage du joueur 2 si le layer correspond
    }

    void instancierPersonnages(int choix1, int choix2, GameObject personnage1, GameObject personnage2)
    {
        if (choix1 == 1)
        {
            personnageJoueur1 = Instantiate(personnage1); // Instancie le personnage du joueur 1 en fonction du choix du joueur
        }
        else if (choix1 == 2)
        {
            personnageJoueur1 = Instantiate(personnage2); // Instancie le personnage du joueur 1 en fonction du choix du joueur
        }
        if (choix2 == 1)
        {
            personnageJoueur2 = Instantiate(personnage1); // Instancie le personnage du joueur 2 en fonction du choix du joueur
        }
        else if (choix2 == 2)
        {
            personnageJoueur2 = Instantiate(personnage2); // Instancie le personnage du joueur 2 en fonction du choix du joueur
        }

        personnageJoueur1.layer = 6; // Assigne le layer du personnage du joueur 1 pour la détection des collisions et autres interactions
        personnageJoueur2.layer = 7; // Assigne le layer du personnage du joueur 2 pour la détection des collisions et autres interactions
    }

    void assignerTouchesPersonnage(GameObject personnageJoueur, int layerPersonnage) // Méthode pour assigner toutes les touches du personnage à partir des données des joueurs
    {
        scriptPersonnageJoueur = personnageJoueur.GetComponent<classe_personnage>(); // Récupère le script du personnage pour pouvoir lui assigner des touches et accéder à ses méthodes


        scriptAttaqueBaseJoueur = personnageJoueur.GetComponentInChildren<classe_attaque_base>(); // Récupère le script de l'attaque de base pour pouvoir lui assigner des touches


        scriptAttaqueSpécialeJoueur = personnageJoueur.GetComponentInChildren<classe_attaque_spéciale>(); // Récupère le script de l'attaque spéciale pour pouvoir lui assigner des touches


        if (layerPersonnage == 6) //si c'est le joueur 1
        {
            // Assigne les touches du joueur 1 aux scripts correspondants
            scriptPersonnageJoueur.assignerTouches(données_joueurs.toucheDroite1, données_joueurs.toucheGauche1, données_joueurs.toucheSaut1, données_joueurs.toucheDashBas1);
            scriptAttaqueBaseJoueur.assignerTouches(données_joueurs.toucheAttaqueBase1);
            scriptAttaqueSpécialeJoueur.assignerTouches(données_joueurs.toucheAttaqueSpéciale1);
        }
        if (layerPersonnage == 7) //si c'est le joueur 2
        {
            // Assigne les touches du joueur 2 aux scripts correspondants
            scriptPersonnageJoueur.assignerTouches(données_joueurs.toucheDroite2, données_joueurs.toucheGauche2, données_joueurs.toucheSaut2, données_joueurs.toucheDashBas2);
            scriptAttaqueBaseJoueur.assignerTouches(données_joueurs.toucheAttaqueBase2);
            scriptAttaqueSpécialeJoueur.assignerTouches(données_joueurs.toucheAttaqueSpéciale2);
        }
    }
    private void finDePartie() // Méthode appelée à la fin de la partie pour déterminer le gagnant et afficher le menu de fin de partie
    {
        enFinDePartie = true;
        enElimination = false;

        if (scriptPersonnageJoueur1.obtenirEstEliminé() && !scriptPersonnageJoueur2.obtenirEstEliminé()) // Si le joueur 1 est éliminé et que le joueur 2 ne l'est pas
        {
            Debug.Log("Le joueur 2 a gagné !");
            donnéesDePartie.Instance.définirGagnant(2); // Assigne le joueur 2 comme gagnant
        }
        else if (scriptPersonnageJoueur2.obtenirEstEliminé() && !scriptPersonnageJoueur1.obtenirEstEliminé()) // Si le joueur 2 est éliminé et que le joueur 1 ne l'est pas
        {
            Debug.Log("Le joueur 1 a gagné !");
            donnéesDePartie.Instance.définirGagnant(1); // Assigne le joueur 1 comme gagnant
        }
        else if (scriptPersonnageJoueur1.obtenirEstEliminé() && scriptPersonnageJoueur2.obtenirEstEliminé()) // Si les deux joueurs sont éliminés
        {
            Debug.Log("Match nul !");
            donnéesDePartie.Instance.définirGagnant(0); // Assigne 0 pour indiquer un match nul
        }
        afficherMenuFinDePartie(); // Affiche le menu de fin de partie
        mettreÀJourPersonnageGagnant(); // Met à jour l'icône du personnage gagnant à la fin de la partie
    }
    private void mettreEnElimination(classe_personnage personnageJoueur, classe_attaque_base attaqueBase, classe_attaque_spéciale attaqueSpéciale) // Méthode pour mettre un personnage en phase d'élimination
    {
        personnageJoueur.mettreEnElimination();
        attaqueBase.mettreEnElimination();
        attaqueSpéciale.mettreEnElimination();
    }


    private void infligerDégâtsAuxPersonnages(int dégâts) // Méthode pour infliger des dégâts aux deux personnages
    {
        scriptPersonnageJoueur1.prendreDégâts(dégâts);
        scriptPersonnageJoueur2.prendreDégâts(dégâts);
    }



    // Méthode pour afficher le menu de fin de partie
    private void afficherMenuFinDePartie() // Méthode pour afficher le menu de fin de partie
    {
        menuFinDePartie.SetActive(true);
    }
    
    // Méthode pour cacher le menu de fin de partie
    private void cacherMenuFinDePartie() // Méthode pour cacher le menu de fin de partie
    {
        menuFinDePartie.SetActive(false);
    }

    // Méthode pour charger la scène du menu principal
    public void retournerAuMenuPrincipal() // Méthode pour retourner au menu principal
    {
        donnéesDePartie.Instance.réinitialiserDonnées(); // Réinitialise les données de la partie avant de retourner au menu principal
        SceneManager.LoadScene("MenuPrincipal"); // Remplacez "MenuPrincipal" par le nom de votre scène de menu principal
    }

    // Méthode pour rejouer la partie
    public void rejouerPartie() // Méthode pour rejouer la partie
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recharge la scène actuelle pour rejouer
    }

    private void mettreÀJourPersonnageGagnant() // Méthode pour mettre à jour l'icône et le nom du personnage gagnant à la fin de la partie
    {
        int gagnant = donnéesDePartie.Instance.obtenirJoueurGagnant(); // Récupère le joueur gagnant de la partie

        switch (gagnant) // Met à jour l'icône du personnage gagnant en fonction du joueur gagnant
        {
            case 1: // Le joueur 1 a gagné
                imageGagnant.texture = iconeJoueur1;
                imageGagnant.enabled = true;
                nomGagnant.text =  nomJoueur1;
                break; // Fin du cas où le joueur 1 a gagné
            case 2: // Le joueur 2 a gagné
                imageGagnant.texture = iconeJoueur2;
                imageGagnant.enabled = true;
                nomGagnant.text = nomJoueur2;
                break; // Fin du cas où le joueur 2 a gagné
            default: // Aucun gagnant ou match nul
                imageGagnant.texture = null;
                imageGagnant.enabled = false;
                nomGagnant.text = "Match nul ! ";
                titreGagnant.text = "GG aux deux !";
                break; // Fin du cas par défaut (aucun gagnant ou match nul)
        }
    }


}

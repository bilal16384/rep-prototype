using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class manager_menu : MonoBehaviour
{
    //attributs
    [SerializeField] GameObject panelMenuPersonnages; // Référence au panneau du menu des personnages

    //touches
    [SerializeField] private Key toucheQuitter; // Touche pour quitter le jeu ou revenir au menu précédent
    [SerializeField] private Key toucheJouer; // Touche pour activer le menu des personnages

    //variables de partie

    private int personnageJoueur1; // Stocke le personnage sélectionné par le joueur 1
    private int personnageJoueur2; // Stocke le personnage sélectionné par le joueur 2
    void Start() // Initialisation du menu au démarrage du jeu
    {
        désactiverMenupersonnages();
        donnéesDePartie.Instance.réinitialiserDonnées(); // Réinitialise les données de la partie au démarrage du menu
    }

    void Update() // Vérifie les entrées clavier à chaque frame pour gérer le menu
    {
        Keyboard clavier = Keyboard.current;
        if (clavier != null)
        {
            if (clavier[toucheQuitter].wasPressedThisFrame)
            {
                if (panelMenuPersonnages.activeSelf) // vérifie si le menu des personnages est actif
                {
                    désactiverMenupersonnages(); // désactive le menu des personnages si actif
                }
            }
            if (clavier[toucheJouer].wasPressedThisFrame)
            {
                activerMenuPersonnages();
            }
        }
    }






    //méthodes pour gérer le menu des personnages
    public void quitterJeu() // Fonction qui sera appelée quand le bouton Quitter est déclenché
    {
        Debug.Log("Fermeture du jeu"); //sert à montrer que le jeu se ferme pendant les tests
        
        Application.Quit(); //quitte le jeu
    }

    public void activerMenuPersonnages() // Fonction qui sera appelée quand le bouton Jouer est déclenché
    {
        panelMenuPersonnages.SetActive(true);
        Debug.Log("menu des personnages activé");
    }
    public void désactiverMenupersonnages() // Désactive le menu des personnages et réinitialise les sélections
    {
        panelMenuPersonnages.SetActive(false);
        Debug.Log("menu des personnages désactivé");
        personnageJoueur1 = -1; // réinitialise le personnage du joueur 1
        personnageJoueur2 = -1; // réinitialise le personnage du joueur 2
    }

    public void selectionnerPersonnageJoueur1(int personnage) // Fonction pour sélectionner le personnage du joueur 1
    {
        personnageJoueur1 = personnage;
    }

    public void selectionnerPersonnageJoueur2(int personnage) // Fonction pour sélectionner le personnage du joueur 2
    {
        personnageJoueur2 = personnage;
    }

    public void démarrerPartie() // Démarre la partie si les deux personnages ont été sélectionnés
    {
        if (personnageJoueur1 != -1 && personnageJoueur2 != -1) // vérifie si les deux personnages ont été sélectionnés
        {
            donnéesDePartie.Instance.définirPersonnagesChoisis(personnageJoueur1, personnageJoueur2);
            SceneManager.LoadScene("Partie"); // Charge la scène de la partie avec les personnages choisis
        }
        else
        {
            Debug.Log("Veuillez sélectionner les personnages pour les deux joueurs avant de démarrer la partie.");
        }
    }

}

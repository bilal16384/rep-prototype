using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class manager_menu : MonoBehaviour
{
    //attributs
    [SerializeField] GameObject panelMenuPersonnages;

    //touches
    [SerializeField] private Key toucheQuitter;
    [SerializeField] private Key toucheJouer;

    //variables de partie

    private int personnageJoueur1;
    private int personnageJoueur2;

    void Update()
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
    public void quitterJeu() //fonction qui sera appelée quand le bouton Quitter est déclenché
    {
        Debug.Log("Fermeture du jeu"); //sert à montrer que le jeu se ferme pendant les tests
        
        Application.Quit(); //quitte le jeu
    }

    public void activerMenuPersonnages() //fonction qui sera appelée quand le bouton Jouer est déclenché
    {
        panelMenuPersonnages.SetActive(true);
        Debug.Log("menu des personnages activé");
    }
    public void désactiverMenupersonnages()
    {
        panelMenuPersonnages.SetActive(false);
        Debug.Log("menu des personnages désactivé");
        personnageJoueur1 = -1; // réinitialise le personnage du joueur 1
        personnageJoueur2 = -1; // réinitialise le personnage du joueur 2
    }

    public void selectionnerPersonnageJoueur1(int personnage) //fonction pour sélectionner le personnage du joueur 1
    {
        personnageJoueur1 = personnage;
        Debug.Log("Personnage joueur 1 sélectionné : " + personnageJoueur1);
    }

    public void selectionnerPersonnageJoueur2(int personnage) //fonction pour sélectionner le personnage du joueur 2
    {
        personnageJoueur2 = personnage;
        Debug.Log("Personnage joueur 2 sélectionné : " + personnageJoueur2);
    }

    public void démarrerPartie()
    {
        Debug.Log("Partie démarrée avec les personnages : Joueur 1 - " + personnageJoueur1 + ", Joueur 2 - " + personnageJoueur2);
        
        donnéesDePartie.Instance.définirPersonnagesChoisis(personnageJoueur1, personnageJoueur2);
        SceneManager.LoadScene("Partie"); // Charge la scène de la partie avec les personnages choisis
    }

}

using UnityEngine;

public class donnéesDePartie : MonoBehaviour
{
    public static donnéesDePartie Instance { get; private set; } // Instance unique de la classe (singleton)
    protected int personnageChoisi1; // Indique le personnage choisi par le joueur 1
    protected int personnageChoisi2; // Indique le personnage choisi par le joueur 2

    protected int joueurGagnant; // Indique le joueur gagnant de la partie
    protected int personnageGagnant; // Indique le personnage gagnant de la partie


    private void Awake() // Initialisation de l'instance unique (singleton)
    {
        if (Instance == null) // Si aucune instance n'existe encore, on initialise celle-ci
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Empêche la destruction lors du changement de scène
        }
        else
        {
            Destroy(gameObject); // Évite les doublons si on revient sur la scène initiale (singleton déjà existant)
        }
    }
    public void définirPersonnagesChoisis(int choix1, int choix2) // Définit les personnages choisis par les deux joueurs
    {
        personnageChoisi1 = choix1;
        personnageChoisi2 = choix2;
    }

    public void définirGagnant(int joueur) // Définit le joueur gagnant et le personnage gagnant en fonction du joueur victorieux
    {
        joueurGagnant = joueur;
        if (joueur == 1)
        {
            personnageGagnant = personnageChoisi1;
        }
        else if (joueur == 2)
        {
            personnageGagnant = personnageChoisi2;
        }
    }


    public int obtenirPersonnageChoisi1() // Retourne le personnage choisi par le joueur 1
    {
        return personnageChoisi1;
    }

    public int obtenirPersonnageChoisi2() // Retourne le personnage choisi par le joueur 2
    {
        return personnageChoisi2;
    }

    public int obtenirJoueurGagnant() // Retourne le joueur gagnant de la partie
    {
        return joueurGagnant;
    }

    public int obtenirPersonnageGagnant() // Retourne le personnage gagnant de la partie
    {
        return personnageGagnant;
    }


    public void réinitialiserDonnées() // Réinitialise toutes les données de la partie (personnages choisis et gagnants)
    {
        personnageChoisi1 = -1;
        personnageChoisi2 = -1;
        joueurGagnant = -1;
        personnageGagnant = -1;
    }

}

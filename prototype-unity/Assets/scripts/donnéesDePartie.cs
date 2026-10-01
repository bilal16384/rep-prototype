using UnityEngine;

public class donnéesDePartie : MonoBehaviour
{
    public static donnéesDePartie Instance { get; private set; } // Instance unique de la classe (singleton)
    protected int personnageChoisi1; // Indique le personnage choisi par le joueur 1
    protected int personnageChoisi2; // Indique le personnage choisi par le joueur 2

    protected int joueurGagnant; // Indique le joueur gagnant de la partie


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Empêche la destruction lors du changement de scène
        }
        else
        {
            Destroy(gameObject); // Évite les doublons si on revient sur la scène initiale
        }
    }
    public void définirPersonnagesChoisis(int choix1, int choix2)
    {
        personnageChoisi1 = choix1;
        personnageChoisi2 = choix2;
    }

    public void définirJoueurGagnant(int gagnant)
    {
        joueurGagnant = gagnant;
    }


    public int obtenirPersonnageChoisi1()
    {
        return personnageChoisi1;
    }

    public int obtenirPersonnageChoisi2()
    {
        return personnageChoisi2;
    }

    public int obtenirJoueurGagnant()
    {
        return joueurGagnant;
    }


    public void réinitialiserDonnées()
    {
        personnageChoisi1 = -1;
        personnageChoisi2 = -1;
        joueurGagnant = -1;
    }

}

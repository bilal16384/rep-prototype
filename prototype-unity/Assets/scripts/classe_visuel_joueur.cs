using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class classe_visuel_joueur : MonoBehaviour
{



    // Attributs pour les barres de vie et de recharge des attaques
    [SerializeField] private Slider sliderBarreDeVie; // Barre de vie du joueur
    [SerializeField] private Slider sliderRechargeAttaqueBase; // Barre de recharge de l'attaque de base
    [SerializeField] private Slider sliderRechargeAttaqueSpeciale; // Barre de recharge de l'attaque spéciale
    [SerializeField] private TextMeshProUGUI texteNomJoueur; // Texte affichant le nom du joueur
    [SerializeField] private TextMeshProUGUI texteNombreVies; // Texte affichant le nombre de vies du joueur

    

    private float valeurRechargeAttaqueBase; // Durée de recharge de l'attaque de base
    private float valeurRechargeAttaqueSpeciale; // Durée de recharge de l'attaque spéciale

    protected virtual void Start() // S'exécute une fois avant la première frame
    {
        // Initialisation des barres de vie et de recharge des attaques
        sliderBarreDeVie.value = 1;
        sliderRechargeAttaqueBase.value = 1;
        sliderRechargeAttaqueSpeciale.value = 1;
    }



    protected virtual void Update() // S'exécute à chaque frame
    {
        // Mise à jour des barres de recharge des attaques en fonction du temps écoulé

        if (sliderRechargeAttaqueBase.value < 1) 
        {
            sliderRechargeAttaqueBase.value = sliderRechargeAttaqueBase.value + Time.deltaTime / valeurRechargeAttaqueBase;
            if (sliderRechargeAttaqueBase.value > 1)
                sliderRechargeAttaqueBase.value = 1;
        }
        if (sliderRechargeAttaqueSpeciale.value < 1)
        {
            sliderRechargeAttaqueSpeciale.value = sliderRechargeAttaqueSpeciale.value + Time.deltaTime / valeurRechargeAttaqueSpeciale;
            if (sliderRechargeAttaqueSpeciale.value > 1)
                sliderRechargeAttaqueSpeciale.value = 1;
        }
    }

    // Méthodes pour mettre à jour les éléments visuels du joueur
    public void MettreAJourBarreDeVie(float valeur, int valeurMax) // Met à jour la barre de vie du joueur en fonction de la valeur actuelle et de la valeur maximale
    {
        sliderBarreDeVie.value = ((float)valeur/(float)valeurMax);
    }

    public void ViderRechargeAttaqueBase(float valeur) // Vide la barre de recharge de l'attaque de base et initialise sa durée de recharge
    {
        valeurRechargeAttaqueBase = valeur;
        sliderRechargeAttaqueBase.value = 0;
    }

    public void ViderRechargeAttaqueSpeciale(float valeur) // Vide la barre de recharge de l'attaque spéciale et initialise sa durée de recharge
    {
        valeurRechargeAttaqueSpeciale = valeur;
        sliderRechargeAttaqueSpeciale.value = 0;
    }

    public void MettreAJourNomJoueur(string nom) // Met à jour le nom du joueur affiché
    {
        texteNomJoueur.text = nom;
    }

    public void MettreAJourNombreVies(int nombreVies) // Met à jour le nombre de vies du joueur affiché
    {
        texteNombreVies.text = nombreVies.ToString();
    }
}

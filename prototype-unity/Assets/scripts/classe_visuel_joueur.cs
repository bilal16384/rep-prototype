using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class classe_visuel_joueur : MonoBehaviour
{



    // Attributs pour les barres de vie et de recharge des attaques
    [SerializeField] private Slider sliderBarreDeVie;
    [SerializeField] private Slider sliderRechargeAttaqueBase;
    [SerializeField] private Slider sliderRechargeAttaqueSpeciale;
    [SerializeField] private TextMeshProUGUI texteNomJoueur;
    [SerializeField] private TextMeshProUGUI texteNombreVies;

    

    private float valeurRechargeAttaqueBase;
    private float valeurRechargeAttaqueSpeciale;

    protected virtual void Start() // S'exécute une fois avant la première frame
    {
        sliderBarreDeVie.value = 1;
        sliderRechargeAttaqueBase.value = 1;
        sliderRechargeAttaqueSpeciale.value = 1;
    }



    protected virtual void Update() // S'exécute à chaque frame
    {
        // Mise à jour des barres de recharge des attaques

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
    public void MettreAJourBarreDeVie(float valeur, int valeurMax) // Valeur entre 0 et 1
    {
        sliderBarreDeVie.value = ((float)valeur/(float)valeurMax);
    }

    public void ViderRechargeAttaqueBase(float valeur) 
    {
        valeurRechargeAttaqueBase = valeur;
        sliderRechargeAttaqueBase.value = 0;
    }

    public void ViderRechargeAttaqueSpeciale(float valeur) 
    {
        valeurRechargeAttaqueSpeciale = valeur;
        sliderRechargeAttaqueSpeciale.value = 0;
    }

    public void MettreAJourNomJoueur(string nom)
    {
        texteNomJoueur.text = nom;
    }

    public void MettreAJourNombreVies(int nombreVies) // Nombre de vies du joueur
    {
        texteNombreVies.text = nombreVies.ToString();
    }
}

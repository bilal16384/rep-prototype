using UnityEngine;

public class script_GameManager : MonoBehaviour
{
    //attributs du GameManager
    [SerializeField] protected GameObject Personnage1; // Référence au prefab du personnage du joueur 1
    [SerializeField] protected GameObject Personnage2; // Référence au prefab du personnage du joueur 2

    protected int duréePartie = 300; // Durée de la partie en secondes
    protected float tempsRestant; // Temps restant de la partie en secondes
    protected int tempsAffiché;

    protected GameObject personnageJoueur1; // Référence au personnage du joueur 1
    protected GameObject personnageJoueur2; // Référence au personnage du joueur 2
    protected Vector3 positionInitialeJoueur1; // Position initiale du personnage du joueur 1
    protected Vector3 positionInitialeJoueur2; // Position initiale du personnage du joueur 2
    public Vector3 positionRéapparition; // Position de réapparition des personnages après leur élimination


    protected void Awake()
    {
        // Initialisation du GameManager
        personnageJoueur1 = Instantiate(Personnage1); // Instancie le personnage du joueur 1 à partir du prefab

        personnageJoueur2 = Instantiate(Personnage2); // Instancie le personnage du joueur 2 à partir du prefab

        personnageJoueur1.GetComponent<classe_personnage>().initialiserPersonnage(positionInitialeJoueur1);
        personnageJoueur2.GetComponent<classe_personnage>().initialiserPersonnage(positionInitialeJoueur2);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tempsRestant = duréePartie; // Initialise le temps restant à la durée de la partie
        tempsAffiché = duréePartie;
    }

    // Update is called once per frame
    void Update()
    {
        tempsRestant -= Time.deltaTime; // Décrémente le temps restant en fonction du temps écoulé depuis la dernière frame

        tempsAffiché = Mathf.CeilToInt(tempsRestant); // Initialise le temps affiché à la valeur entière du temps restant
    }
}

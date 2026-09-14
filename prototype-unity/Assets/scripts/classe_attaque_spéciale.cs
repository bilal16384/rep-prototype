using UnityEngine;
using UnityEngine.InputSystem;

public class classe_attaque_spéciale : classe_attaque
{
    //attributs des attaques spéciales
    [SerializeField] protected float rechargeAttaqueSpéciale;
    //touches
    [SerializeField] protected Key toucheAttaqueSpeciale;


    protected float duréeAttaqueSpéciale = 0.05f;
    protected float tempsDernièreAttaqueSpéciale = 0f;
    protected bool enAttaqueSpéciale = false;

    //script du personnage
    protected classe_personnage scriptPersonnage;

    protected override void Awake()
    {
        base.Awake();
        scriptPersonnage = transform.parent.GetComponent<classe_personnage>();
        Debug.Log("script_personnage trouvé sur le parent de" + transform.name + " : " + scriptPersonnage.name);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        enAttaqueSpéciale = false;
        tempsDernièreAttaqueSpéciale = -(rechargeAttaqueSpéciale); // permet d'attaquer dès le début du jeu
        if(layerPersonnageEnnemi == 0)
        {
            définirLayerPersonnageEnnemi(gameObject.layer); // Définit le layer du personnage ennemi en fonction du layer du personnage actuel
            Debug.Log("Layer du personnage ennemi de "+ gameObject.name +" défini sur : " + LayerMask.LayerToName(layerPersonnageEnnemi));
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        Keyboard clavier = Keyboard.current;
        if (clavier != null)
        {
            if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame && Time.time - tempsDernièreAttaqueSpéciale >= rechargeAttaqueSpéciale)
            {
                attaqueSpéciale();
                tempsDernièreAttaqueSpéciale = Time.time;
            }
            else if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame)
            {
                Debug.Log("Attaque spéciale en recharge. Temps restant : " + (rechargeAttaqueSpéciale - (Time.time - tempsDernièreAttaqueSpéciale)) + " secondes.");
            }
        }
    
    }



    protected virtual void OnTriggerStay2D(Collider2D collision)
    {

    }

    protected virtual void attaqueSpéciale()
    {
        enAttaqueSpéciale = true;
        Debug.Log("Attaque spéciale effectuée !");
    }
    protected virtual void finAttaqueSpéciale()
    {
        enAttaqueSpéciale = false;
        Debug.Log("Fin de l'attaque spéciale.");
    }



}

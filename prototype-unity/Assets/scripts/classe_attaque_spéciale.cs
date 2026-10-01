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
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        this.gameObject.layer = transform.parent.gameObject.layer; // Assigne le layer du GameObject de l'attaque au même layer que le parent (personnage)
        enAttaqueSpéciale = false;
        tempsDernièreAttaqueSpéciale = -(rechargeAttaqueSpéciale); // permet d'attaquer dès le début du jeu
        if(layerPersonnageEnnemi == 0)
        {
            définirLayerPersonnageEnnemi(gameObject.layer); // Définit le layer du personnage ennemi en fonction du layer du personnage actuel
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

    public virtual void assignerTouches(Key toucheAttaqueSpéciale)
    {
        this.toucheAttaqueSpeciale = toucheAttaqueSpéciale;
    }

}

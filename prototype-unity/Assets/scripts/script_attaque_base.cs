using UnityEngine;
using UnityEngine.InputSystem;


public class script_attaque_base : MonoBehaviour
{

    //hitboxes
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxColliderEnnemi;
    [SerializeField] private script_points_vie pointsVieScriptEnnemi;

    //touches
    public Key toucheAttaqueBase;


    //variables d'attaque
    public float dégâtsAttaqueBase;
    public float rechargeAttaqueBase = 0.2f;
    private float duréeAttaqueBase = 0.05f;
    private float tempsDernièreAttaque = 0f;
    public bool enAttaque = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enAttaque = false;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        //attaque de base
        Keyboard clavier = Keyboard.current;
        if (clavier != null)
        {
            if (clavier[toucheAttaqueBase].wasPressedThisFrame && Time.time - tempsDernièreAttaque >= rechargeAttaqueBase)
            {
                attaqueBase();
                tempsDernièreAttaque = Time.time;
                
            }
            if (enAttaque)
            {
                if (Time.time - tempsDernièreAttaque >= duréeAttaqueBase)
                {
                    finAttaqueBase();
                }
            }
            if (enAttaque && boxCollider.IsTouching(boxColliderEnnemi))
            {
                pointsVieScriptEnnemi.pointsVieActuels -= dégâtsAttaqueBase;
                Debug.Log("Points de vie de l'ennemi : " + pointsVieScriptEnnemi.pointsVieActuels);
                enAttaque = false; // Réinitialise l'état d'attaque après avoir infligé des dégâts
            }


        }
    }




    void attaqueBase()
    {
        enAttaque = true;
        Debug.Log("Attaque de base effectuée !");
    }

    void finAttaqueBase()
    {
        enAttaque = false;
        Debug.Log("Fin de l'attaque de base.");
    }

}

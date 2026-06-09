using UnityEngine;
using UnityEngine.InputSystem;


public class script_attaque_base : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    public Key toucheAttaqueBase;
    public float dégâtsAttaqueBase = 5f;
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

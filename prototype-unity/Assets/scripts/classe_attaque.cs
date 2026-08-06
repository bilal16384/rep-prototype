using UnityEngine;
using UnityEngine.InputSystem;

public class classe_attaque : MonoBehaviour
{
    //attribus de l'attaque de base

    [SerializeField] protected int dégâtsAttaqueBase;
    [SerializeField] protected float rechargeAttaqueBase;

    //touches
    [SerializeField] protected Key toucheAttaqueBase;
    [SerializeField] protected Key toucheAttaqueSpeciale;

    //variables d'attaque
    protected float duréeAttaqueBase = 0.05f;
    protected float duréeAttaqueSpeciale = 0.05f;
    protected bool enAttaque = false;
    protected float tempsDernièreAttaque = 0f;

    //hitbox
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb;






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
        }

        if (enAttaque)
        {
            if (Time.time - tempsDernièreAttaque >= duréeAttaqueBase)
            {
                finAttaqueBase();
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



    void onTriggerEnter2D(Collider2D collider2D)
    {
        
    }


    void infligerDégâts(GameObject cible, int dégâts)
    {
        // Logique pour infliger des dégâts à la cible
        
        Debug.Log($"Inflige {dégâts} points de dégâts à {cible.name} !");
    }
}

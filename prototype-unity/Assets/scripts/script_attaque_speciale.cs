using UnityEngine;
using UnityEngine.InputSystem;

public class script_attaque_speciale : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxColliderEnnemi;
    [SerializeField] private script_points_vie pointsVieScriptEnnemi;



    [SerializeField] private Key toucheAttaqueSpeciale;


    public float dégâtsAttaqueSpeciale;
    public float dégâtsProjectile;
    public float rechargeAttaqueSpeciale = 1f;
    private float duréeAttaqueSpeciale = 0.1f;
    private float tempsDernièreAttaque = 0f;
    
    private bool enAttaque = false;


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
            if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame && 
            Time.time - tempsDernièreAttaque >= rechargeAttaqueSpeciale)
            {
                attaqueSpeciale();
                tempsDernièreAttaque = Time.time;
            }
            if (enAttaque)
            {
                if (Time.time - tempsDernièreAttaque >= duréeAttaqueSpeciale)
                {
                    finAttaqueSpeciale();
                    attaqueProjectile();
                }
            }









        }
    }
    void attaqueSpeciale()
    {
        enAttaque = true;
        // Logique pour l'attaque spéciale



        Debug.Log("Attaque spéciale effectuée !");
    }
    void finAttaqueSpeciale()
    {
        enAttaque = false;
        Debug.Log("Fin de l'attaque spéciale.");
    }
    void attaqueProjectile()
    {
        // Logique pour l'attaque projectile
        Debug.Log("Attaque projectile effectuée !");
    }


}

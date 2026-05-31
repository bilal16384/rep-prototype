using System.Numerics;
using Unity.Collections;
using UnityEditor.Callbacks;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class script_move : MonoBehaviour
{
    
    
    
    


    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;




    private bool regardeDroite = true;
    private float longueurSaut;
    private bool estEnSaut = false;



    //touches
    public Key toucheDroite = Key.D;
    public Key toucheGauche = Key.A;
    
    
    
    public float valeur_longueurSaut;
    public Key toucheSaut = Key.W;
    public float forceSaut;
    public float vitesse;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new UnityEngine.Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        longueurSaut = valeur_longueurSaut;
    }
    // Update is called once per frame
    void Update()
    {
        Keyboard clavier = Keyboard.current;
        
        // Vérifie si le clavier est disponible avant de lire les entrées
        if (clavier != null)
        {


            //déplacement horizontal
            float moveX = 0;
            if (clavier[toucheDroite].isPressed) moveX = 1;
            if (clavier[toucheGauche].isPressed) moveX = -1;
            
            rb.linearVelocity = new UnityEngine.Vector2(moveX * vitesse, rb.linearVelocity.y);
            
            



            //direction du personnage
            if (moveX > 0 && !regardeDroite)
            {
                regardeDroite = true;
                transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            }
            else if (moveX < 0 && regardeDroite)
            {
                regardeDroite = false;
                transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
            }




            // saut
            if (clavier[toucheSaut].wasPressedThisFrame && detectersol() == true)
            {
                rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, forceSaut);
                estEnSaut = true;
                
            }

            if (estEnSaut == true && clavier [toucheSaut].isPressed)
            {
                rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, forceSaut);
                longueurSaut -= Time.deltaTime;
                if (longueurSaut <= 0)
                {
                    estEnSaut = false;
                    longueurSaut = valeur_longueurSaut;
                
                }
            }
        
            else if (estEnSaut == true && clavier[toucheSaut].wasReleasedThisFrame)
            {
                estEnSaut = false;
                longueurSaut = valeur_longueurSaut;
            }
        }    
    }
        
    bool detectersol()
    {
        UnityEngine.Vector2 tailleReduite = new UnityEngine.Vector2(boxCollider.bounds.size.x * 0.8f, boxCollider.bounds.size.y);
        
        RaycastHit2D detecterSol = Physics2D.BoxCast(
            boxCollider.bounds.center,
            tailleReduite,
            0f,
            UnityEngine.Vector2.down,
            0.1f,
            UnityEngine.LayerMask.GetMask("Terrain")
        );
        if (detecterSol.collider != null)
        {
            return true;
        }
        return false;
    }


}

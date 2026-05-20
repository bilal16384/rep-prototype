using System.Numerics;
using Unity.Collections;
using UnityEditor.Callbacks;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class script_move : MonoBehaviour
{
    
    
    
    
    public float vitesse;

    private Rigidbody2D rb;

    private BoxCollider2D boxCollider;

    private bool regardeDroite = true;


    




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new UnityEngine.Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody2D>();
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
            if (clavier.dKey.isPressed) moveX = 1;
            if (clavier.aKey.isPressed) moveX = -1;
            
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
        }    
            
        


    }
}

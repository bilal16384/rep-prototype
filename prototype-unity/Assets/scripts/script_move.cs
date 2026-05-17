using System.Numerics;
using Unity.Collections;
using UnityEditor.Callbacks;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class script_move : MonoBehaviour
{
    
    
    
    
    public float vitesse;
    public float forceSaut;
    private Rigidbody2D rb;

    public float valeur_longueurSaut;
    private float longueurSaut;
    private bool estEnSaut = false;
    private bool regardeDroite = true;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new UnityEngine.Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody2D>();
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
            if (clavier.dKey.isPressed) moveX = 1;
            if (clavier.aKey.isPressed) moveX = -1;
            
            transform.position += new UnityEngine.Vector3(moveX, 0, 0) * vitesse * Time.deltaTime;

            
            
            
            //saut
            if (clavier.spaceKey.wasPressedThisFrame)
            {
                rb.linearVelocity = new UnityEngine.Vector2(0, forceSaut);
                estEnSaut = true;
            } 
            
            if (estEnSaut == true && clavier.spaceKey.isPressed)
            {
                rb.linearVelocity = new UnityEngine.Vector2(0, forceSaut);
                longueurSaut -= Time.deltaTime;
                if (longueurSaut <= 0)
                {
                    estEnSaut = false;
                    longueurSaut = valeur_longueurSaut;
                }
            }
            else if (estEnSaut == true && clavier.spaceKey.wasReleasedThisFrame)
            {
                estEnSaut = false;
                longueurSaut = valeur_longueurSaut;
            }



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
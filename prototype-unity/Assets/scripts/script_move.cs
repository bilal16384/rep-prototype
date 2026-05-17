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

    //private bool regardeDroite = true;




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
        
        if (clavier != null)
        {
            float moveX = 0;
            if (clavier.dKey.isPressed) moveX = 1;
            if (clavier.aKey.isPressed) moveX = -1;
            
            transform.position += new UnityEngine.Vector3(moveX, 0, 0) * vitesse * Time.deltaTime;

            if (clavier.spaceKey.wasPressedThisFrame)
            {
                rb.linearVelocity = new UnityEngine.Vector2(0, forceSaut);
            } 
            
            
            
            
        }
    }
}
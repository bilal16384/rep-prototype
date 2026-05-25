using System.Numerics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class script_saut : MonoBehaviour
{

    private Rigidbody2D rb;

    private BoxCollider2D boxCollider;
    public float valeur_longueurSaut;
    private float longueurSaut;
    private bool estEnSaut = false;
    
    public Key toucheSaut = Key.W;
    
    public float forceSaut;
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
        

        

        //saut
        if (clavier != null)
        {
            // vérifie si le personnage est au sol
            if (clavier[toucheSaut].wasPressedThisFrame && detectersol() == true)
            {

                


                rb.linearVelocity = new UnityEngine.Vector2(0, forceSaut);
                estEnSaut = true;
                
            }

            if (estEnSaut == true && clavier [toucheSaut].isPressed)
            {
                rb.linearVelocity = new UnityEngine.Vector2(0, forceSaut);
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
            boxCollider.bounds.size,
            0f, 
            UnityEngine.Vector2.down,
            0.05f,
            UnityEngine.LayerMask.GetMask("Terrain")
        );
        if (detecterSol.collider != null)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null) return;

        // On choisit une couleur rouge transparente
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        
        // On calcule la position où la boîte va chercher (centre du collider + décalage vers le bas)
        float extraHauteur = 0.1f;
        UnityEngine.Vector3 positionBoite = collider.bounds.center + (UnityEngine.Vector3.down * extraHauteur);

        // On dessine la boîte virtuelle dans l'éditeur
        UnityEngine.Vector3 tailleBoite = new UnityEngine.Vector3(collider.bounds.size.x * 0.8f, collider.bounds.size.y, 1f);
        Gizmos.DrawCube(positionBoite, collider.bounds.size);
    }
}








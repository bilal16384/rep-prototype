using UnityEngine;
using UnityEngine.InputSystem;
public class classe_personnage : MonoBehaviour
{



    //attribus des personnages
    [SerializeField] protected float pointsVieMax;


    [SerializeField] protected float vitesse;
    [SerializeField] protected float forceSaut;
    [SerializeField] protected float valeur_longueurSaut;
    [SerializeField] protected Vector3 positionDépart;




    //touches 
    [SerializeField] protected Key toucheDroite;
    [SerializeField] protected Key toucheGauche;   
    [SerializeField] protected Key toucheSaut;
    [SerializeField] protected Key toucheAttaqueBase;



    //variables de jeu
    protected float pointsVieActuels;
    protected bool estMort = false;


    protected bool regardeDroite = true;
    protected float longueurSaut;
    protected bool estEnSaut = false;
    
    
    
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        pointsVieActuels = pointsVieMax;

        transform.position = positionDépart;
        
        longueurSaut = valeur_longueurSaut;
        
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //vérifie si le clavier est disponible avant de lire les entrées
        Keyboard clavier = Keyboard.current;
        
        if (clavier !=null)
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


    protected virtual void mourir()
    {
        estMort = true;
        Debug.Log("Le personnage est mort.");
    }
    
    
    protected virtual void prendreDégâts(float dégâts)
    {
        pointsVieActuels -= dégâts;
        if (pointsVieActuels <= 0 && !estMort)
        {
            mourir();
        }
        else
        {
            Debug.Log("Points de vie actuels : " + pointsVieActuels);
        }
    }



    protected virtual bool detectersol()
    {
        UnityEngine.Vector2 tailleReduite = new UnityEngine.Vector2
        (
            boxCollider.bounds.size.x * 0.8f,
            boxCollider.bounds.size.y
        );
        
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

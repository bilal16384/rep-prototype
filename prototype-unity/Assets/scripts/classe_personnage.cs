using UnityEngine;
using UnityEngine.InputSystem;
public class classe_personnage : MonoBehaviour, In_prendre_dégâts
{



    //attribus des personnages
    [SerializeField] protected string nom;




    //vie
    [SerializeField] protected int pointsVieMax;

    //déplacement
    [SerializeField] protected float vitesse;
    [SerializeField] protected float forceSaut;
    [SerializeField] protected float valeur_longueurSaut;
    [SerializeField] protected Vector3 positionDépart;
    




    //touches 
    [SerializeField] protected Key toucheDroite;
    [SerializeField] protected Key toucheGauche;   
    [SerializeField] protected Key toucheSaut;
    [SerializeField] protected Key toucheDashBas;


    //animation
    [SerializeField] protected Animator animator;



    //variables de jeu

    //vie
    protected float pointsVieActuels;
    protected bool estMort = false;

    //déplacement
    protected bool regardeDroite = true;
    protected float longueurSaut;
    protected bool estEnSaut = false;
    protected int sautRestant = 1; // Nombre de sauts restants (1 pour un double saut) :)
    protected bool peutBouger = true; // Variable pour contrôler si le personnage peut bouger ou non
    //affaiblissement
    protected float duréeAffaiblissement = 0; // Variable pour stocker la durée de l'affaiblissement en secondes
    protected float tempsAffaibli = 0; // Variable pour stocker le temps écoulé depuis le début de l'affaiblissement
    protected float estAffaibli = 0f; // Variable pour contrôler à quel point le personnage est affaibli (0 = pas affaibli, 1 = complètement affaibli)
    protected float tempsDernierAffaiblissement = 0; // Variable pour stocker le temps écoulé depuis le dernier affaiblissement
    
    
    //hitbox
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    //hitbox ennemis
    [SerializeField] protected BoxCollider2D boxColliderEnnemi;


    //paramètres match
    protected int nombreVies = 3;

    protected virtual void Awake() // Awake est appelé avant Start, même si le script est désactivé
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(boxCollider, boxColliderEnnemi, true); // Ignore la collision entre le personnage et l'ennemi
    }
    protected virtual void Start() // Start est appelé avant la première image, seulement si le script est activé
    {
        transform.position = positionDépart;   // Position de départ à défiir selon les règles...
        pointsVieActuels = pointsVieMax;
        estMort = false;
        longueurSaut = valeur_longueurSaut;
    }



    protected virtual void Update()
    {
        Keyboard clavier = Keyboard.current;
        
        // Vérifie si le clavier est disponible avant de lire les entrées
        if (clavier != null)
        {


            //déplacement horizontal

            //modifie la valeur de la vitesse horizontale en fonction des touches pressées
            float moveX = 0;
            if (clavier[toucheDroite].isPressed) moveX = 1;
            if (clavier[toucheGauche].isPressed) moveX = -1;
            
            //applique la vitesse horizontale au personnage
            if (peutBouger)
            {
                rb.linearVelocity = new UnityEngine.Vector2(moveX * vitesse, rb.linearVelocity.y); // Applique la vitesse horizontale au personnage
            }
            else
            {
                rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, rb.linearVelocity.y); // Empêche le personnage de bouger pendant le double saut
                if (detectersol())
                {
                    peutBouger = true; // Permet au personnage de bouger à nouveau lorsqu'il touche le sol
                }
            }
            
            //calcul de la vitesse actuelle pour l'animation (valeur absolue de la vitesse horizontale)
            float vitesseActuelle = Mathf.Abs(rb.linearVelocity.x);
            animator.SetFloat("Vitesse", vitesseActuelle);

            



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
            if (detectersol())
            {
                sautRestant = 1; // Réinitialise le nombre de sauts restants lorsqu'il touche le sol
                peutBouger = true; // Permet au personnage de bouger lorsqu'il touche le sol
            }

            if (clavier[toucheSaut].wasPressedThisFrame)
            {
                if (detectersol())
                {
                    rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, forceSaut);
                    estEnSaut = true;
                    
                }
                else if (sautRestant > 0)
                {
                    rb.linearVelocity = new UnityEngine.Vector2((System.Convert.ToInt32(regardeDroite) * 2 - 1) * vitesse, forceSaut * 0.5f); // Applique la vitesse horizontale en fonction de la direction du personnage
                    peutBouger = false; // Empêche le personnage de bouger pendant le double saut
                    sautRestant--; // Décrémente le nombre de sauts restants
                }
            }
            //dash vers le bas
            if(clavier[toucheDashBas].wasPressedThisFrame)
            {
                if(detectersol() == false) //vérifie que le personnage soit en l'air avant d'effectuer le dash vers le bas
                {
                    peutBouger = true; // Permet au personnage de bouger à nouveau après le dash vers le bas
                    rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, -forceSaut);
                }



            }


            
            //Prolongation du premier saut si la touche de saut est maintenue enfoncée
            if (estEnSaut == true && clavier [toucheSaut].isPressed && peutBouger == true)
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
            
            
            //tue le personnage si sa position est trop basse (au cas où il tombe du terrain)
            if(transform.position.y <= -20)
            {
                mourir();
                Debug.Log("Le personnage a été tué car sa position était trop basse");
            }

            //gestion de l'affaiblissement
            if (estAffaibli > 0)
            {
                if (Time.time - tempsDernierAffaiblissement >= duréeAffaiblissement)
                {
                    estAffaibli = 0; // Réinitialise l'affaiblissement lorsque la durée est écoulée
                    Debug.Log("L'affaiblissement du personnage " + nom + " est terminé.");
                }
               
               
            }



        }    
    }


    //méthodes
    protected virtual void mourir()
    {
        estMort = true;
        nombreVies --; //réduit le nombre de vies du personnage de 1 chaque fois qu'il meurt
        Debug.Log(nombreVies);
        if (nombreVies <= 0)
        {
            éliminerPersonnage();
        }
        else
        {
            apparaîtrePersonnage();
            
        }
    }
    protected virtual void éliminerPersonnage()
    {
        //à définir selon la logique de la partie
        //script_Partie.Set.....
    }
    protected virtual void apparaîtrePersonnage()
    {
        transform.position = positionDépart;   // Position de départ à défiir selon les règles...
        pointsVieActuels = pointsVieMax;
        estMort = false;
        longueurSaut = valeur_longueurSaut;
    }
    
    public virtual void prendreDégâts(int dégâts)
    {
        pointsVieActuels -= dégâts;
        if (pointsVieActuels <= 0 && !estMort)
        {
            mourir();
            Debug.Log("Le personnage" + nom + " est mort." + " Points de vie actuels : " + pointsVieActuels);
        }
        else
        {
            Debug.Log("Points de vie actuels : " + pointsVieActuels);
        }
    }
    public virtual void soigner(int pointsDeSoin)
    {
        pointsVieActuels += pointsDeSoin;
        if (pointsVieActuels > pointsVieMax)
        {
            pointsVieActuels = pointsVieMax;
        }
        Debug.Log("Points de vie actuels : " + pointsVieActuels);
    }
    public virtual void affaiblir(int durée, float pourcentageRéduction)
    {
        estAffaibli = pourcentageRéduction;
        duréeAffaiblissement = durée;
        tempsDernierAffaiblissement = Time.time; // Enregistre le temps actuel comme le temps du dernier affaiblissement
    }
    public virtual float obtenirEstAffaibli()
    {
        return estAffaibli;
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

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using TMPro;
public class classe_personnage : MonoBehaviour, In_prendre_dégâts
{



    //attribus des personnages
    protected string nom; // nom du joueur utilisé pour l'affichage et l'identification

    // visuel du personnage
    protected classe_visuel_joueur scriptVisuelJoueur; // Référence au script visuel du joueur

    [SerializeField] protected TMP_Text nomTexte; // Référence au texte affichant le nom du joueur


    //vie
    [SerializeField] protected int pointsVieMax; // Points de vie maximum du personnage

    //déplacement
    [SerializeField] protected float vitesse; // Vitesse de déplacement du personnage
    [SerializeField] protected float forceSaut; // Force appliquée lors du saut du personnage
    [SerializeField] protected float valeur_longueurSaut; // Valeur de la longueur du saut du personnage
    [SerializeField] protected Vector3 positionDépart; // Position de départ du personnage dans la scène
    




    //touches 
    [SerializeField] protected Key toucheDroite; // Touche pour déplacer le personnage vers la droite
    [SerializeField] protected Key toucheGauche;   // Touche pour déplacer le personnage vers la gauche
    [SerializeField] protected Key toucheSaut; // Touche pour faire sauter le personnage
    [SerializeField] protected Key toucheDashBas; // Touche pour effectuer un dash vers le bas

    

    //animation
    [SerializeField] protected Animator animator; // Référence à l'Animator du personnage
    //couleur du personnage
    protected SpriteRenderer spriteRenderer; // Référence au SpriteRenderer du personnage
    protected Color couleurInitiale; // Couleur initiale du personnage
    protected Color couleurRalenti = Color.blue; // Couleur appliquée lorsque le personnage est ralenti
    protected Color couleurAffaibli = Color.green; // Couleur appliquée lorsque le personnage est affaibli
    protected Color couleurRalentiAffaibli = Color.cyan; // Couleur appliquée lorsque le personnage est à la fois ralenti et affaibli
    protected Color couleurDégâts = Color.red; // Couleur appliquée lorsque le personnage prend des dégâts
    protected float duréeFlash = 0.1f; // Durée du flash de couleur lorsqu'on prend des dégâts
    protected bool estEnFlash = false; // Indique si le personnage est actuellement en train de flasher ou non

    //variables de jeu

    //vie
    protected float pointsVieActuels; // Points de vie actuels du personnage
    protected bool estMort = false; // Indique si le personnage est mort
    protected bool estEliminé = false; // Indique si le personnage a été éliminé
    protected int nombreVies = 3; // Nombre de vies du personnage

    //déplacement
    protected bool regardeDroite = true; // Indique si le personnage regarde vers la droite
    protected float longueurSaut; // Longueur actuelle du saut du personnage
    protected bool estEnSaut = false; // Indique si le personnage est actuellement en train de sauter
    protected int sautRestant = 1; // Nombre de sauts restants (1 pour un double saut) :)
    protected bool peutBouger = true; // Variable pour contrôler si le personnage peut bouger ou non

    //ralentissement
    protected float estRalenti = 0f; // Variable pour contrôler à quel point le personnage est ralenti (0 = pas ralenti, 1 = complètement ralenti)
    protected float duréeRalenti = 0; // Variable pour stocker la durée du ralentissement en secondes
    protected float tempsDernierRalenti = 0; // Variable pour stocker le temps écoulé depuis le dernier ralentissement
    //affaiblissement
    protected float duréeAffaiblissement = 0; // Variable pour stocker la durée de l'affaiblissement en secondes
    protected float tempsAffaibli = 0; // Variable pour stocker le temps écoulé depuis le début de l'affaiblissement
    protected float estAffaibli = 0f; // Variable pour contrôler à quel point le personnage est affaibli (0 = pas affaibli, 1 = complètement affaibli)
    protected float tempsDernierAffaiblissement = 0; // Variable pour stocker le temps écoulé depuis le dernier affaiblissement
    

    // en phase d'élimination après le temps imparti
    protected bool enElimination = false; // Indique si le personnage est en phase d'élimination après le temps imparti    
    //hitbox
    protected Rigidbody2D rb; // Référence au Rigidbody2D du personnage
    protected BoxCollider2D boxCollider; // Référence au BoxCollider2D du personnage
    //hitbox ennemis
    protected BoxCollider2D boxColliderEnnemi; // Référence au BoxCollider2D de l'ennemi

    //layer du personnage
    protected int layerpersonnage; // Layer du personnage


    //paramètres match
    

    protected virtual void Awake() // Awake est appelé avant Start, même si le script est désactivé
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Référence au SpriteRenderer du personnage
        couleurInitiale = spriteRenderer.color; // Stocke la couleur initiale du SpriteRenderer
        rb = GetComponent<Rigidbody2D>(); // Référence au Rigidbody2D du personnage
        boxCollider = GetComponent<BoxCollider2D>(); // Référence au BoxCollider2D du personnage
        
    }
    protected virtual void Start() // Start est appelé avant la première image, seulement si le script est activé
    {
        transform.position = positionDépart;   // Position de départ à défiir selon les règles...
        pointsVieActuels = pointsVieMax; // Initialise les points de vie actuels à la valeur maximale
        estMort = false; // Initialise l'état de mort du personnage à false
        longueurSaut = valeur_longueurSaut; // Initialise la longueur du saut à la valeur définie
        Physics2D.IgnoreCollision(boxCollider, boxColliderEnnemi, true); // Ignore la collision entre le personnage et l'ennemi au démarrage
    }



    protected virtual void Update() // Update est appelé à chaque frame, seulement si le script est activé
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
            
            //applique la vitesse horizontale au personnage en tenant compte de l'état ralenti
            if (peutBouger && !enElimination)
            {
                rb.linearVelocity = new UnityEngine.Vector2
                (
                    moveX * vitesse * (1 - estRalenti), 
                    rb.linearVelocity.y
                ); 
            }
            else
            {
                rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, rb.linearVelocity.y); // Empêche le personnage de bouger pendant le double saut ou lorsqu'il est en élimination
                if (detectersol())
                {
                    peutBouger = true; // Permet au personnage de bouger à nouveau lorsqu'il touche le sol
                }
            }
            
            //calcul de la vitesse actuelle pour l'animation (valeur absolue de la vitesse horizontale)
            float vitesseActuelle = Mathf.Abs(rb.linearVelocity.x); // Calcule la vitesse actuelle pour l'animation (valeur absolue de la vitesse horizontale)
            animator.SetFloat("Vitesse", vitesseActuelle); // Met à jour l'animation en fonction de la vitesse actuelle

            
            //couleur du personnage en fonction de l'état (ralenti, affaibli, ralenti et affaibli)
            if (!estEnFlash) // Vérifie si le personnage n'est pas en train de flasher
            {
                if (estRalenti > 0 && estAffaibli > 0) // Vérifie si le personnage est à la fois ralenti et affaibli
                {
                    changerCouleur(couleurRalentiAffaibli);
                }
                else if (estRalenti > 0) // Vérifie si le personnage est ralenti
                {
                    changerCouleur(couleurRalenti);
                }
                else if (estAffaibli > 0) // Vérifie si le personnage est affaibli
                {
                    changerCouleur(couleurAffaibli);
                }
                else // Si le personnage n'est ni ralenti ni affaibli
                {
                    réinitialiserCouleur();
                }
            }


            



            //direction du personnage
            if (moveX > 0 && !regardeDroite) // Vérifie si le personnage se déplace vers la droite alors qu'il ne regarde pas dans cette direction
            {
                regardeDroite = true;
                transform.localScale = new UnityEngine.Vector3(1, 1, 1);
                nomTexte.transform.localScale = new UnityEngine.Vector3(1, 1, 1); // Réinitialise le texte du nom à sa taille normale lorsque le personnage regarde à droite
            }
            else if (moveX < 0 && regardeDroite) // Vérifie si le personnage se déplace vers la gauche alors qu'il regarde dans la direction opposée
            {
                regardeDroite = false;
                transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
                nomTexte.transform.localScale = new UnityEngine.Vector3(-1, 1, 1); // Inverse le texte du nom pour le garder à l'endroit lorsque le personnage regarde à gauche
            }




            // saut
            if (detectersol()) // Vérifie si le personnage touche le sol
            {
                sautRestant = 1; // Réinitialise le nombre de sauts restants lorsqu'il touche le sol
                peutBouger = true; // Permet au personnage de bouger lorsqu'il touche le sol
            }

            if (clavier[toucheSaut].wasPressedThisFrame) // Vérifie si la touche de saut a été pressée ce frame
            {
                if (detectersol() && !enElimination) // Vérifie si le personnage touche le sol et n'est pas en élimination
                {
                    rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, forceSaut);
                    estEnSaut = true;
                    
                }
                else if (sautRestant > 0) // Vérifie si le personnage a des sauts restants pour effectuer un double saut
                {
                    rb.linearVelocity = new UnityEngine.Vector2((System.Convert.ToInt32(regardeDroite) * 2 - 1) * vitesse * (1- estRalenti), forceSaut * 0.5f); // Applique la vitesse horizontale en fonction de la direction du personnage
                    peutBouger = false; // Empêche le personnage de bouger pendant le double saut
                    sautRestant--; // Décrémente le nombre de sauts restants
                }
            }
            //dash vers le bas
            if(clavier[toucheDashBas].wasPressedThisFrame && !enElimination) // Vérifie si la touche de dash vers le bas a été pressée et que le personnage n'est pas en élimination
            {
                if(detectersol() == false) // Vérifie que le personnage soit en l'air avant d'effectuer le dash vers le bas
                {
                    peutBouger = true; // Permet au personnage de bouger à nouveau après le dash vers le bas
                    rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, -forceSaut);
                }



            }


            
            //Prolongation du premier saut si la touche de saut est maintenue enfoncée
            if (estEnSaut == true && clavier [toucheSaut].isPressed && peutBouger == true && !enElimination) // vérifie si le personnage est en train de sauter, si la touche de saut est maintenue enfoncée, si le personnage peut bouger et s'il n'est pas en élimination
            {
                rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocity.x, forceSaut); // Applique la vitesse verticale pour prolonger le saut
                longueurSaut -= Time.deltaTime; // Réduit la durée restante du saut prolongé en fonction du temps écoulé
                if (longueurSaut <= 0)
                {
                    estEnSaut = false; // Indique que le personnage n'est plus en train de sauter
                    longueurSaut = valeur_longueurSaut; // Réinitialise la durée du saut prolongé
                
                }
            }
            else if (estEnSaut == true && clavier[toucheSaut].wasReleasedThisFrame) // Vérifie si le personnage est en train de sauter et si la touche de saut a été relâchée ce frame
            {
                estEnSaut = false; // Indique que le personnage n'est plus en train de sauter
                longueurSaut = valeur_longueurSaut; // Réinitialise la durée du saut prolongé
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
                if (Time.time - tempsDernierAffaiblissement >= duréeAffaiblissement) // Vérifie si la durée de l'affaiblissement est écoulée
                {
                    estAffaibli = 0; // Réinitialise l'affaiblissement lorsque la durée est écoulée
                    Debug.Log("L'affaiblissement du personnage " + nom + " est terminé.");
                }
            }
            //gestion du ralentissement
            if (estRalenti > 0) // Vérifie si le personnage est actuellement ralenti
            {
                if (Time.time - tempsDernierRalenti >= duréeRalenti) // Vérifie si la durée du ralentissement est écoulée
                {
                    estRalenti = 0; // Réinitialise le ralentissement lorsque la durée est écoulée
                    Debug.Log("Le ralentissement du personnage " + nom + " est terminé.");
                }
            }


        }    
    }


    //méthodes

    //apparition et disparition du personnage
    protected virtual void mourir() // Gère la mort du personnage
    {
        estMort = true;
        nombreVies --; //réduit le nombre de vies du personnage de 1 chaque fois qu'il meurt
        scriptVisuelJoueur.MettreAJourNombreVies(nombreVies); // Met à jour l'affichage du nombre de vies du joueur

        if (nombreVies <= 0)
        {
            éliminerPersonnage();
        }
        else
        {
            apparaîtrePersonnage();
            
        }
    }
    protected virtual void éliminerPersonnage() // Gère l'élimination définitive du personnage
    {
        estEliminé = true;
        gameObject.SetActive(false); // Désactive le GameObject du personnage

        
    }
    protected virtual void apparaîtrePersonnage() // Gère la réapparition du personnage
    {
        transform.position = positionDépart;   // Position de départ à défiir selon les règles...
        pointsVieActuels = pointsVieMax;
        scriptVisuelJoueur.MettreAJourBarreDeVie(pointsVieActuels, pointsVieMax); // Met à jour la barre de vie du personnage

        estMort = false;
        longueurSaut = valeur_longueurSaut;
    }
    

    //effets sur le personnage (dégâts, soins, affaiblissement, ralentissement)
    public virtual void prendreDégâts(int dégâts) // Applique des dégâts au personnage
    {
        pointsVieActuels -= dégâts;
        déclencherCouleurDégâts();
        if (pointsVieActuels <= 0 && !estMort)
        {
            mourir();
            Debug.Log("Le personnage" + nom + " est mort." + " Points de vie actuels : " + pointsVieActuels);
        }
        else
        {
            Debug.Log("Points de vie actuels : " + pointsVieActuels);
        }
        scriptVisuelJoueur.MettreAJourBarreDeVie(pointsVieActuels, pointsVieMax);
    }
    //méthode pour soigner le personnage
    public virtual void soigner(int pointsDeSoin) // Soigne le personnage
    {
        pointsVieActuels += pointsDeSoin;
        if (pointsVieActuels > pointsVieMax)
        {
            pointsVieActuels = pointsVieMax;
        }
        Debug.Log("Points de vie actuels : " + pointsVieActuels);
        scriptVisuelJoueur.MettreAJourBarreDeVie(pointsVieActuels, pointsVieMax);
    }
    //affaiblissement du personnage
    public virtual void affaiblir(float durée, float pourcentageAffaiblissement) // Affaiblit le personnage pendant une durée donnée
    {
        estAffaibli = pourcentageAffaiblissement;
        duréeAffaiblissement = durée;
        tempsDernierAffaiblissement = Time.time; // Enregistre le temps actuel comme le temps du dernier affaiblissement
    }
    public virtual float obtenirEstAffaibli() // Retourne le pourcentage d'affaiblissement actuel du personnage
    {
        return estAffaibli;
    }

    //ralentissement du personnage
    public virtual void ralentir(float durée, float pourcentageRalenti) // Ralentit le personnage pendant une durée donnée
    {
        estRalenti = pourcentageRalenti;
        duréeRalenti = durée;
        tempsDernierRalenti = Time.time; // Enregistre le temps actuel comme le temps du dernier ralentissement
    }

    //méthode pour détecter si le personnage touche le sol (retourne true si le personnage est au sol, false sinon)
    protected virtual bool detectersol() // Détecte si le personnage touche le sol
    {
        UnityEngine.Vector2 tailleReduite = new UnityEngine.Vector2 // Taille réduite du box collider pour éviter la détection sur les côtés
        (
            boxCollider.bounds.size.x * 0.8f, // Réduction de la largeur du box collider
            boxCollider.bounds.size.y
        );
        
        RaycastHit2D detecterSol = Physics2D.BoxCast // Effectue un box cast à 0.1f vers le bas pour détecter le sol
        (
            boxCollider.bounds.center, // Centre du box collider comme point de départ du box cast
            tailleReduite, // Taille du box cast
            0f, // Angle de rotation du box cast
            UnityEngine.Vector2.down, // Direction du box cast (vers le bas)
            0.1f, // Distance du box cast
            UnityEngine.LayerMask.GetMask("Terrain") // Masque de couche pour détecter uniquement le terrain
        );
        if (detecterSol.collider != null) // Si le box cast détecte un collider, cela signifie que le personnage touche le sol
        {
            return true;
        }
        return false;
    }

    //couleur du personnage
    protected virtual void changerCouleur(Color couleur) // Change la couleur du personnage
    {
        spriteRenderer.color = couleur;
    }

    protected virtual void réinitialiserCouleur() // Réinitialise la couleur du personnage à sa couleur initiale
    {
        estEnFlash = false;
        spriteRenderer.color = couleurInitiale;
    }

    protected virtual void déclencherCouleurDégâts() // Change la couleur du personnage pour indiquer qu'il a subi des dégâts
    {
        changerCouleur(couleurDégâts); // Change la couleur du personnage pour indiquer qu'il a subi des dégâts
        estEnFlash = true; // Indique que le personnage est en flash de couleur pour les dégâts
        Invoke("réinitialiserCouleur", duréeFlash); // Réinitialise la couleur après la durée du flash 
    }



    //initialisation du personnage
    public virtual void initialiserPersonnage // Initialise le personnage avec les paramètres donnés
    (
        Vector3 positionInitiale,
        BoxCollider2D boxColliderPersonnageEnnemi,
        int layerpersonnage,
        classe_visuel_joueur visuelJoueur
    )
    {
        positionDépart = positionInitiale; 
        boxColliderEnnemi = boxColliderPersonnageEnnemi;
        this.layerpersonnage = layerpersonnage;
        scriptVisuelJoueur = visuelJoueur;
    }


    public virtual void assignerTouches(Key toucheDroite, Key toucheGauche, Key toucheSaut, Key toucheDashBas) // Méthode pour assigner les touches du personnage
    {
        this.toucheSaut = toucheSaut;
        this.toucheDashBas = toucheDashBas;
        this.toucheGauche = toucheGauche;
        this.toucheDroite = toucheDroite;
    }

    public virtual classe_visuel_joueur récupérerScriptVisuelJoueur() // Méthode pour récupérer le script visuel du joueur pour l'utiliser dans les scripts d'attaque
    {
        return scriptVisuelJoueur;
    }
    public virtual bool obtenirEstEliminé() // Retourne true si le personnage est éliminé, false sinon
    {
        return estEliminé;
    }

    public virtual void mettreEnElimination() // Met le personnage en état d'élimination
    {
        enElimination = true;
        rb.linearVelocity = Vector2.zero; // Arrête le mouvement du personnage lors de l'élimination
    }
    public virtual bool obtenirEnElimination() // Retourne true si le personnage est en état d'élimination, false sinon
    {
        return enElimination;
    }


    public virtual void mettreAJourNom(string nouveauNom) // Met à jour le nom du personnage et l'affiche dans le texte correspondant
    {
        nom = nouveauNom;
        if (nomTexte != null)
        {
            nomTexte.text = nom;
        }
    }
}

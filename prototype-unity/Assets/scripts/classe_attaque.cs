using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class classe_attaque : MonoBehaviour
{
    //attribus des attaques

    [SerializeField] protected int dégâtsAttaqueBase;
    [SerializeField] protected float rechargeAttaqueBase;

    [SerializeField] protected float rechargeAttaqueSpéciale;

    //touches
    [SerializeField] protected Key toucheAttaqueBase;
    [SerializeField] protected Key toucheAttaqueSpeciale;

    //variables d'attaque
    protected float duréeAttaqueBase = 0.05f;
    protected float duréeAttaqueSpéciale = 0.05f;
    protected bool enAttaque = false;
    protected bool enAttaqueSpéciale = false;
    protected float tempsDernièreAttaque = 0f;
    protected float tempsDernièreAttaqueSpéciale = 0f;
    protected int layerPersonnageEnnemi = 0;
    protected float pourcentageRéduction = 0f; // Variable pour stocker le pourcentage de réduction des dégâts


    //hitbox
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb;
    //script du personnage
    protected classe_personnage scriptPersonnage;





    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        scriptPersonnage = transform.parent.GetComponent<classe_personnage>();
        Debug.Log("script_personnage trouvé sur le parent de" + transform.name + " : " + scriptPersonnage.name);
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        enAttaque = false;
        enAttaqueSpéciale = false;
        tempsDernièreAttaque = -(rechargeAttaqueBase); // permet d'attaquer dès le début du jeu
        tempsDernièreAttaqueSpéciale = -(rechargeAttaqueSpéciale); // permet d'attaquer dès le début du jeu
        if(layerPersonnageEnnemi == 0)
        {
            définirLayerPersonnageEnnemi();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Keyboard clavier = Keyboard.current;
        if (clavier != null)
        {
            if (clavier[toucheAttaqueBase].wasPressedThisFrame && Time.time - tempsDernièreAttaque >= rechargeAttaqueBase)
            {
                attaqueBase();
                tempsDernièreAttaque = Time.time;
            }

            if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame && Time.time - tempsDernièreAttaqueSpéciale >= rechargeAttaqueSpéciale)
            {
                attaqueSpéciale();
                tempsDernièreAttaqueSpéciale = Time.time;
            }
            else if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame)
            {

                Debug.Log("Attaque spéciale en recharge. Temps restant : " + (rechargeAttaqueSpéciale - (Time.time - tempsDernièreAttaqueSpéciale)) + " secondes.");
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

    protected virtual void attaqueBase()
    {
        enAttaque = true;
        Debug.Log("Attaque de base effectuée !");
    }

    protected virtual void finAttaqueBase()
    {
        enAttaque = false;
        Debug.Log("Fin de l'attaque de base.");
    }

    protected virtual void attaqueSpéciale()
    {
        enAttaqueSpéciale = true;
        Debug.Log("Attaque spéciale effectuée !");
    }
    protected virtual void finAttaqueSpéciale()
    {
        enAttaqueSpéciale = false;
        Debug.Log("Fin de l'attaque spéciale.");
    }




    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if(enAttaque)
        {
            if (collisionAvecEnnemi(collision))
            {
                GameObject cible = collision.gameObject;
                {
                    if (infligerDégâts(cible, dégâtsAttaqueBase))
                    {
                        finAttaqueBase(); // Fin de l'attaque après avoir infligé des dégâts :)
                    }
                }
            }
        }
    }

    protected virtual bool collisionAvecEnnemi(Collider2D collision) // Vérifie si le GameObject avec lequel il y a collision est un ennemi
    {
        return collision.gameObject.layer == layerPersonnageEnnemi;
    }


    
    protected virtual bool infligerEffet(GameObject cible, int dégâts, float pourcentageRéduction, float durée) // Méthode pour infliger des dégâts et/ou des effets à un GameObject cible
    {
        if (cible.TryGetComponent<In_prendre_dégâts>(out In_prendre_dégâts personnageCible)) // Vérifie si le GameObject cible a un composant qui implémente l'interface In_prendre_dégâts et l'assigne à la variable personnageCible
        {
            dégâts = Mathf.RoundToInt(obtenirDégâts(dégâts)); // Applique la réduction des dégâts si le personnage est affaibli
            personnageCible.prendreDégâts(dégâts);
            Debug.Log("Dégâts infligés à " + cible.name + " : " + dégâts);
            personnageCible.affaiblir(pourcentageRéduction, durée); // Applique l'affaiblissement au personnage cible avec la durée spécifiée
            return true; // Retourne true si les dégâts ont été infligés avec succès
        }
        else
        {
            Debug.Log("Aucune interface In_prendre_dégâts trouvé sur " + cible.name);
            return false; // Retourne false si le GameObject cible n'a pas de composant qui implémente l'interface In_prendre_dégâts
        }
    }
    protected virtual bool infligerDégâts(GameObject cible, int dégâts)
    {
        return infligerEffet(cible, dégâts, 0f); // Appelle infligerEffet avec un pourcentage de réduction de 0
    }
    protected virtual bool infligerAffaiblissement(GameObject cible, float pourcentageRéduction, float durée)
    {
        return infligerEffet(cible, 0, pourcentageRéduction, durée); // Appelle infligerEffet avec des dégâts de 0
    }

    protected virtual float obtenirDégâts(float dégâts)
    {
        if (scriptPersonnage == null)
        {
            Debug.LogError("Le script du personnage n'est pas assigné dans " + gameObject.name);
            return dégâts; // Retourne les dégâts d'origine si le script du personnage n'est pas assigné
        }
        pourcentageRéduction = scriptPersonnage.obtenirEstAffaibli();
        if (pourcentageRéduction > 0f)
        {
            float dégâtsRéduits = dégâts * (1f - pourcentageRéduction);
            return dégâtsRéduits;
        }
        else
        {
            return dégâts;
        }
    }

    protected virtual void génererProjectile(GameObject projectile, Vector3 position, float vitesseX, float vitesseY, int dégâts, float duréeVie)
    {
        // Instancie le projectile à la position spécifiée sans rotation
        GameObject nouveauProjectile = Instantiate(projectile, position, Quaternion.identity); 
        script_projectile_attaque_spéciale scriptProjectile = nouveauProjectile.GetComponent<script_projectile_attaque_spéciale>();
        if (scriptProjectile != null)
        {
            scriptProjectile.initialiserProjectile(position, vitesseX, vitesseY, dégâts, duréeVie, layerPersonnageEnnemi);

        }
        else
        {
            Debug.LogError("Le GameObject n'a pas de script_projectile_attaque_spéciale attaché.");
            
        }
    }


    protected virtual void définirLayerPersonnageEnnemi()// Définit le layer du personnage ennemi en fonction du layer du personnage actuel
    {
        if (gameObject.layer == LayerMask.NameToLayer("Personnage_1")) // Layer "Personnage_1"
            layerPersonnageEnnemi = LayerMask.NameToLayer("Personnage_2"); // Layer "Personnage_2"
        else if (gameObject.layer == LayerMask.NameToLayer("Personnage_2")) // Layer "Personnage_2"
            layerPersonnageEnnemi = LayerMask.NameToLayer("Personnage_1"); // Layer "Personnage_1"
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class classe_attaque_base : classe_attaque
{
    //attribus des attaques

    [SerializeField] protected int dégâtsAttaqueBase;
    [SerializeField] protected float rechargeAttaqueBase;



    //touches
    [SerializeField] protected Key toucheAttaqueBase;


    //variables d'attaque
    protected float duréeAttaqueBase = 0.05f;
    
    protected bool enAttaque = false;

    protected float tempsDernièreAttaque = 0f;

    


    //script du personnage
    protected classe_personnage scriptPersonnage;





    protected override void Awake()
    {
        base.Awake();
        scriptPersonnage = transform.parent.GetComponent<classe_personnage>();
        Debug.Log("script_personnage trouvé sur le parent de" + transform.name + " : " + scriptPersonnage.name);
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        enAttaque = false;
        tempsDernièreAttaque = -(rechargeAttaqueBase); // permet d'attaquer dès le début du jeu
        if(layerPersonnageEnnemi == 0)
        {
            définirLayerPersonnageEnnemi(gameObject.layer); // Définit le layer du personnage ennemi en fonction du layer du personnage actuel
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        Keyboard clavier = Keyboard.current;
        if (clavier != null)
        {
            if (clavier[toucheAttaqueBase].wasPressedThisFrame && Time.time - tempsDernièreAttaque >= rechargeAttaqueBase)
            {
                attaqueBase();
                tempsDernièreAttaque = Time.time;
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





    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if(enAttaque && collisionAvecEnnemi(collision))
        {
            GameObject cible = collision.gameObject;
                
            if (infligerDégâts(cible, dégâtsAttaqueBase))
            {
                finAttaqueBase(); // Fin de l'attaque après avoir infligé des dégâts :)
            }
                
        }
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
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class classe_attaque : MonoBehaviour
{
    //attribus de l'attaque de base

    [SerializeField] protected int dégâtsAttaqueBase;
    [SerializeField] protected int dégâtsAttaqueSpéciale;
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

    //hitbox
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb;






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        enAttaque = false;
        enAttaqueSpéciale = false;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
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
        if(enAttaque)
        {
            // Ignore les collisions avec le parent
            ignorerCollisionAvecParent(collision);
        
            if (collision.gameObject.layer == LayerMask.NameToLayer("Personnage"))
            {
                GameObject cible = collision.gameObject;
                {
                    infligerDégâts(cible, dégâtsAttaqueBase);
                    finAttaqueBase(); // Fin de l'attaque après avoir infligé des dégâts :)
                }
            }
        }
    }

    protected void ignorerCollisionAvecParent(Collider2D collision)
    {
        if (transform.parent != null && collision.transform == transform.parent)
        {
            Debug.Log("Collision ignorée avec le parent : " + collision.gameObject.name);
            return; 
        }
    }

    protected virtual void infligerDégâts(GameObject cible, int dégâts)
    {
        script_personnage personnageCible = cible.GetComponent<script_personnage>();
        if (personnageCible != null)
        {
            personnageCible.prendreDégâts(dégâts);
            Debug.Log("Dégâts infligés à " + cible.name + " : " + dégâts);
        }
        else
        {
            Debug.Log("Aucun script_personnage trouvé sur " + cible.name);
        }
    }
}

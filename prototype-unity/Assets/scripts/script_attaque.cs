using UnityEngine;
using UnityEngine.InputSystem;
public class script_attaque : classe_attaque
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //attaque spéciale
        Keyboard clavier = Keyboard.current;
        if (clavier != null)
        {
            if (clavier[toucheAttaqueSpeciale].wasPressedThisFrame && Time.time - tempsDernièreAttaque >= rechargeAttaqueSpéciale)
            {
                attaqueSpéciale();
                tempsDernièreAttaque = Time.time;
            }
        }
        if (enAttaqueSpéciale)
        {
            if (Time.time - tempsDernièreAttaque >= duréeAttaqueSpéciale)
            {
                finAttaqueSpéciale();
            }
        }
        
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        ignorerCollisionAvecParent(collision);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Personnage"))
        {
            infligerDégâts(collision.gameObject, dégâtsAttaqueSpéciale);
        }
    }

    protected void attaqueSpéciale()
    {
        enAttaqueSpéciale = true;
        Debug.Log("Attaque spéciale effectuée !");
    }
    protected void finAttaqueSpéciale()
    {
        enAttaqueSpéciale = false;
        Debug.Log("Fin de l'attaque spéciale.");
    }
}



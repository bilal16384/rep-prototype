using UnityEngine;

public class script_points_vie : MonoBehaviour
{
    public float pointsVieMax = 100f;
    private float pointsVieActuels;
    private bool estMort = false;

    

    private Collider2D boxCollider;

    [SerializeField] private script_attaque_base attaqueBaseScript;
    [SerializeField] private BoxCollider2D boxColliderAttaqueBase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointsVieActuels = pointsVieMax;
        boxCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attaqueBaseScript.enAttaque && pointsVieActuels > 0 && boxColliderAttaqueBase.IsTouching(boxCollider))
        {
            pointsVieActuels -= attaqueBaseScript.dégâtsAttaqueBase;
            attaqueBaseScript.enAttaque = false; // Réinitialise l'état d'attaque
            Debug.Log("Points de vie actuels : " + pointsVieActuels);
        }

        if (pointsVieActuels <= 0 && !estMort)
        {
            estMort = true;
            Debug.Log("Le personnage est mort.");
            
        }
    }
}

using Unity.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
public class script_move : MonoBehaviour
{
    
    
    
    
    public float vitesse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
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
            
            transform.position += new Vector3(moveX, 0, 0) * vitesse * Time.deltaTime;
        }
    }
}
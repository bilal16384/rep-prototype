using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(fileName = "données_joueurs", menuName = "Scriptable Objects/données_joueurs")]
public class Données_joueurs : ScriptableObject // Classe stockant les données des joueurs (touches de contrôle)
{
    //touches des joueurs (clavier)

    //joueur 1
    public Key toucheDroite1 = Key.D;
    public Key toucheGauche1 = Key.A;   
    public Key toucheSaut1 = Key.W;
    public Key toucheDashBas1 = Key.S;

    public Key toucheAttaqueBase1 = Key.C;
    public Key toucheAttaqueSpéciale1 = Key.V;
    
    //joueur 2
    public Key toucheDroite2 = Key.L;
    public Key toucheGauche2 = Key.J;   
    public Key toucheSaut2 = Key.I;
    public Key toucheDashBas2 = Key.K;
    public Key toucheAttaqueBase2 = Key.N;
    public Key toucheAttaqueSpéciale2 = Key.M;


}

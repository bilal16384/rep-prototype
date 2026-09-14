using UnityEngine;

public interface In_prendre_dégâts
{
    void prendreDégâts(int dégâts);
    void affaiblir(float duréeAffaiblissement, float pourcentageAffaiblissement);
    void ralentir(float duréeRalenti, float pourcentageRalenti);
    void soigner(int pointsDeSoin);
}
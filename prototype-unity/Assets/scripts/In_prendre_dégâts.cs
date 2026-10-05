using UnityEngine;

public interface In_prendre_dégâts  // Interface définissant les actions liées aux dégâts, affaiblissement, ralentissement et soins
{
    void prendreDégâts(int dégâts); // Applique des dégâts au personnage
    void affaiblir(float duréeAffaiblissement, float pourcentageAffaiblissement); // Applique un affaiblissement temporaire au personnage
    void ralentir(float duréeRalenti, float pourcentageRalenti); // Applique un ralentissement temporaire au personnage
    void soigner(int pointsDeSoin); // Soigne le personnage de certains points de vie
}
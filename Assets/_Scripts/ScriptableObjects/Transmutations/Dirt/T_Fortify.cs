using UnityEngine;

[CreateAssetMenu(fileName = "T_Fortify", menuName = "Scriptable Objects/Transmutations/T_Fortify")]
public class T_Fortify : TransmutationSOBase
{
    public GameObject FortifyArmor; // Reference to prefab of the projectile that this transmutation summons
    public float immunityTime = 2;
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        GameObject fortifyArmor = Instantiate(FortifyArmor, player.transform);
        Destroy(fortifyArmor, immunityTime);
        player.GetComponentInChildren<PlayerCasting>().T_Fortify(immunityTime);
    }

}

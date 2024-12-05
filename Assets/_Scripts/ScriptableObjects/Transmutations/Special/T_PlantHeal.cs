
using UnityEngine;

[CreateAssetMenu(fileName = "T_PlantHealth", menuName = "Scriptable Objects/Transmutations/T_PlantHealth")]
public class T_PlantHealth : TransmutationSOBase
{
    public float healingAmount;
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);
        
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_heal, player.transform.position);
        player.GetComponentInChildren<Player>().Heal(healingAmount);
        Destroy(player.GetComponentInChildren<PlayerCasting>().currentObject);
    }
}

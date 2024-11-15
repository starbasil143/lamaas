using UnityEngine;

[CreateAssetMenu(fileName = "T_GrassSpike", menuName = "Scriptable Objects/Transmutations/T_GrassSpike")]
public class T_GrassSpike : TransmutationSOBase
{
    public GameObject theSpikes; // Reference to prefab of the object that this transmutation summons
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        Instantiate(theSpikes, player.transform.position, player.transform.rotation);
    }

}

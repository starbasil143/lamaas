using UnityEngine;

[CreateAssetMenu(fileName = "T_GroundSlam", menuName = "Scriptable Objects/Transmutations/T_GroundSlam")]
public class T_GroundSlam : TransmutationSOBase
{
    public GameObject groundSlamPrefab; // Reference to prefab of the projectile that this transmutation summons
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        Instantiate(groundSlamPrefab, player.transform.position, player.transform.rotation);
        Debug.Log("Deploying ground Slam");
    }

}

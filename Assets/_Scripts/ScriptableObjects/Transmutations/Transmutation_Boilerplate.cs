using UnityEngine;

//[CreateAssetMenu(fileName = "T_TesttileExplosion", menuName = "Scriptable Objects/Transmutations/T_TesttileExplosion")]
public class Transmutation_Boilerplate : TransmutationSOBase
{
    // THIS IS JUST AN EXAMPLE . dont  put this script on anything. please
    public GameObject theObject; // Reference to prefab of the projectile that this transmutation summons
    public override void PerformTransmutation(GameObject player)
    {
        // you have a reference to the player and can declare and use whatever public variables you need.
        base.PerformTransmutation(player);

        Instantiate(theObject, player.transform.position, player.transform.rotation);
    }

}

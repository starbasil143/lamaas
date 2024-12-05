using UnityEngine;

[CreateAssetMenu(fileName = "T_IceSpike", menuName = "Scriptable Objects/Transmutations/T_IceSpike")]
public class T_IceSpike : TransmutationSOBase
{
    public GameObject IceSpike; // Reference to prefab of the object that this transmutation summons
    public float knockback;
    public float duration;
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = player.transform.position;

        GameObject iceSpike = Instantiate(IceSpike, mousePos, Quaternion.identity);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_icespike, player.transform.position);

    }
}

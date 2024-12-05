using UnityEngine;

[CreateAssetMenu(fileName = "T_IceTomb", menuName = "Scriptable Objects/Transmutations/T_IceTomb")]
public class T_IceTomb : TransmutationSOBase
{
    public GameObject IceTomb; // Reference to prefab of the object that this transmutation summons
    public float knockback;
    public float duration;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = player.transform.position;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_icespike, player.transform.position);
        GameObject iceTomb = Instantiate(IceTomb, mousePos, Quaternion.identity);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            
            Debug.Log("You Have Hit an Enemy");

        }
    }
}

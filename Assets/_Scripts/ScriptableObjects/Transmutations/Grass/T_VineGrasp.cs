using UnityEngine;

[CreateAssetMenu(fileName = "T_VineGrasp", menuName = "Scriptable Objects/Transmutations/T_VineGrasp")]
public class T_VineGrasp : TransmutationSOBase
{
    public GameObject vineGraspPrefab;
    public float timeToFullyExtend = .5f;
    public float range = 10f;
    public float pullFactor = 2f;
    public float lifetimeFactor = 2.2f;
    public float distanceToDrop = .5f;
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2); // Get mouse position
        float castAngle = Vector2.SignedAngle(Vector2.right, mousePos); // Get angle from mouse position
        Vector2 castDirection = (mousePos - Vector2.zero).normalized;

        GameObject Projectile = Instantiate(vineGraspPrefab, new Vector2(player.transform.position.x, player.transform.position.y + 0.5f) + mousePos.normalized*2, Quaternion.Euler(0, 0, castAngle)); // Make the projectile
        //Physics2D.IgnoreCollision(Projectile.GetComponent<Collider2D>(), player.GetComponent<Collider2D>()); // Make the player collider and projectile collider ignore each other
        
        Projectile.GetComponent<Vine>().direction = castDirection;
        Projectile.GetComponent<Vine>().timeToFullyExtend = timeToFullyExtend;
        Projectile.GetComponent<Vine>().range = range;
        Projectile.GetComponent<Vine>().pullFactor = pullFactor;
        Projectile.GetComponent<Vine>().lifetimeFactor = lifetimeFactor;
        Projectile.GetComponent<Vine>().distanceToDrop = distanceToDrop;
    }
}

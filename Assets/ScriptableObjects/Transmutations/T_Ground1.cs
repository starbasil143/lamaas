using UnityEngine;

[CreateAssetMenu(fileName = "T_Ground1", menuName = "Scriptable Objects/Transmutations/T_Ground1")]
public class T_Ground1 : TransmutationSOBase
{
    public GameObject projectile;
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
        float castAngle = Vector2.SignedAngle(Vector2.right, mousePos);
        if (!Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y+0.5f) + mousePos.normalized*.5f, mousePos.normalized, .5f, 1<<LayerMask.NameToLayer("Collisions")))
        {
            GameObject Projectile = Instantiate(projectile, new Vector2(player.transform.position.x, player.transform.position.y+0.5f) + mousePos.normalized, Quaternion.Euler(0, 0, castAngle));
            Vector2 shootForce = mousePos.normalized * 7;
            Physics2D.IgnoreCollision(Projectile.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
            Projectile.GetComponent<Rigidbody2D>().AddForce(shootForce, ForceMode2D.Impulse);
        }
    }

}

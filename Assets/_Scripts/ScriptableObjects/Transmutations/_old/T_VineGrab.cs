using UnityEngine;

[CreateAssetMenu(fileName = "T_VineGrab", menuName = "Scriptable Objects/Transmutations/T_VineGrab")]
public class T_VineGrab : TransmutationSOBase
{
    public GameObject projectile;

    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2); // Get mouse position
        float castAngle = Vector2.SignedAngle(Vector2.right, mousePos); // Get angle from mouse position

        if (!Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y + 0.5f) + mousePos.normalized * .5f, mousePos.normalized, .5f, 1 << LayerMask.NameToLayer("Collisions"))) // If you are not standing right up against a wall
        {
            GameObject Projectile = Instantiate(projectile, new Vector2(player.transform.position.x, player.transform.position.y + 0.5f) + mousePos.normalized, Quaternion.Euler(0, 0, castAngle)); // Make the projectile
            Projectile.GetComponent<HarmfulObjectScript>().Source = player;
            Physics2D.IgnoreCollision(Projectile.GetComponent<Collider2D>(), player.GetComponent<Collider2D>()); // Make the player collider and projectile collider ignore each other
            Vector2 shootForce = mousePos.normalized * 7;
            Projectile.GetComponent<Rigidbody2D>().AddForce(shootForce, ForceMode2D.Impulse); // Give the projectile a force so it moves
        }
    }
}

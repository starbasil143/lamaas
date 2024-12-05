using UnityEngine;

[CreateAssetMenu(fileName = "T_VineGrasp", menuName = "Scriptable Objects/Transmutations/T_VineGrasp")]
public class T_VineGrasp : TransmutationSOBase
{
    public GameObject thornGraspPrefab;
    public float ThornGraspSpeed = 14f;
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);

        Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2); // Get mouse position
        float castAngle = Vector2.SignedAngle(Vector2.right, mousePos); // Get angle from mouse position
        Vector2 castDirection = (mousePos - Vector2.zero).normalized;

        if (!Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y + 0.5f) + mousePos.normalized * .5f, mousePos.normalized, .5f, 1 << LayerMask.NameToLayer("Collisions"))) // If you are not standing right up against a wall
        {
            GameObject Projectile = Instantiate(thornGraspPrefab, new Vector2(player.transform.position.x, player.transform.position.y + 0.5f) + mousePos.normalized, Quaternion.Euler(0, 0, castAngle)); // Make the projectile
            Physics2D.IgnoreCollision(Projectile.GetComponent<Collider2D>(), player.GetComponent<Collider2D>()); // Make the player collider and projectile collider ignore each other
            
            Projectile.GetComponent<Vine>().direction = castDirection;
        }
    }
}

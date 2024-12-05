using UnityEngine;

public class PushTowardPoint : MonoBehaviour
{
    [SerializeField] private Vector3 point;

    private void Start()
    {
        Vector2 force = point-transform.position;
        GetComponent<Rigidbody2D>().AddForce(1.2f * force, ForceMode2D.Impulse);
    }
}

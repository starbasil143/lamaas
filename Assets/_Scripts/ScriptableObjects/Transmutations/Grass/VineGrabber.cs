using UnityEngine;

public class VineGrabber : MonoBehaviour
{
    private GameObject vine;

    private void Awake()
    {
        vine = transform.parent.gameObject;
    }

    private void Update()
    {
        transform.localPosition = new Vector3(vine.GetComponent<SpriteRenderer>().size.x + 0.3f, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!vine.GetComponent<Vine>().isGrappling)
        {
            if(collision.gameObject.CompareTag("Enemy") || LayerMask.LayerToName(collision.gameObject.layer) == "Obstacle")
            {
                vine.GetComponent<Vine>().Catch(collision.gameObject);
            }
        }
        Debug.Log(collision.gameObject);
    }
}

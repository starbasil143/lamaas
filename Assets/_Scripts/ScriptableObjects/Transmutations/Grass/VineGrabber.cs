using UnityEngine;

public class VineGrabber : MonoBehaviour
{
    private GameObject vine;

    private void Awake()
    {
        vine = transform.parent.gameObject;
    }

    private void FixedUpdate()
    {
        transform.localPosition = vine.transform.position + new Vector3(vine.GetComponent<SpriteRenderer>().size.x + .4f, 0, 0);
    }
}

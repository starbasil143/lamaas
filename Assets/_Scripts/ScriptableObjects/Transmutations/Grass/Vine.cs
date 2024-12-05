using UnityEngine;

public class Vine : MonoBehaviour
{
    private GameObject grabber;
    public Vector2 direction;

    private void Start()
    {
        grabber = transform.Find("Grabber").gameObject;
    }

    private void FixedUpdate()
    {

    }
}

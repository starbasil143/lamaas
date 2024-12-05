using System.Collections;
using UnityEngine;

public class Vine : MonoBehaviour
{
    private GameObject grabber;
    public Vector2 direction;
    public Vector2 point;
    public float range = 100f;
    private GameObject _player;
    public SpriteRenderer _spriteRenderer;
    public float normalizedReach = 0f;
    public float timeToFullyExtend = .5f;
    public float pullFactor;
    private GameObject caughtTarget;
    public float lifetimeFactor;
    public float distanceToDrop;
    public Vector3 localpoint;

    public bool isRetracting = false;
    public bool isGrappling = false;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        grabber = transform.Find("Grabber").gameObject;
        point = (Vector2)transform.position + range*direction;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.size = new Vector2(0, 0.33333f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_vinestart, transform.position);
    }

    private void Update()
    {
        transform.position = new Vector2(_player.transform.position.x, _player.transform.position.y + 0.5f);
        transform.right = (Vector3)point - transform.position;

        

        
        if (isGrappling)
        {
            if (caughtTarget.CompareTag("Enemy"))
            {
                point = caughtTarget.transform.position;
            }
            normalizedReach = 1;
            _spriteRenderer.size = new Vector2((-Vector3.Distance(transform.position, point) +.3f) * .7f, _spriteRenderer.size.y);
            Vector3 force = ((Vector3)point - new Vector3(_player.transform.position.x, _player.transform.position.y + 0.5f, 0)).normalized * pullFactor;
            _player.GetComponentInChildren<PlayerMovement>().grappleForce = force;

            if (Vector2.Distance(new Vector2(_player.transform.position.x, _player.transform.position.y + 0.5f), point) < distanceToDrop)
            {
                _player.GetComponentInChildren<PlayerMovement>().isGrappling = false;
                _player.GetComponentInChildren<PlayerMovement>().grappleForce = Vector3.zero;
                
                Destroy(gameObject);
            }
        }
        else if (!isRetracting)
        {
            normalizedReach += Time.deltaTime / timeToFullyExtend;
            _spriteRenderer.size = new Vector2(Vector3.Distance(transform.position, point) * -normalizedReach, _spriteRenderer.size.y);


            if (normalizedReach >= 1)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_vineend, transform.position);
                isRetracting = true;
            }
        }
        else
        {
            normalizedReach -= Time.deltaTime / timeToFullyExtend;
            _spriteRenderer.size = new Vector2(Vector3.Distance(transform.position, point) * -normalizedReach, _spriteRenderer.size.y);
            if (normalizedReach <= 0)
            {
                Destroy(gameObject);
            }
            
        }

    }

    public void CancelGrapple()
    {
        isGrappling = false;
        _player.GetComponentInChildren<PlayerMovement>().isGrappling = false;
        Destroy(gameObject);
    }


    public void Catch(GameObject target)
    {
        Debug.Log("goooood morning!!!!");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_vineend, transform.position);
        caughtTarget = target;
        if (caughtTarget.CompareTag("Enemy"))
        {
            point = caughtTarget.transform.position;
        }
        else
        {
            //point = transform.position + (-_spriteRenderer.size.x * (transform.rotation * Vector3.right));
            point = grabber.transform.position;
            localpoint = grabber.transform.localPosition;
        }
        StartCoroutine(Grapple());
    }

    private IEnumerator Grapple()
    {
        isGrappling = true;
        _player.GetComponentInChildren<PlayerMovement>().isGrappling = true;
        yield return new WaitForSeconds(lifetimeFactor);
        _player.GetComponentInChildren<PlayerMovement>().isGrappling = false;
        Destroy(gameObject);
    }
}

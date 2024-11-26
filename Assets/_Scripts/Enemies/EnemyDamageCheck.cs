using UnityEngine;
using UnityEngine.Scripting;

public class EnemyDamageCheck : MonoBehaviour
{
    private Enemy _enemy;
    private void Awake()
    {
        _enemy = transform.parent.GetComponentInChildren<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Harm"))
        {
            _enemy.ReceiveHarm(collision.gameObject.GetComponent<HarmfulObjectScript>());
        }
        if (collision.gameObject.CompareTag("Vines"))
        {
            _enemy.VinePull(collision.gameObject.GetComponent<HarmfulObjectScript>());
        }
    }
}

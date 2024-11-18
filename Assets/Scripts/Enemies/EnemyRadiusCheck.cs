using UnityEngine;

public class EnemyRadiusCheck : MonoBehaviour
{
    private GameObject _player;
    private Enemy _enemy;
    public string radiusType;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemy = transform.parent.GetComponentInChildren<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (radiusType)
            {
                case "Hostile":
                    _enemy.HostileRadiusEntryTrigger();
                break;
                case "Attack":

                break;
                default:
                Debug.LogWarning("You Broke It,,,... (invalid entry radius type)");
                break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (radiusType)
            {
                case "Hostile":
                    _enemy.HostileRadiusExitTrigger();
                break;
                case "Attack":

                break;
                default:
                Debug.LogWarning("You Broke It,,,... (invalid exit radius type)");
                break;
            }
        }
    }
}

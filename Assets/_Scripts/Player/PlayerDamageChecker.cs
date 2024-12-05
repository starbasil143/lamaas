using UnityEngine;

public class PlayerDamageChecker : MonoBehaviour
{
    private Player _player;
    private void Awake()
    {
        _player = transform.parent.GetComponentInChildren<Player>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Harm"))
        {
            if (_player.healthAmount > 0)
            {
                _player.ReceiveHarm(collision.gameObject.GetComponent<HarmfulObjectScript>());
            }
        }
    }
}

using UnityEngine;

public class Scroll : MonoBehaviour
{
    private PlayerCasting _playerCasting;
    public TileData tileData;
    public int index;
    void Start()
    {
        _playerCasting = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerCasting>();
    }

    public void LearnSpell()
    {
        _playerCasting.LearnSpell(tileData, index);
        Destroy(gameObject);
    }
}

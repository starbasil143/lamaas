using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

public class OLDProgressFlagHandlerOLD : MonoBehaviour
{
    private GameObject _player;
    private PlayerProgress _progress;

    public List<GameObject> removeTrue0;
    public List<GameObject> removeFalse0;
    public List<GameObject> removeTrue1;
    public List<GameObject> removeFalse1;
    public List<GameObject> removeTrue2;
    public List<GameObject> removeFalse2;
    
    public List<GameObject>[] removeOnFlagTrue = new List<GameObject>[3]; // set size manually
    public List<GameObject>[] removeOnFlagFalse = new List<GameObject>[3];

    // lists of game objects to disable for each flag
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _progress = _player.GetComponent<PlayerProgress>();

        for (int i = 0; i < _progress.progressFlags.Count; i++) // for each flag
        {
            if (_progress.progressFlags.Values.ElementAt(i) == true) // if the flag is set, then remove relevant objects
            {
                removeOnFlagTrue[i].ForEach(j => j.SetActive(false));
            }
            else // otherwise, remove relevant objects
            {
                removeOnFlagFalse[i].ForEach(j => j.SetActive(false));
            }
        }
    }



}

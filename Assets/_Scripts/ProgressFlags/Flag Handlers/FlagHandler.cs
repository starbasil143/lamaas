using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

public class FlagHandler : MonoBehaviour
{
    private PlayerProgress _progress;



    public List<string> flagsMustBeTrue;
    public List<string> flagsMustBeFalse;

    private void Awake()
    {
        _progress = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerProgress>();

        foreach (string flag in flagsMustBeTrue)
        {
            if (_progress.progressFlags[flag] == false)
            {
                Destroy(gameObject);
            }
        }
        foreach (string flag in flagsMustBeFalse)
        {
            if (_progress.progressFlags[flag] == true)
            {
                Destroy(gameObject);
            }
        }
    }



}

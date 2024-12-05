using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

public class FlagHandler : MonoBehaviour
{
    private PlayerProgress _progress;



    public List<string> flagsMustBeTrue;
    public List<string> flagsMustBeFalse;
    public bool delayDisable;

    private void Awake()
    {
        _progress = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerProgress>();

        foreach (string flag in flagsMustBeTrue)
        {
            if (_progress.progressFlags[flag] == false)
            {
                if (delayDisable)
                {
                    StartCoroutine(DeactivateLate());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
        foreach (string flag in flagsMustBeFalse)
        {
            if (_progress.progressFlags[flag] == true)
            {
                if (delayDisable)
                {
                    StartCoroutine(DeactivateLate());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private IEnumerator DeactivateLate()
    {
        yield return new WaitForSeconds(.05f);
        gameObject.SetActive(false);
    }



}

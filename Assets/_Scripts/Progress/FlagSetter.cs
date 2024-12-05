using System.Collections;
using UnityEngine;

public class FlagSetter : MonoBehaviour
{
    private PlayerProgress _progress;

    public string flagToSet;
    public bool setOnSceneLoad;

    private void Awake()
    {
        _progress = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerProgress>();
    }

    private void Start()
    {
        if (setOnSceneLoad)
        {
            StartCoroutine(SetFlagLate());
        }
    }

    public void SetFlag()
    {
        _progress.progressFlags[flagToSet] = true;
    }

    private IEnumerator SetFlagLate()
    {
        yield return new WaitForSeconds(.5f);
        SetFlag();
    }



}

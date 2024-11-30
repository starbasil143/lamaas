using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }
    [Header("Voices")]
    public EventReference voice_lithas;
    public EventReference voice_typing;
    public EventReference voice_inner;
    public EventReference voice_default;
    public EventReference voice_kid;
    public EventReference voice_nasal;
    public EventReference voice_deep;
    public EventReference voice_stupid;
    public EventReference voice_cultist;

    [Header("SFX")]
    public EventReference playerDamage;

    [Header("Ambience")]
    public EventReference ambience;

    [Header("Music")]
    public EventReference music;




    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("it broke. the fmodevents. the singleton.");
        }
    }


}

using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }
    [Header("SFX Voices")]
    public EventReference voice_lithas;
    public EventReference voice_typing;
    public EventReference voice_inner;
    public EventReference voice_default;
    public EventReference voice_kid;
    public EventReference voice_nasal;
    public EventReference voice_deep;
    public EventReference voice_stupid;
    public EventReference voice_cultist;

    [Header("SFX Transmutations")]
    public EventReference sfx_dirtthrow;
    public EventReference sfx_dirtthrowhit;
    public EventReference sfx_dirtslam;
    public EventReference sfx_vinestart;
    public EventReference sfx_vineend;
    public EventReference sfx_icespike;
    public EventReference sfx_icetomb;
    public EventReference sfx_icetombshatter;
    public EventReference sfx_heal;
    public EventReference sfx_freeze;
    public EventReference sfx_thornbomb;

    [Header("SFX Enemies")]
    public EventReference sfx_deer1;
    public EventReference sfx_deer2;
    public EventReference sfx_bear1;
    public EventReference sfx_bear2;

    [Header("SFX UI")]
    public EventReference sfx_startgame;
    public EventReference sfx_select;
    

    [Header("SFX General")]
    public EventReference sfx_xp;
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

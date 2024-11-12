using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Objects/TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public bool slot1Exists;
    public bool slot2Exists;
    public bool slot3Exists;
    public bool slot4Exists;


    public TransmutationSOBase transmutation1;
    public TransmutationSOBase transmutation2;
    public TransmutationSOBase transmutation3;
    public TransmutationSOBase transmutation4;



}

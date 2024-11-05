using UnityEngine;

[CreateAssetMenu(fileName = "MaterialData", menuName = "Scriptable Objects/MaterialData")]
public class MaterialData : ScriptableObject
{
    public bool slot1Exists;
    public bool slot2Exists;
    public bool slot3Exists;
    public bool slot4Exists;


    public TransmutationSOBase transmutation1;
    public TransmutationSOBase transmutation2;
    public TransmutationSOBase transmutation3;
    public TransmutationSOBase transmutation4;
}

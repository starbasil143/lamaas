using UnityEngine;

[CreateAssetMenu(fileName = "TransmutationSOBase", menuName = "Scriptable Objects/TransmutationSOBase")]
public class TransmutationSOBase : ScriptableObject
{
    public virtual void PerformTransmutation(GameObject player) { }
    

}

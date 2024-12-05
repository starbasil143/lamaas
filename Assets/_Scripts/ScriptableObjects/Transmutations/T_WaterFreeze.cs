using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "T_WaterFreeze", menuName = "Scriptable Objects/Transmutations/T_WaterFreeze")]
public class T_WaterFreeze : TransmutationSOBase
{
    private Tilemap waterMap;
    [SerializeField] private int freezeRadius;
    public override void PerformTransmutation(GameObject player)
    {
        base.PerformTransmutation(player);
        
        waterMap = player.GetComponentInChildren<PlayerCasting>().currentObject.GetComponent<Tilemap>();

        Vector3Int currentTilePosition = waterMap.WorldToCell(player.transform.position);

        List<Vector3Int> positionsToDestroy = new List<Vector3Int> 
        {
            currentTilePosition
        };
        
        for (int i = -freezeRadius; i <= freezeRadius; i++)
        {
            for (int j = -freezeRadius; j <= freezeRadius; j++)
            {
                positionsToDestroy.Add(currentTilePosition + new Vector3Int(i,j,0));
            }
        }
        
        foreach (Vector3Int positionToDestroy in positionsToDestroy)
        {
            waterMap.SetTile(positionToDestroy, null);
        }
        

    }


    
}

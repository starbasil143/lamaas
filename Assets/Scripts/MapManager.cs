using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;


    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    public TileData GetCurrentTile()
    {
        Vector3Int currentTilePosition = map.WorldToCell(transform.position);
        TileBase currentTile = map.GetTile(currentTilePosition);

        string[] tileMaterials = dataFromTiles[currentTile].materialTypes;


        return dataFromTiles[currentTile];
    }
}

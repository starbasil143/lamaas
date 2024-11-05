using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCasting : MonoBehaviour
{
    public Player _player;
    [SerializeField] private Tilemap map;
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;
    [SerializeField] private List<MaterialData> materialDatas;
    private Dictionary<MaterialData, bool[]> unlocksPerMaterial;

    private TileData currentTile;

    private float cooldown1 = 0f;
    private float cooldown2 = 0f;
    private float cooldown3 = 0f;
    private float cooldown4 = 0f;

    private void Awake()
    {
        _player = gameObject.GetComponent<Player>();
        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
        unlocksPerMaterial = new Dictionary<MaterialData, bool[]>();
        foreach (MaterialData materialData in materialDatas)
        {
            unlocksPerMaterial.Add(materialData, new bool[] {true, true, true, true});
        }
    }

    public TileData GetCurrentTile()
    {
        Vector3Int currentTilePosition = map.WorldToCell(transform.position);
        TileBase currentTile = map.GetTile(currentTilePosition);

        MaterialData tileSuperMaterial = dataFromTiles[currentTile].superMaterial;
        MaterialData tileSubMaterial = dataFromTiles[currentTile].subMaterial;


        return dataFromTiles[currentTile];
    }


    void Update()
    {
        currentTile = GetCurrentTile();
        MaterialData tileSuperMaterial = currentTile.superMaterial;
        MaterialData tileSubMaterial = currentTile.subMaterial;
        //bool unlocked1 = unlocksPerMaterial[tileSuperMaterial][0];
        //bool unlocked2 = unlocksPerMaterial[tileSubMaterial][1];
        //bool unlocked3 = unlocksPerMaterial[tileSubMaterial][2];
        //bool unlocked4 = unlocksPerMaterial[tileSubMaterial][3];

        if (InputManager.Slot1) // R - SuperMaterial Attack
        {
            if (cooldown1 <= 0 && tileSuperMaterial.slot1Exists && unlocksPerMaterial[tileSuperMaterial][0])
            {
                tileSuperMaterial.transmutation1.PerformTransmutation(gameObject);
            }
        }
        if (InputManager.Slot2) // F - SubMaterial Attack
        {
            if (cooldown2 <= 0 && unlocksPerMaterial[tileSubMaterial][1])
            {
                
            }
        }
        if (InputManager.Slot3) // T - SubMaterial Special 1
        {
            if (cooldown3 <= 0 && unlocksPerMaterial[tileSubMaterial][2])
            {
                
            }
        }
        if (InputManager.Slot4) // G - SubMaterial Special 2
        {
            if (cooldown4 <= 0 && unlocksPerMaterial[tileSubMaterial][3])
            {
                
            }
        }


        if (cooldown1 > 0)
            cooldown1 -= Time.deltaTime;
        if (cooldown2 > 0)
            cooldown2 -= Time.deltaTime;
        if (cooldown3 > 0)
            cooldown3 -= Time.deltaTime;
        if (cooldown4 > 0)
            cooldown4 -= Time.deltaTime;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerCasting : MonoBehaviour
{
    public Player _player;
    [SerializeField] private Tilemap map;
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;
    [SerializeField] private List<MaterialData> materialDatas;
    private Dictionary<MaterialData, bool[]> unlocksPerMaterial;

    public GameObject SlotOneImage;
    public GameObject SlotTwoImage;
    public GameObject SlotThreeImage;
    public GameObject SlotFourImage;

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

    void Start()
    {
        SlotOneImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile().superMaterial][0]);
        SlotTwoImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile().subMaterial][1]);
        SlotThreeImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile().subMaterial][2]);
        SlotFourImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile().subMaterial][3]);
    }

    public TileData GetCurrentTile()
    {
        Vector3Int currentTilePosition = map.WorldToCell(transform.position);
        TileBase currentTile = map.GetTile(currentTilePosition);

        return dataFromTiles[currentTile];
    }


    void Update()
    {
        if (GetCurrentTile() != currentTile)
        {
            SwitchTile();
        }
        MaterialData tileSuperMaterial = currentTile.superMaterial;
        MaterialData tileSubMaterial = currentTile.subMaterial;
        //bool unlocked1 = unlocksPerMaterial[tileSuperMaterial][0];
        //bool unlocked2 = unlocksPerMaterial[tileSubMaterial][1];
        //bool unlocked3 = unlocksPerMaterial[tileSubMaterial][2];
        //bool unlocked4 = unlocksPerMaterial[tileSubMaterial][3];

        if (InputManager.Slot1) // R - SuperMaterial Attack
        {
            if (cooldown1 <= 0 && tileSuperMaterial.slot1Exists && unlocksPerMaterial[tileSuperMaterial][0] && tileSuperMaterial.slot1Exists)
            {
                tileSuperMaterial.transmutation1.PerformTransmutation(gameObject);
                // unlocksPerMaterial[tileSubMaterial][3] = !unlocksPerMaterial[tileSubMaterial][3];  debug stuff
                // Debug.Log(unlocksPerMaterial[tileSubMaterial][3]);
            }
        }
        if (InputManager.Slot2) // F - SubMaterial Attack
        {
            if (cooldown2 <= 0 && unlocksPerMaterial[tileSubMaterial][1] && tileSubMaterial.slot2Exists)
            {
                tileSubMaterial.transmutation2.PerformTransmutation(gameObject);
            }
        }
        if (InputManager.Slot3) // T - SubMaterial Special 1
        {
            if (cooldown3 <= 0 && unlocksPerMaterial[tileSubMaterial][2] && tileSubMaterial.slot3Exists)
            {
                tileSubMaterial.transmutation3.PerformTransmutation(gameObject);
            }
        }
        if (InputManager.Slot4) // G - SubMaterial Special 2
        {
            if (cooldown4 <= 0 && unlocksPerMaterial[tileSubMaterial][3] && tileSubMaterial.slot4Exists)
            {
                tileSubMaterial.transmutation4.PerformTransmutation(gameObject);
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

    private void SwitchTile()
    {
        currentTile = GetCurrentTile();
        
        MaterialData tileSuperMaterial = currentTile.superMaterial;
        MaterialData tileSubMaterial = currentTile.subMaterial;


        SlotOneImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[tileSuperMaterial][0] && tileSuperMaterial.slot1Exists);
        SlotTwoImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[tileSubMaterial][1] && tileSubMaterial.slot2Exists);
        SlotThreeImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[tileSubMaterial][2] && tileSubMaterial.slot3Exists);
        SlotFourImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[tileSubMaterial][3] && tileSubMaterial.slot4Exists);

        
        
        
        Debug.Log("Switching to " + currentTile);
    }
}

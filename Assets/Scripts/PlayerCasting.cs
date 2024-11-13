using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerCasting : MonoBehaviour
{
    public Player _player;
    [SerializeField] private Tilemap map; // reference to the Ground tilemap
    [SerializeField] private List<TileData> tileDatas; // list of all tile types
    private Dictionary<TileBase, TileData> dataFromTiles; // list of tiles paired with tile types
    private Dictionary<TileData, bool[]> unlocksPerMaterial; // list of each material and whether each of their four transmutations are unlocked
    private Dictionary<string, bool> unlocksPerObject;

    // transmutation slot ui images
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
        _player = gameObject.GetComponent<Player>(); // get reference to Player.cs script
        
        // fill the dataFromTiles dictionary and the unlocksPerMaterial dictionary

        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDatas) 
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
        unlocksPerMaterial = new Dictionary<TileData, bool[]>();
        foreach (TileData tileData in tileDatas)
        {
            unlocksPerMaterial.Add(tileData, new bool[] {true, true, true, true});
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        map = GameObject.FindGameObjectWithTag("Grid").transform.Find("Ground").GetComponent<Tilemap>();
    }

    void Start()
    {
        SlotOneImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile()][0]);
        SlotTwoImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile()][1]);
        SlotThreeImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile()][2]);
        SlotFourImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile()][3]);
    }

    public TileData GetCurrentTile()
    {
        // Get current position, find its tile, and return its tile type.
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
        

        if (InputManager.Slot1) // R - SuperMaterial Attack
        {
            if (cooldown1 <= 0 && currentTile.slot1Exists && unlocksPerMaterial[currentTile][0] && currentTile.slot1Exists)
            {
                currentTile.transmutation1.PerformTransmutation(gameObject);
            }
        }
        if (InputManager.Slot2) // F - SubMaterial Attack
        {
            if (cooldown2 <= 0 && unlocksPerMaterial[currentTile][1] && currentTile.slot2Exists)
            {
                currentTile.transmutation2.PerformTransmutation(gameObject);
            }
        }
        if (InputManager.Slot3) // T - SubMaterial Special 1
        {
            if (cooldown3 <= 0 && unlocksPerMaterial[currentTile][2] && currentTile.slot3Exists)
            {
                currentTile.transmutation3.PerformTransmutation(gameObject);
            }
        }
        if (InputManager.Slot4) // G - SubMaterial Special 2
        {
            /*
            if (cooldown4 <= 0 && unlocksPerObject[currentObject] && currentObject.slot4Exists)
            {
                currentObject.PerformTransmutation(gameObject);
            }*/
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
        currentTile = GetCurrentTile(); // Switch current tile
        
        

        // Update transmutation icons based off of currently available transmutations
        SlotOneImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[currentTile][0] && currentTile.slot1Exists);
        SlotTwoImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[currentTile][1] && currentTile.slot2Exists);
        SlotThreeImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[currentTile][2] && currentTile.slot3Exists);
        SlotFourImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[currentTile][3] && currentTile.slot4Exists);

        
        
        
        Debug.Log("Switching to " + currentTile);
    }


}

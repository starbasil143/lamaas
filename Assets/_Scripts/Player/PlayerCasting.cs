using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private TileData defaultTileData;
    [SerializeField] private List<string> objectNames;
    private Dictionary<TileBase, TileData> dataFromTiles; // list of tiles paired with tile types
    private Dictionary<TileData, bool[]> unlocksPerMaterial; // list of each material and whether each of their four transmutations are unlocked
    
    private Dictionary <TileData, float[]> cooldownsPerMaterial;
    private Dictionary<string, bool> unlocksPerObject;

    // transmutation slot ui images
    public GameObject SlotOneImage;
    public GameObject SlotTwoImage;
    public GameObject SlotThreeImage;
    public GameObject SlotFourImage;

    private TileData currentTile;
    public GameObject currentObject;
    public List<GameObject> objectsInRangeList;

    private float cooldown1 = 0f;
    private float cooldown2 = 0f;
    private float cooldown3 = 0f;
    private float cooldown4 = 0f;

    private GameObject PlayerParent;

    public bool unlockAllTransmutations;
    


    private void Awake()
    {
        PlayerParent = transform.parent.gameObject;
        _player = PlayerParent.GetComponentInChildren<Player>(); // get reference to Player.cs script
        objectsInRangeList = new List<GameObject>();
        
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
            if (unlockAllTransmutations)
            {
                unlocksPerMaterial.Add(tileData, new bool[] {true, true, true, true});
            }
            else
            {
                unlocksPerMaterial.Add(tileData, new bool[] {false, false, false, false});
            }

        }
        unlocksPerMaterial[tileDatas[1]][0] = true; // auto unlock dirt throw
        cooldownsPerMaterial = new Dictionary<TileData, float[]>();
        foreach (TileData tileData in tileDatas)
        {
            cooldownsPerMaterial.Add(tileData, new float[] {0, 0, 0, 0});
        }
        unlocksPerObject = new Dictionary<string, bool>();
        foreach (string objectName in objectNames)
        {
            unlocksPerObject.Add(objectName, unlockAllTransmutations);
        }
    }

    public void CreateSaveData_PlayerCasting()
    {
        foreach (TileData tileData in tileDatas)
        {
            PlayerPrefs.SetInt(tileData.name + "0", unlocksPerMaterial[tileData][0]?1:0);
            PlayerPrefs.SetInt(tileData.name + "1", unlocksPerMaterial[tileData][1]?1:0);
            PlayerPrefs.SetInt(tileData.name + "2", unlocksPerMaterial[tileData][2]?1:0);
            PlayerPrefs.SetInt(tileData.name + "3", unlocksPerMaterial[tileData][3]?1:0);
        }
        foreach (string objName in objectNames)
        {
            PlayerPrefs.SetInt(objName, unlocksPerObject[objName]?1:0);
        }
    }

    public void LoadSaveData_PlayerCasting()
    {
        foreach (TileData tileData in tileDatas)
        {
            unlocksPerMaterial[tileData][0] = PlayerPrefs.GetInt(tileData.name + "0") == 1;
            unlocksPerMaterial[tileData][1] = PlayerPrefs.GetInt(tileData.name + "1") == 1;
            unlocksPerMaterial[tileData][2] = PlayerPrefs.GetInt(tileData.name + "2") == 1;
            unlocksPerMaterial[tileData][3] = PlayerPrefs.GetInt(tileData.name + "3") == 1;
        }
        foreach (string objName in objectNames)
        {
            unlocksPerObject[objName] = PlayerPrefs.GetInt(objName) == 1;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        PlayerSaveDataManager.onSaveData += CreateSaveData_PlayerCasting;
        PlayerSaveDataManager.onLoadData += LoadSaveData_PlayerCasting;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        PlayerSaveDataManager.onSaveData -= CreateSaveData_PlayerCasting;
        PlayerSaveDataManager.onLoadData -= LoadSaveData_PlayerCasting;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            return;
        }

        map = GameObject.FindGameObjectWithTag("Grid").transform.Find("Ground").GetComponent<Tilemap>();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            return;
        }
        SlotOneImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile()][0]);
        SlotTwoImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile()][1]);
        SlotThreeImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[GetCurrentTile()][2]);
        SlotFourImage.GetComponent<Animator>().SetBool("SlotIsEnabled",!(currentObject == null) && unlocksPerObject[currentObject.GetComponent<TransmutationObject>().name]);
    }

    public TileData GetCurrentTile()
    {
        // Get current position, find its tile, and return its tile type.
        Vector3Int currentTilePosition = map.WorldToCell(transform.position);
        TileBase currentTile = map.GetTile(currentTilePosition);
        if (currentTile != null)
        {
            return dataFromTiles[currentTile];
        }
        else
        {
            return defaultTileData;
        }
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            return;
        }
        if (GetCurrentTile() != currentTile)
        {
            SwitchTile();
        }
        if (objectsInRangeList.Count > 0)
        {
            if (objectsInRangeList.Count > 1)
            {
                SwitchObject(FindClosestObject());
            }
            else
            {
                SwitchObject(objectsInRangeList[0]);
            }
        }
        else
        {
            SwitchObject(null);
        }

        if (InputManager.Slot1 && !_player.isPaused) // R - SuperMaterial Attack
        {
            if (cooldownsPerMaterial[currentTile][0] <= 0 && unlocksPerMaterial[currentTile][0] && currentTile.slot1Exists)
            {
                currentTile.transmutation1.PerformTransmutation(PlayerParent);
                cooldownsPerMaterial[currentTile][0] = currentTile.transmutation1.cooldown;
            }
        }
        if (InputManager.Slot2 && !_player.isPaused) // F - SubMaterial Attack
        {
            if (cooldownsPerMaterial[currentTile][1] <= 0 && unlocksPerMaterial[currentTile][1] && currentTile.slot2Exists)
            {
                currentTile.transmutation2.PerformTransmutation(PlayerParent);
                cooldownsPerMaterial[currentTile][1] = currentTile.transmutation2.cooldown;
            }
        }
        if (InputManager.Slot3 && !_player.isPaused) // T - SubMaterial Special 1
        {
            if (cooldownsPerMaterial[currentTile][2] <= 0 && unlocksPerMaterial[currentTile][2] && currentTile.slot3Exists)
            {
                currentTile.transmutation3.PerformTransmutation(PlayerParent);
                cooldownsPerMaterial[currentTile][2] = currentTile.transmutation3.cooldown;
            }
        }
        if (InputManager.Slot4 && !_player.isPaused) // G - SubMaterial Special 2: Vine Grab
        {
            
            if (cooldown4 <= 0 && !(currentObject == null) && unlocksPerObject[currentObject.GetComponent<TransmutationObject>().objectName])
            {
                currentObject.GetComponent<TransmutationObject>().PerformTransmutation();
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

        foreach (KeyValuePair<TileData, float[]> tileData in cooldownsPerMaterial)
        {
            for (int i = 0; i < 3; i++)
            {
                if (tileData.Value[i] > 0)
                {
                    tileData.Value[i] -= Time.deltaTime;
                }
            }
        }
    }

    private void SwitchTile()
    {
        currentTile = GetCurrentTile(); // Switch current tile
        
        

        // Update transmutation icons based off of currently available transmutations
        SlotOneImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[currentTile][0] && currentTile.slot1Exists);
        SlotTwoImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[currentTile][1] && currentTile.slot2Exists);
        SlotThreeImage.GetComponent<Animator>().SetBool("SlotIsEnabled",unlocksPerMaterial[currentTile][2] && currentTile.slot3Exists);
        
        
    }

    public void LearnSpell(TileData tileData, int index)
    {
        unlocksPerMaterial[tileData][index] = true;
        SwitchTile();
    }

    public void LearnObjectSpell(string objectName)
    {
        if (unlocksPerObject.ContainsKey(objectName))
        {
            unlocksPerObject[objectName] = true;
        }
        else
        {
            Debug.LogWarning("that is NOT a real object");
        }
    }
    public bool KnowsSpell(TileData td, int i)
    {
        return unlocksPerMaterial[td][i];
    }

    public bool KnowsObjectSpell(string objectName)
    {
        return unlocksPerObject[objectName];
    }

    #region Object Transmutation 

    public void SwitchObject(GameObject newObject)
    {
        currentObject = newObject;
        SlotFourImage.GetComponent<Animator>().SetBool("SlotIsEnabled",!(currentObject == null) && unlocksPerObject[currentObject.GetComponent<TransmutationObject>().objectName]);
    }
    public bool hasObjectUnlocked(string objectToCheck)
    {
        return unlocksPerObject[objectToCheck];
    }
    public void addObjectToInRangeList(GameObject objectToAdd)
    {
        objectsInRangeList.Add(objectToAdd);
    }
    public void removeObjectFromInRangeList(GameObject objectToRemove)
    {
        objectsInRangeList.Remove(objectToRemove);
    }

    private GameObject FindClosestObject()
    {
        GameObject closestObject = null;
        float closestMagnitude = 143143143;
        foreach (GameObject availableObject in objectsInRangeList)
        {
            if (Mathf.Abs((availableObject.transform.position - PlayerParent.transform.position).sqrMagnitude) < closestMagnitude)
            {
                closestObject = availableObject;
                closestMagnitude = Mathf.Abs((availableObject.transform.position - PlayerParent.transform.position).sqrMagnitude);
            }
        }
        return closestObject;
    }
    #endregion

    

    

}

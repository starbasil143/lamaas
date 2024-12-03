using UnityEngine;

public class SaverLoaderTest : MonoBehaviour
{
    public void SaveMaybe()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerSaveDataManager>().SaveGame();
    }

    public void LoadMaybe()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerSaveDataManager>().LoadGame();
    }
}

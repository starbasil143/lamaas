using UnityEngine;

public class PlayerSaveDataManager : MonoBehaviour
{
    public delegate void OnSaveData();
    public static event OnSaveData onSaveData;

    public delegate void OnLoadData();
    public static event OnLoadData onLoadData;

    // yes i am using playerprefs. you cant stop me

    public void SaveGame()
    {
        onSaveData?.Invoke();
    }

    public void LoadGame()
    {
        onLoadData?.Invoke();
    }
}

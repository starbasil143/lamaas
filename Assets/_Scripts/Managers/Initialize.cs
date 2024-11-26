using UnityEngine;

public class Initialize
{
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Debug.Log("Persistence Loaded (Initializer Script)");
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PERSISTENT")));
    }
}

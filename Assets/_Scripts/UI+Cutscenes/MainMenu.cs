using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool hasSaveData;
    [SerializeField] private GameObject ConfirmNewGamePanel;
    [SerializeField] private GameObject NoSaveGame;
    public GameObject openingCutscene;
    public GameObject openingCutsceneCanvas;
    public GameObject lookiesImage;
    public GameObject buttons;
    

    private void Awake()
    {
        hasSaveData = PlayerPrefs.GetInt("openingScene") == 1;

        
    }
    private void Start()
    {
        AudioManager.instance.SetMusicArea(MusicArea.TITLE);
    }

    private void Update()
    {
        if (InputManager.Advance || InputManager.Interact || InputManager.ToggleMenu)
        {
            openingCutscene.SetActive(false);
            openingCutsceneCanvas.SetActive(false);
        }
    }
    
    public void StartGameButton()
    {
        PlayerPrefs.DeleteAll();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_startgame, transform.position);
        StartCoroutine(Lookies());
    }

    public void ContinueButton()
    {
        if (hasSaveData)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>().LoadGameNow();
            StartCoroutine(Lookies());
            AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_startgame, transform.position);
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_select, transform.position);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("the quitterrrr");
    }

    private IEnumerator Lookies()
    {
        buttons.SetActive(false);
        lookiesImage.GetComponent<Animator>().Play("MenuAnim");
        yield return new WaitForSeconds(1.1f);
        GoToHouse();
    }

    private void GoToHouse()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>().isPaused = false;
        GetComponent<NonEntrySceneChange>().GoToSceneAtPosition();
    }
    

    public void ConfirmStartGame()
    {

    }


}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject spellListPanel;

    public Button pauseButton;
    public Button exitPauseButton;

    [SerializeField] Transform spellList;
    [SerializeField] Transform spellInformation;

    [SerializeField] Image spellInformationImage;

    public TextMeshProUGUI[] spellInformationText;

    public CanvasGroup background;
    private GameObject player;

    void Start()
    {
        if (pauseMenuPanel != null || spellListPanel != null)
        {
            CloseAllMenus();
        }
    }

    void Update()
    {
        // Toggle pause menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        // Toggle pause menu visibility
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);

        // Pause or unpause the game
        Time.timeScale = pauseMenuPanel.activeSelf ? 0f : 1f;
    }

    public void ShowSpellList()
    {
        // Hide main pause menu
        pauseMenuPanel.SetActive(false);

        // Show spell list panel
        spellListPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        // Unpause the game before loading
        CloseAllMenus();
        Time.timeScale = 1f;

        // Load main menu scene
        SceneManager.LoadScene("MainMenu"); // Replace with main menu scene name
    }

    public void CloseSpellList()
    {
        // Hide spell list
        spellListPanel.SetActive(false);

        // Show main pause menu
        pauseMenuPanel.SetActive(true);
    }

    public void CloseAllMenus()
    {
        spellListPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }
}
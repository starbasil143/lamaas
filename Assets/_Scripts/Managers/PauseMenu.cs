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
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        pauseButton.onClick.AddListener(TogglePauseMenu);
        exitPauseButton.onClick.AddListener(TogglePauseMenu);
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

    public void ResumeGame()
    {
        // Close pause menu
        pauseMenuPanel.SetActive(false);

        // Unpause game
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        // Unpause the game before loading
        Time.timeScale = 1f;

        // Load main menu scene
        //UnityEngine.SceneManagement.LoadScene("MainMenu"); // Replace with your main menu scene name
    }

    public void CloseSpellList()
    {
        // Hide spell list
        spellListPanel.SetActive(false);

        // Show main pause menu
        pauseMenuPanel.SetActive(true);
    }
}
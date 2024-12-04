using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Diagnostics;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;

    private GameObject player;

    void Start()
    {
        if (pauseMenuPanel != null || settingsPanel != null)
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

    public void ReturnToMainMenu()
    {
        // Unpause the game before loading
        CloseAllMenus();
        Time.timeScale = 1f;

        // Load main menu scene
        SceneManager.LoadScene("MainMenu"); // Replace with main menu scene name
    }

    public void ToggleSettings()
    {
        // Toggle settings menu visibility
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void CloseAllMenus()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
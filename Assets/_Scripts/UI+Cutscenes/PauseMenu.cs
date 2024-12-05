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

    public List<GameObject> subMenuPanels;

    public bool isPaused;
    private GameObject player;

    void Start()
    {
        if (pauseMenuPanel != null || settingsPanel != null)
        {
            CloseAllMenus();
        }
    }


    public void TogglePauseMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            CloseAllMenus();
            Time.timeScale = 1f;
        }

        
    }

    public void ReturnToMainMenu()
    {
        CloseAllMenus();
        Time.timeScale = 1f;

        //SceneManager.LoadScene("MainMenu"); // Replace with main menu scene name
    }

    public void CloseAllMenus()
    {
        foreach (GameObject subMenuPanel in subMenuPanels)
        {
            subMenuPanel.SetActive(false);
        }
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);

    }
}
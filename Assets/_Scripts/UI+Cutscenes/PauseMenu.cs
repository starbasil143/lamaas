using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    public List<GameObject> subMenuPanels;
    public TextMeshProUGUI expText;

    public bool isPaused;
    private GameObject _player;

    void Start()
    {
        CloseAllMenus();
        _player = GameObject.FindGameObjectWithTag("Player");
    }


    public void TogglePauseMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            _player.GetComponentInChildren<Player>().isPaused = true;
            expText.text = _player.GetComponentInChildren<Player>().GetExpAmount().ToString();
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            _player.GetComponentInChildren<Player>().isPaused = false;
            CloseAllMenus();
            Time.timeScale = 1f;
        }

        
    }

    public void BackOut()
    {
        foreach (GameObject subMenuPanel in subMenuPanels)
        {
            if (subMenuPanel.activeSelf)
            {
                subMenuPanel.SetActive(false);
                return;
            }
        }
        TogglePauseMenu();
    }

    public void ReturnToMainMenu()
    {
        CloseAllMenus();
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenuScene"); // Replace with main menu scene name
    }

    public void CloseAllMenus()
    {
        foreach (GameObject subMenuPanel in subMenuPanels)
        {
            subMenuPanel.SetActive(false);
        }
        pauseMenuPanel.SetActive(false);

    }
}
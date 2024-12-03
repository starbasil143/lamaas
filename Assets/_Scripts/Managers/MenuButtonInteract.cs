using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using System;

public class MenuButtonInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Original button properties
    private Vector3 originalScale;

    // Hover effect properties
    public float hoverScaleMultiplier = 1.2f;

    public Button button;

    void Start()
    {
        // Get the button component
        button = GetComponent<Button>();

        // Store original scale
        originalScale = transform.localScale;
    }

    // When mouse enters button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Scale up the button
        transform.localScale = originalScale * hoverScaleMultiplier;

        //UnityEngine.Debug.Log("Mouse on Button");
    }

    // When mouse exits button area
    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore original scale
        transform.localScale = originalScale;

        //UnityEngine.Debug.Log("Mouse off Button");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("TownScene");
    }

    public void ContinueGame()
    {
        UnityEngine.Debug.Log("Continue Game button pressed");
    }
    
    public void ExitGame()
    {
        UnityEngine.Application.Quit();
        //UnityEngine.Debug.Log("Exit Pressed");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
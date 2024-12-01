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
    private Color originalColor;

    // Hover effect properties
    public float hoverScaleMultiplier = 1.2f;
    public Color hoverColor = new Color(1f, 1f, 1f, 1f); // White

    public Button button;

    void Start()
    {
        // Get the button component
        button = GetComponent<Button>();

        // Store original scale and color
        originalScale = transform.localScale;
        originalColor = button.colors.normalColor;
    }

    // When mouse enters button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Scale up the button
        transform.localScale = originalScale * hoverScaleMultiplier;

        // Change button color
        ColorBlock colors = button.colors;
        colors.normalColor = hoverColor;
        button.colors = colors;

        UnityEngine.Debug.Log("Mouse on Button");
    }

    // When mouse exits button area
    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore original scale
        transform.localScale = originalScale;

        // Restore original color
        ColorBlock colors = button.colors;
        colors.normalColor = originalColor;
        button.colors = colors;

        UnityEngine.Debug.Log("Mouse off Button");
    }

    public void ExitGame()
    {
        UnityEngine.Application.Quit();
        UnityEngine.Debug.Log("Exit Pressed");
    }

    public void ButtonPressed()
    {
        UnityEngine.Debug.Log(button + " Pressed");
    }
}
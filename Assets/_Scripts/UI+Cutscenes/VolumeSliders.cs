using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public float defaultVolume = 0.5f;

    // Reference to the Audio Manager script
    public AudioManager audioManager;

    // References to the UI Sliders
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    void Start()
    {
        // Initialize sliders with current volume levels
        if (audioManager != null)
        {
            // Set initial slider values to match current audio levels
            musicVolumeSlider.value = audioManager.musicVolume;
            sfxVolumeSlider.value = audioManager.sfxVolume;

            // Add listeners to respond to slider changes
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

            // Reset volume settings on start
            audioManager.musicVolume = defaultVolume;
            audioManager.sfxVolume = defaultVolume;
            musicVolumeSlider.value = defaultVolume;
            sfxVolumeSlider.value = defaultVolume;
        }
        else
        {
            UnityEngine.Debug.LogError("Audio Manager is not assigned in Volume Controller!");
        }
    }

    // Method to update music volume
    public void SetMusicVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.musicVolume = volume;
        }
    }

    // Method to update SFX volume
    public void SetSFXVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.sfxVolume = volume;
        }
    }
}
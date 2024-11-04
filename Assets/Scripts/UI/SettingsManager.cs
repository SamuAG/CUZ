using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    // Default values
    [SerializeField] private float defaultVolume = 0f; // esta en dB
    [SerializeField] private int defaultQualityLevel = 3; // Medium Quality

    // UI Elements (TextMeshPro)
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown; // Dropdown for resolution
    [SerializeField] private Toggle fullscreenToggle; // Toggle for fullscreen

    // Audio Mixer
    [SerializeField] private AudioMixer audioMixer;

    // Exposed parameters in the AudioMixer
    private const string volumeParam = "MasterVolume";


    [SerializeField] private Resolution[] availableResolutions;

    private void Start()
    {
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;


        // Initialize UI with current settings
        InitializeGraphicsSettings();
        InitializeAudioSettings();

        if (PlayerPrefs.GetInt("FirstTime", 1) == 1)
        {
            // Set the resolution to the highest available and enable fullscreen
            SetMaxResolution();

            // Mark as not the first time run
            PlayerPrefs.SetInt("FirstTime", 0);
            PlayerPrefs.Save();

            ResetSettings();
        }
        else
        {
            // Load saved settings
            LoadSettings();
        }
    }

    private void InitializeGraphicsSettings()
    {
        // Set the quality dropdown to current quality level
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.onValueChanged.AddListener(ChangeQuality);

        // Initialize resolutions and populate dropdown
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + " x " + availableResolutions[i].height;
            resolutionOptions.Add(option);

            if (availableResolutions[i].width == Screen.currentResolution.width && availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        // Fullscreen toggle
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    private void InitializeAudioSettings()
    {
        // Get the current volume from the AudioMixer and set the sliders
        float volume;

        audioMixer.GetFloat(volumeParam, out volume);
        volumeSlider.value = volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Method to change graphics quality
    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    // Methods to set volumes
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(volumeParam, fixSoundValue(volume));
    }

    // Method to change resolution
    public void ChangeResolution(int resolutionIndex)
    {
        Resolution resolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Method to toggle fullscreen
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Method to save settings
    public void SaveSettings()
    {
        // Save graphics quality
        PlayerPrefs.SetInt("QualityLevel", QualitySettings.GetQualityLevel());

        // Save volumes
        float volume;

        audioMixer.GetFloat(volumeParam, out volume);
        PlayerPrefs.SetFloat(volumeParam, volume);

        // Save resolution settings
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);

        // Save fullscreen setting
        PlayerPrefs.SetInt("Fullscreen", Screen.fullScreen ? 1 : 0);

        PlayerPrefs.Save();
    }

    // Method to load settings
    public void LoadSettings()
    {
        // Load and apply graphics quality
        int qualityLevel = PlayerPrefs.GetInt("QualityLevel", defaultQualityLevel);
        ChangeQuality(qualityLevel);
        qualityDropdown.value = qualityLevel;

        // Load and apply volumes
        float volume;

        volume = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);
        SetVolume(inverseFixSoundValue(volume));
        volumeSlider.value = inverseFixSoundValue(volume);

        // Load and apply resolution settings
        int resolutionIndex = PlayerPrefs.GetInt("Resolution", 0);
        ChangeResolution(resolutionIndex);
        resolutionDropdown.value = resolutionIndex;

        // Load and apply fullscreen setting
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        SetFullScreen(isFullscreen);
        fullscreenToggle.isOn = isFullscreen;
    }

    // Optional: Reset settings to default values
    public void ResetSettings()
    {
        // Reset to default quality level
        ChangeQuality(defaultQualityLevel);
        qualityDropdown.value = defaultQualityLevel;

        // Reset volumes to default (0dB)
        SetVolume(defaultVolume);
        volumeSlider.value = inverseFixSoundValue(defaultVolume);

        // Reset resolution to the default screen resolution
        resolutionDropdown.value = Array.FindIndex(availableResolutions, res => res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height);
        ChangeResolution(resolutionDropdown.value);

        // Reset fullscreen to the default value
        fullscreenToggle.isOn = true;
        SetFullScreen(true);

        // Save the reset settings
        SaveSettings();
    }

    private void SetMaxResolution()
    {
        // Set the resolution to the highest available resolution
        availableResolutions = Screen.resolutions;
        Resolution maxResolution = availableResolutions[availableResolutions.Length - 1];
        Screen.SetResolution(maxResolution.width, maxResolution.height, true);

        // Update UI elements
        resolutionDropdown.value = availableResolutions.Length - 1;
        fullscreenToggle.isOn = true;
        SetFullScreen(true);
    }

    public float fixSoundValue(float sliderValue)
    {
        // Verificar que el valor est� en el rango [0, 1]
        if (sliderValue < 0 || sliderValue > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(sliderValue), "El valor del slider debe estar entre 0 y 1.");
        }

        // Puntos de control
        float[] sliderPositions = { 0f, 0.25f, 0.5f, 1f };
        float[] outputValues = { -80f, -20f, 0f, 20f };

        // Interpolaci�n lineal
        if (sliderValue <= 0.25f)
        {
            // Interpolaci�n entre -80 y -20
            return Mathf.Lerp(-80f, -20f, sliderValue / 0.25f);
        }
        else if (sliderValue <= 0.5f)
        {
            // Interpolaci�n entre -20 y 0
            return Mathf.Lerp(-20f, 0f, (sliderValue - 0.25f) / 0.25f);
        }
        else
        {
            // Interpolaci�n entre 0 y 20
            return Mathf.Lerp(0f, 20f, (sliderValue - 0.5f) / 0.5f);
        }
    }

    public float inverseFixSoundValue(float value)
    {
        // Verificar que el valor est� en el rango [-80, 20]
        if (value < -80f || value > 20f)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "El valor debe estar entre -80 y 20.");
        }

        // Desinterpolar
        if (value <= -20f)
        {
            // Interpolaci�n inversa entre -80 y -20
            return (value + 80f) / 60f * 0.25f; // Devuelve valor entre 0 y 0.25
        }
        else if (value <= 0f)
        {
            // Interpolaci�n inversa entre -20 y 0
            return 0.25f + ((value + 20f) / 20f * 0.25f); // Devuelve valor entre 0.25 y 0.5
        }
        else
        {
            // Interpolaci�n inversa entre 0 y 20
            return 0.5f + ((value / 20f) * 0.5f); // Devuelve valor entre 0.5 y 1
        }
    }


}
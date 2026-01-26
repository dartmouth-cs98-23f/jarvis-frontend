using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;

public class SettingsManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;
    
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle muteToggle;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Text currentServerText;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string MUTE_KEY = "Mute";
    private const string QUALITY_KEY = "Quality";
    private const string FULLSCREEN_KEY = "Fullscreen";

    void Start()
    {
        LoadSettings();
    }

    void OnEnable()
    {
        LoadSettings();
        UpdateCurrentServerDisplay();
    }

    private void LoadSettings()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1.0f);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        if (muteToggle != null)
        {
            muteToggle.isOn = PlayerPrefs.GetInt(MUTE_KEY, 0) == 1;
            muteToggle.onValueChanged.AddListener(OnMuteChanged);
        }

        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
            qualityDropdown.value = PlayerPrefs.GetInt(QUALITY_KEY, QualitySettings.GetQualityLevel());
            qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = PlayerPrefs.GetInt(FULLSCREEN_KEY, Screen.fullScreen ? 1 : 0) == 1;
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        }
    }

    private void UpdateCurrentServerDisplay()
    {
        if (currentServerText != null)
        {
            currentServerText.text = "Current Server: " + ServerConfig.CurrentServerAddress;
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
        PlayerPrefs.Save();
        AudioListener.volume = muteToggle != null && muteToggle.isOn ? 0 : value;
        Debug.Log("Music volume set to: " + value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
        PlayerPrefs.Save();
        Debug.Log("SFX volume set to: " + value);
    }

    public void OnMuteChanged(bool isMuted)
    {
        PlayerPrefs.SetInt(MUTE_KEY, isMuted ? 1 : 0);
        PlayerPrefs.Save();
        AudioListener.volume = isMuted ? 0 : (musicVolumeSlider != null ? musicVolumeSlider.value : 1.0f);
        Debug.Log("Mute set to: " + isMuted);
    }

    public void OnQualityChanged(int qualityIndex)
    {
        PlayerPrefs.SetInt(QUALITY_KEY, qualityIndex);
        PlayerPrefs.Save();
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Quality set to: " + QualitySettings.names[qualityIndex]);
    }

    public void OnFullscreenChanged(bool isFullscreen)
    {
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen set to: " + isFullscreen);
    }

    public void OnResetSettingsButtonPressed()
    {
        PlayerPrefs.DeleteKey(MUSIC_VOLUME_KEY);
        PlayerPrefs.DeleteKey(SFX_VOLUME_KEY);
        PlayerPrefs.DeleteKey(MUTE_KEY);
        PlayerPrefs.DeleteKey(QUALITY_KEY);
        PlayerPrefs.DeleteKey(FULLSCREEN_KEY);
        PlayerPrefs.Save();

        LoadSettings();
        Debug.Log("Settings reset to defaults");
    }

    public void OnBackButtonPressed()
    {
        if (mainMenuManager != null)
        {
            mainMenuManager.OnBackToMainMenu();
        }
    }
}

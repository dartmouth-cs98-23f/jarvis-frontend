using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject ServerSelectionPanel;
    public GameObject SettingsPanel;

    void Start()
    {
        ServerConfig.SelectDefaultServer();
        
        if (MainMenuPanel != null)
        {
            MainMenuPanel.SetActive(true);
        }
        if (ServerSelectionPanel != null)
        {
            ServerSelectionPanel.SetActive(false);
        }
        if (SettingsPanel != null)
        {
            SettingsPanel.SetActive(false);
        }
    }

    public void OnServerSelectionButtonPressed()
    {
        MainMenuPanel.SetActive(false);
        ServerSelectionPanel.SetActive(true);
    }

    public void OnSettingsButtonPressed()
    {
        MainMenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void OnStartGameButtonPressed()
    {
        SceneNavigator.LoadOnboarding();
    }

    public void OnBackToMainMenu()
    {
        if (ServerSelectionPanel != null)
        {
            ServerSelectionPanel.SetActive(false);
        }
        if (SettingsPanel != null)
        {
            SettingsPanel.SetActive(false);
        }
        MainMenuPanel.SetActive(true);
    }
}

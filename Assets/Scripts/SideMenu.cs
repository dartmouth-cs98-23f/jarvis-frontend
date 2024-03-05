using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Clients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject charactersButton;
    public GameObject charactersSubMenu;
    public GameObject hamburgerIcon;
    public Button characters;
    public Button worldSettings;
    public Button exitWorld;
    public Button players;
    public Button agents;

    public GameObject worldSettingsPanel;
    public GameObject ownerWorldSettingsPanel;
    public GameObject playersListPanel;
    public GameObject hatchedPanel;
    public GameObject incubatingPanel;
    public GameObject swipePrefab;
    public GameObject createAgentPanel;
    public GameObject visualDescPanel; // for asking "what does your bot look like?"
    public GameObject confirmCreatePanel;
    public GameObject agentInfoPanel;
    public GameObject trainAgentPanel;
    public GameObject agentCollabPanel;
    public GameObject viewAnswersPanel;
    public GameObject typeAnswerPanel;
    public GameObject shadow;
    public GameObject largerShadow;
    void Start()
    {
        // Disable the submenu initially
        menuPanel.SetActive(false);
        charactersSubMenu.SetActive(false);
        worldSettingsPanel.SetActive(false);
        ownerWorldSettingsPanel.SetActive(false);
        playersListPanel.SetActive(false);
        hatchedPanel.SetActive(false);
        incubatingPanel.SetActive(false);
        swipePrefab.SetActive(false);
        createAgentPanel.SetActive(false);
        visualDescPanel.SetActive(false);
        confirmCreatePanel.SetActive(false);
        agentInfoPanel.SetActive(false);
        trainAgentPanel.SetActive(false);
        agentCollabPanel.SetActive(false);
        viewAnswersPanel.SetActive(false);
        typeAnswerPanel.SetActive(false);
    }

    void Update()
    {
        if (worldSettingsPanel.activeSelf ||
        ownerWorldSettingsPanel.activeSelf ||
        playersListPanel.activeSelf ||
        hatchedPanel.activeSelf ||
        incubatingPanel.activeSelf ||
        swipePrefab.activeSelf){
            characters.interactable = false;
            worldSettings.interactable = false;
            exitWorld.interactable = false;
            players.interactable = false;
            agents.interactable = false;
            shadow.SetActive(true);
        }
        if (createAgentPanel.activeSelf ||
        visualDescPanel.activeSelf ||
        confirmCreatePanel.activeSelf ||
        agentInfoPanel.activeSelf ||
        trainAgentPanel.activeSelf ||
        agentCollabPanel.activeSelf ||
        viewAnswersPanel.activeSelf ||
        typeAnswerPanel.activeSelf)
        {
            characters.interactable = false;
            worldSettings.interactable = false;
            exitWorld.interactable = false;
            players.interactable = false;
            agents.interactable = false;
            largerShadow.SetActive(true);
        }
        if (!worldSettingsPanel.activeSelf &&
            !ownerWorldSettingsPanel.activeSelf &&
            !playersListPanel.activeSelf &&
            !hatchedPanel.activeSelf &&
            !incubatingPanel.activeSelf &&
            !swipePrefab.activeSelf)
        {
            shadow.SetActive(false);
        }
        if (!createAgentPanel.activeSelf &&
            !visualDescPanel.activeSelf &&
            !confirmCreatePanel.activeSelf &&
            !agentInfoPanel.activeSelf &&
            !trainAgentPanel.activeSelf &&
            !agentCollabPanel.activeSelf &&
            !viewAnswersPanel.activeSelf &&
            !typeAnswerPanel.activeSelf)
        {
            largerShadow.SetActive(false);
        }
        
        if (!worldSettingsPanel.activeSelf &&
            !ownerWorldSettingsPanel.activeSelf &&
            !playersListPanel.activeSelf &&
            !hatchedPanel.activeSelf &&
            !incubatingPanel.activeSelf &&
            !swipePrefab.activeSelf &&
            !createAgentPanel.activeSelf &&
            !visualDescPanel.activeSelf &&
            !confirmCreatePanel.activeSelf &&
            !agentInfoPanel.activeSelf &&
            !trainAgentPanel.activeSelf &&
            !agentCollabPanel.activeSelf &&
            !viewAnswersPanel.activeSelf &&
            !typeAnswerPanel.activeSelf)
        {
            characters.interactable = true;
            worldSettings.interactable = true;
            exitWorld.interactable = true;
            players.interactable = true;
            agents.interactable = true;
            shadow.SetActive(false);
            largerShadow.SetActive(false);
            GameObject userPrefabInstance = GameObject.Find("UserPrefab(Clone)");
            if (userPrefabInstance != null){
                userPrefabInstance.GetComponent<PlayerMovement>().enabled = true;
            }
        }
        else {
            GameObject userPrefabInstance = GameObject.Find("UserPrefab(Clone)");
            if (userPrefabInstance != null){
                userPrefabInstance.GetComponent<PlayerMovement>().enabled = false;
            }
        }
    }

    public void ToggleMenu()
    {
        // Toggle the main menu panel's visibility
        menuPanel.SetActive(!menuPanel.activeSelf);
        hamburgerIcon.SetActive(!hamburgerIcon.activeSelf);

        // If the submenu is active, deactivate it when toggling the main menu
        if (charactersSubMenu.activeSelf)
        {
            ToggleCharactersSubMenu();
        }
    }

    public void ToggleCharactersSubMenu()
    {
        // Toggle the submenu's visibility
        charactersSubMenu.SetActive(!charactersSubMenu.activeSelf);

        // Hide the original "Characters" button when the submenu is active
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void ToggleWorldSettingsPanel()
    {
        worldSettingsPanel.SetActive(!worldSettingsPanel.activeSelf);
    }
    public void ToggleOwnerWorldSettingsPanel()
    {
        ownerWorldSettingsPanel.SetActive(!ownerWorldSettingsPanel.activeSelf);
    }

    public void TogglePlayersListPanel()
    {
        playersListPanel.SetActive(!playersListPanel.activeSelf);
    }

    public void ToggleHatchedPanel()
    {
        hatchedPanel.SetActive(!hatchedPanel.activeSelf);
        incubatingPanel.SetActive(!incubatingPanel.activeSelf);
        swipePrefab.SetActive(!swipePrefab.activeSelf);
    }

    public void ToggleCreateAgentPanel(){
        createAgentPanel.SetActive(!createAgentPanel.activeSelf);
    }

    public void ToggleVisualDescPanel(){
        visualDescPanel.SetActive(!visualDescPanel.activeSelf);
    }

    public void ToggleConfirmCreatePanel(){
        confirmCreatePanel.SetActive(!confirmCreatePanel.activeSelf);
    }
    public void ToggleAgentInfoPanel(){
        agentInfoPanel.SetActive(!agentInfoPanel.activeSelf);
    }
    public void ToggleTrainAgentPanel(){
        trainAgentPanel.SetActive(!trainAgentPanel.activeSelf);
    }
    public void ToggleAgentCollabPanel(){
        agentCollabPanel.SetActive(!agentCollabPanel.activeSelf);
    }
    public void ToggleViewAnswersPanel(){
        viewAnswersPanel.SetActive(!viewAnswersPanel.activeSelf);
    }
    public void ToggleTypeAnswerPanel(){
        typeAnswerPanel.SetActive(!typeAnswerPanel.activeSelf);
    }

    public async Task OnPressExitWorld()
    {
        try {
            await HTTPClient.Instance.LogoutUser();
        } catch (Exception e) {
            Debug.LogError($"Error logging out: {e.Message}");
        
        }
    }

    // Synchronous wrapper method
    public void OnPressExitWorldWrapper()
    {
        // Call the async method without awaiting it
        _ = OnPressExitWorld();
    }
}

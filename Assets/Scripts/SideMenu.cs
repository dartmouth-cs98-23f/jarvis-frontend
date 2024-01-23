using UnityEngine;
using TMPro;

public class SideMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject charactersButton;
    public GameObject charactersSubMenu;
    public GameObject hamburgerIcon;

    public GameObject worldSettingsPanel;
    public GameObject playersListPanel;
    public GameObject hatchedPanel;
    public GameObject incubatingPanel;
    public GameObject swipePrefab;
    

    void Start()
    {
        // Disable the submenu initially
        menuPanel.SetActive(false);
        charactersSubMenu.SetActive(false);
        worldSettingsPanel.SetActive(false);
        playersListPanel.SetActive(false);
        hatchedPanel.SetActive(false);
        incubatingPanel.SetActive(false);
        swipePrefab.SetActive(false);
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

}

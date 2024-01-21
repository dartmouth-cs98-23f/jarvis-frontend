using UnityEngine;

public class SideMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject charactersButton;
    public GameObject charactersSubMenu;
    public GameObject hamburgerIcon;


    void Start()
    {
        // Disable the submenu initially
        menuPanel.SetActive(false);
        charactersSubMenu.SetActive(false);
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
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonClickHandler : MonoBehaviour
{
    public Button worldSettings;
    public Button players;
    public Button agents;
    public GameObject menu;
    public GameObject charactersSubMenu;
    public void OnPlayersButtonClicked()
    {
        menu.SetActive(false);
        charactersSubMenu.SetActive(false);
    }
    public void OnAgentsButtonClicked()
    {
        menu.SetActive(false);
        charactersSubMenu.SetActive(false);
    }
    public void OnWorldSettingsButtonClicked()
    {
        menu.SetActive(false);
        charactersSubMenu.SetActive(false);
    }


}

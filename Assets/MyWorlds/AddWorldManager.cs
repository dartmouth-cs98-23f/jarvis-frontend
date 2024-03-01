using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddWorldManager : MonoBehaviour
{
    public GameObject addWorldPanel;
    public GameObject myWorldsPanel;
    public InputField worldCodeInputField;
    private MyWorldsManager myWorldsManager;
    public GameObject navbarPanel;
    public Text errorText;

    // Start is called before the first frame update
    void OnEnable()
    {
        myWorldsManager = myWorldsPanel.GetComponent<MyWorldsManager>();
        worldCodeInputField.text = ""; // reset code input field
        errorText.text = ""; // reset error text
    }

    public async void OnPressAdd()
    {
        // TODO: Check if world code is valid format
        string addWorldResponse = await myWorldsManager.AddWorld(worldCodeInputField.text);

        if (addWorldResponse == "success") {
            addWorldPanel.SetActive(false);
            myWorldsPanel.SetActive(true);
            navbarPanel.GetComponent<NavbarManager>().NavigateBackToMyWorlds();
        } else {
            errorText.text = addWorldResponse;
        }
    }
}

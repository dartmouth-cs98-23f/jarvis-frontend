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

    // Start is called before the first frame update
    void OnEnable()
    {
        myWorldsManager = myWorldsPanel.GetComponent<MyWorldsManager>();
        worldCodeInputField.text = ""; // reset code input field
    }

    public void OnPressAdd()
    {
        // TODO: Check if world code is valid format
        bool addedSuccessfully = myWorldsManager.AddWorld(worldCodeInputField.text);
        if (addedSuccessfully)
        {
            addWorldPanel.SetActive(false);
            myWorldsPanel.SetActive(true);
        } else 
        {
            // TODO: Add UI error message on failure to add world
            Debug.Log("Failed to add world");
        }
    }
}

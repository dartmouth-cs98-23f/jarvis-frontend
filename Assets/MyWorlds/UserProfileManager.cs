using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserProfileManager : MonoBehaviour
{
    public Text username;
    public Image userImage;
    public Text userSummary;

    public InputField userSummaryInputField;
    public GameObject inputFieldContainer;
    public GameObject summaryContainer;
    public GameObject editButton;
    public GameObject saveButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        // TODO: Connect to backend to get user data
        username.text = "@" + "testUsername";
        userImage.sprite = Resources.Load<Sprite>("Shapes/obi_wan_kenobi");
        initializeText();
    }

    private void initializeText()
    {
        // TODO: Replace this text with user's actual summary by getting it from the backend
        userSummary.text = "You are a compassionate, wise, and brave Jedi Master. You're known for your strategic mind, moral integrity, mentorship skills, patience, and a subtle sense of humor. You are a compassionate, wise, and brave Jedi Master. You're known for your strategic mind, moral integrity, mentorship skills, patience, and a subtle sense of humor.";
        userSummaryInputField.text = userSummary.text;
        userSummaryInputField.placeholder.GetComponent<Text>().text = userSummary.text;
    }

    public void OnPressSave()
    {
        editButton.SetActive(true);
        saveButton.SetActive(false);
        summaryContainer.SetActive(true);
        inputFieldContainer.SetActive(false);
        userSummary.text = userSummaryInputField.text;
        
        // TODO: Add backend save summary method
    }

    public void OnPressEdit()
    {
        editButton.SetActive(false);
        saveButton.SetActive(true);
        summaryContainer.SetActive(false);
        inputFieldContainer.SetActive(true);
    }
}
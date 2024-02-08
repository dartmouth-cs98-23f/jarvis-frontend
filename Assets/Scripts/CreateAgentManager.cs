using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CreateAgentManager : MonoBehaviour
{
    public InputField nameInputField;
    public InputField descInputField;
    public Slider incubationTime;
    public InputField agentVisualDesc;
    public GameObject confirmSpriteObject;
    public GameObject confirmNameObject;
    public GameObject confirmDescObject;
    public GameObject confirmIncObject;
    public GameObject confirmCreateContent;
    public TextMeshProUGUI sliderValueText;
    public RectTransform handleRect;
    private string name;
    private string desc;
    private float incubation;
    private string visual;

    [Serializable]
    public class AgentData
    {
        public string id;
        public string username;
        public string description;
        public string summary;
        public LocationData location;
        public string creatorId;
        public bool isHatched;
        public string sprite_URL;
        public string sprite_headshot_URL;
        public DateTime createdTime;
        public DateTime hatchTime;
    }

    [Serializable]
    public class LocationData
    {
        public int x_coord;
        public int y_coord;
    }


    public void StoreInput()
    {
        // Store text input from the username input field
        name = nameInputField.text;

        // Store text input from the password input field
        desc = descInputField.text;

        // Store incubation time from the slider
        incubation = incubationTime.value;

        // TODO: Send this information to the backend upon clicking the next button
    }

    public void StoreVisualDesc(){
        visual = agentVisualDesc.text;

        // TODO: Send this information to the backend upon clicking the next button
    }

    public void ResetInputFields()
    {
        // Reset the input fields
        nameInputField.text = "";
        descInputField.text = "";
        incubationTime.value = 0;
        agentVisualDesc.text = "";
    }

    public void FillConfirmCreateFields(){
        AgentData agent = new AgentData();
        agent.sprite_URL = "Sprites/master_yoda";
        confirmSpriteObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(agent.sprite_URL);
        confirmNameObject.GetComponent<TextMeshProUGUI>().text = name;
        confirmDescObject.GetComponent<TextMeshProUGUI>().text = desc;
        confirmIncObject.GetComponent<TextMeshProUGUI>().text = "Incubation Time: " + incubation.ToString("F0") + "h";
    }

    public void UpdateSliderValueText()
    {
        // Update the text of the Text component with the value of the slider
        sliderValueText.text = incubationTime.value.ToString() + "h"; 
    }

    private void Update()
    {
        // Update the position of the text component to follow the position of the slider handle
        Vector3 handlePosition = handleRect.position;
        Vector3 textPosition = new Vector3(handlePosition.x, handlePosition.y - 2.5f, handlePosition.z); // Adjust the offset as needed
        sliderValueText.transform.position = textPosition;
    }

    public void SendAgentInfo(){
        // TODO: Send info about agent to backend upon clicking confirm on confirm create screen
    }

    // Adjusts size of scrollable description on the confirm create panel
    public void adjustDescHeight(){
        float preferredHeight = confirmDescObject.GetComponent<TextMeshProUGUI>().preferredHeight;
        RectTransform contentRectTransform = confirmCreateContent.GetComponent<RectTransform>();

        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, preferredHeight);
    }

}

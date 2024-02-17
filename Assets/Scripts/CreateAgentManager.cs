using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Clients;

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
    private HTTPClient httpClient = HTTPClient.Instance;
    public SideMenu sideMenuManager;
    public IncubatingListManager incubatingListManager;

    public void StoreInput()
    {
        // Store text input from the username input field
        name = nameInputField.text;

        // Store text input from the password input field
        desc = descInputField.text;

        // Store incubation time from the slider
        incubation = incubationTime.value;
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
        // TODO: Add sprite visual method here
        HTTPClient.AgentData agent = new HTTPClient.AgentData();
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

    public async void SendAgentInfo()
    {
        HTTPClient.IdData agent = await httpClient.CreateAgent(name, desc, httpClient.MyId, (int)incubation);
        Guid agentId = agent.id;

        if (agentId != null)
        {
            // bool addSuccess = await httpClient.AddAgentToWorld(agentId);
            sideMenuManager.ToggleConfirmCreatePanel();
            incubatingListManager.CloseIncubatingListPanel();
            incubatingListManager.localDisplayIncubatingList(); // TODO: Change to non-local when backend working
        }
        else
        {
            Debug.Log("Failed to create agent due to backend error");
        }
    }

    // Adjusts size of scrollable description on the confirm create panel
    public void adjustDescHeight(){
        float preferredHeight = confirmDescObject.GetComponent<TextMeshProUGUI>().preferredHeight;
        RectTransform contentRectTransform = confirmCreateContent.GetComponent<RectTransform>();

        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, preferredHeight);
    }

}

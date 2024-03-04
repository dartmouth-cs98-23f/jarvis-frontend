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
    public string sprite_URL;
    public string sprite_headshot_URL;
    public GameObject eggPrefab;
    public SpriteLoader spriteLoader;
    public GameObject spinner;
    public Text nameError;
    public Text descError;
    public Text visualDescError;

    public GameObject incubationTimeInfoPanel;
    public GameObject incubationTimeInfoButton;

    void Start(){
        spinner.SetActive(false);
        nameInputField.onValueChanged.AddListener(delegate { CheckEmpty(nameInputField, nameError); });
        descInputField.onValueChanged.AddListener(delegate { CheckEmpty(descInputField, descError); });
        agentVisualDesc.onValueChanged.AddListener(delegate { CheckEmpty(agentVisualDesc, visualDescError); });
    }

    public void OnIncubationTimeInfoPanelButtonPressed(){
        if (incubationTimeInfoPanel.activeSelf){
            incubationTimeInfoPanel.SetActive(false);
        }
        else {
            incubationTimeInfoPanel.SetActive(true);
        }
    }

    bool CheckEmpty(InputField inputField, Text errorText)
    {
        string str = inputField.text;
        string error = InputValidation.CheckEmpty(str);
        errorText.text = error;
        if (string.IsNullOrEmpty(error)) 
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void StoreInput()
    {
        // Store text input from the username input field
        name = nameInputField.text;

        // Store text input from the password input field
        desc = descInputField.text;

        // Store incubation time from the slider
        incubation = incubationTime.value;

        bool usernameIsValid = CheckEmpty(nameInputField, nameError);
        bool descIsValid = CheckEmpty(descInputField, descError);

        if (usernameIsValid && descIsValid){
            sideMenuManager.ToggleVisualDescPanel();
            sideMenuManager.ToggleCreateAgentPanel();
        }
    }

    public async void StoreVisualDesc(){
        visual = agentVisualDesc.text;

        bool descIsValid = CheckEmpty(agentVisualDesc, visualDescError);

        if (descIsValid){
            spinner.SetActive(true);
            HTTPClient.PostVisualResponse resp = await httpClient.PostVisualDescription(visual);
            if (resp != null){
                spinner.SetActive(false);
                sprite_URL = resp.sprite_URL;
                sprite_headshot_URL = resp.sprite_headshot_URL;
                FillConfirmCreateFields();
                sideMenuManager.ToggleVisualDescPanel();
                sideMenuManager.ToggleConfirmCreatePanel();
            }
            else {
                Debug.LogError("Visual Description response is null");
            }
        }
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
        // Call the LoadSprite method with the desired URL
        spriteLoader.LoadSprite(sprite_headshot_URL, (sprite) => {
            confirmSpriteObject.GetComponent<Image>().sprite = sprite;
        });

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
        Vector3 textPosition = new Vector3(handlePosition.x, handlePosition.y - 60f, handlePosition.z); // Adjust the offset as needed
        sliderValueText.transform.position = textPosition;
    }

    public async void SendAgentInfo()
    {
        if (incubation == 0.0f){
            incubation = 2.5f / 60;
        }
        HTTPClient.IdData agent = await httpClient.CreateAgent(name, desc, httpClient.MyId, incubation, sprite_URL, sprite_headshot_URL);
        Guid agentId = agent.id;
        HTTPClient.UserData user = await httpClient.GetUser(httpClient.MyId);
        
        if (agentId != null)
        {
            bool addSuccess = await httpClient.AddAgentToWorld(agentId);
            if (!addSuccess){
                Debug.Log("Failed to add agent to world");
            }
            
            sideMenuManager.ToggleConfirmCreatePanel();
            sideMenuManager.ToggleHatchedPanel();
            incubatingListManager.CloseIncubatingListPanel();

            HTTPClient.AgentData agentInfo = await httpClient.GetAgent(agentId);
            if (agentInfo == null){
                Debug.Log("Failed to get agent data to create egg with");
            }
            GameObject gameClient = GameObject.Find("GameClient");
            gameClient.GetComponent<GameClient>().BuildEgg(agentInfo.hatchTime, agentInfo.createdTime, user.location.coordX, user.location.coordY, agentInfo);
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

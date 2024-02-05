using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

[System.Serializable]
public class WorldCreationResponse
{
    public string worldCode;
    public string thumbnail_URL;
}

public class CreateWorldManager : MonoBehaviour
{
    public InputField worldNameInputField;
    public InputField worldDescriptionInputField;
    public GameObject loadingPanel;
    public GameObject createWorldPanel;

    public GameObject generatedWorldPanel;
    public GameObject navbarPanel;
    // Start is called before the first frame update
    void OnEnable()
    {
        worldNameInputField.text = ""; // reset world name input field
        worldDescriptionInputField.text = ""; // reset world description input field
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void OnPressCreate()
    {
        string worldName = worldNameInputField.text;
        string worldDescription = worldDescriptionInputField.text;
        StartLoading();
        try {
            WorldCreationResponse response = await LocalCreateWorld(new Guid(), worldName, worldDescription); // sends a post request to the backend and wait for response
            // WorldCreationResponse response = await CreateWorld(HTTPClient.Instance.MyId, worldName, worldDescription); // sends a post request to the backend and wait for response
            // TODO: Check if CreateWorld on backend adds to User's list of worlds otherwise, add it to user's list of worlds using AddUserWorld()
            if (response != null) // when response comes back
            {
                FinishLoading(worldName, response.thumbnail_URL); // navigate to next screen
            }
        } catch (Exception e) {
            Debug.Log("Failed to create world: " + e.Message); // TODO: Add UI error message on failure to create world
        }
    }

    void StartLoading()
    {
        createWorldPanel.SetActive(false);
        loadingPanel.SetActive(true);
        navbarPanel.SetActive(false);
    }

    void FinishLoading(string worldName, string thumbnail_URL)
    {
        loadingPanel.SetActive(false);
        generatedWorldPanel.SetActive(true);
        generatedWorldPanel.GetComponent<GeneratedWorldManager>().SetGeneratedWorld(worldName, thumbnail_URL);
    }

    public async Task<WorldCreationResponse> LocalCreateWorld(Guid userId, string worldName, string worldDescription)
    {
        // Simulate delay to mimic network request time
        await Task.Delay(3000); // Wait for 3 second

        Debug.Log($"Simulated creation of world with name: {worldName} and description: {worldDescription}");

        // Return the mock world code as if it was received from the backend
        return new WorldCreationResponse { worldCode = "ABC123", thumbnail_URL = "https://picsum.photos/202" };
    }


    // TODO: Add this backend api accordingly. this is currently just a template code.
    async Task<WorldCreationResponse> CreateWorld()
    {
        string apiUrl = "your_api_endpoint_here";
        // Prepare your POST data here (example uses an empty JSON body)
        string jsonData = "{}";
        
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(apiUrl, jsonData))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Deserialize the JSON response
                WorldCreationResponse response = JsonConvert.DeserializeObject<WorldCreationResponse>(request.downloadHandler.text);
                Debug.Log("World created successfully. Code: " + response.worldCode);
                return response; // Return the world code from the response
            }
            else
            {
                Debug.LogError("Error creating world: " + request.error);
                return null;
            }
        }
    }
}

using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Clients;


public class CreateWorldManager : MonoBehaviour
{
    public InputField worldNameInputField;
    public InputField worldDescriptionInputField;
    public GameObject loadingPanel;
    public GameObject createWorldPanel;

    public GameObject generatedWorldPanel;
    public GameObject navbarPanel;
    private HTTPClient httpClient = HTTPClient.Instance;
    // Start is called before the first frame update
    void OnEnable()
    {
        worldNameInputField.text = ""; // reset world name input field
        worldDescriptionInputField.text = ""; // reset world description input field
    }

    public async void OnPressCreate()
    {
        string worldName = worldNameInputField.text;
        string worldDescription = worldDescriptionInputField.text;
        StartLoading();
        try {
            HTTPClient.CreateWorldResponse response = await LocalCreateWorld(new Guid(), worldName, worldDescription); // sends a post request to the backend and wait for response
            // HTTPClient.CreateWorldResponse response = await CreateWorld(httpClient.MyId, worldName, worldDescription); // sends a post request to the backend and wait for response
            // TODO: Check if CreateWorld on backend adds to User's list of worlds otherwise, add it to user's list of worlds using AddUserWorld()
            if (response != null) // when response comes back
            {
                FinishLoading(response.id, worldName, response.thumbnail_URL); // navigate to next screen
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

    void FinishLoading(Guid worldId, string worldName, string thumbnail_URL)
    {
        loadingPanel.SetActive(false);
        generatedWorldPanel.SetActive(true);
        generatedWorldPanel.GetComponent<GeneratedWorldManager>().SetGeneratedWorld(worldId, worldName, thumbnail_URL);
    }

    public async Task<HTTPClient.CreateWorldResponse> LocalCreateWorld(Guid userId, string worldName, string worldDescription)
    {
        // Simulate delay to mimic network request time
        await Task.Delay(3000); // Wait for 3 second

        Debug.Log($"Simulated creation of world with name: {worldName} and description: {worldDescription}");

        // Return the mock world code as if it was received from the backend
        return new HTTPClient.CreateWorldResponse { id = new Guid("55cd50d5-7775-4dd2-b632-a502a031ac41"), name = worldName, thumbnail_URL = "https://picsum.photos/202" };
    }


    // TODO: Add this backend api accordingly. this is currently just a template code.
    async Task<HTTPClient.CreateWorldResponse> CreateWorld(Guid userId, string worldName, string worldDescription)
    {
        return await httpClient.CreateWorld(userId, worldName, worldDescription);
    }
}

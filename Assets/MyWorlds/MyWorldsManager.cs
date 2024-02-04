using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class MyWorldsManager : MonoBehaviour
{
    public GameObject myWorldsPanel;
    public GameObject createWorldPanel;

    public List<HTTPClient.UserWorld> userWorlds;
    public GameObject currentWorldObject;
    private ImageSwiper worldSwiper;

    private string LOCAL_USER_WORLDS = @"
    [
        {
            ""id"": ""825fb828-1987-440f-af23-48bb201df08f"",
            ""name"": ""world1"",
            ""description"": ""string"",
            ""thumbnail_url"": ""https://picsum.photos/200""
        },
        {
            ""id"": ""3492d51d-0e04-4be9-afc7-a84f3e14fb9a"",
            ""name"": ""world2"",
            ""description"": ""string"", 
            ""thumbnail_url"": ""https://picsum.photos/id/237/200""
        },
        {
            ""id"": ""8e096bae-8898-49cf-a0e4-0abe0d79c03a"",
            ""name"": ""world3"",
            ""description"": ""string"",
            ""thumbnail_url"": ""https://picsum.photos/200?grayscale""
        },
    ]";

    // Start is called before the first frame update
    void Start()
    {
        currentWorldObject.SetActive(true);
        // TODO: Get from backend API
        userWorlds = LocalGetUserWorlds();
        Debug.Log("User Worlds: " + userWorlds.Count);
        worldSwiper = currentWorldObject.GetComponent<ImageSwiper>();
    }

    List<HTTPClient.UserWorld> LocalGetUserWorlds()
    {
        List<HTTPClient.UserWorld> userWorldList = JsonConvert.DeserializeObject<List<HTTPClient.UserWorld>>(LOCAL_USER_WORLDS);
        return userWorldList;
    }


    List<HTTPClient.UserWorld> GetUserWorlds()
    {
        // TODO: Convert this into sending a request to the backend API.
        List<HTTPClient.UserWorld> userWorldList = JsonConvert.DeserializeObject<List<HTTPClient.UserWorld>>(LOCAL_USER_WORLDS);
        return userWorldList;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressEnter()
    {
        // TODO: Load World scene.
    }

    public void OnPressCreateWorld()
    {
        myWorldsPanel.SetActive(false);
        createWorldPanel.SetActive(true);
    }

    public void LeaveWorld(string worldId)
    {
        // TODO: Local Testing version. Comment out if you want to test backend
        userWorlds.RemoveAt(worldSwiper.currentIndex);

        // TODO: Backend version:
        // await RemoveUserWorld(worldId);

        bool removeWorldSuccessful = true; // comment out this line to connect with backend API.
        // bool removeWorldSuccessful = await RemoveUserWorld(GetCurrentWorldId()); // TODO: uncomment

        if (removeWorldSuccessful)
        {
            // Get the updated list of worlds
            // userWorlds = await GetUserWorlds(); // TODO: Uncomment this for backend version 
            // re-render user's worlds
            worldSwiper.RemoveWorld();
        }
    }

}

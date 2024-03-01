using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


public class MyWorldsManager : MonoBehaviour
{
    public GameObject myWorldsPanel;
    public GameObject createWorldPanel;
    public GameObject addWorldPanel;

    public List<HTTPClient.UserWorld> userWorlds;
    public GameObject currentWorldObject;
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;
    public GameObject enterWorldButton;
    public GameObject navBarObject;
    public GameObject userProfilePanel;
    private NavbarManager navbarManager;
    private ImageSwiper worldSwiper;
    private HTTPClient httpClient = HTTPClient.Instance;

    private string LOCAL_USER_WORLDS = @"
    [
        {
            ""id"": ""825fb828-1987-440f-af23-48bb201df08f"",
            ""creatorId"": ""825fb828-1987-440f-af23-48bb201df08f"",
            ""name"": ""world1"",
            ""description"": ""string"",
            ""thumbnail_URL"": ""https://picsum.photos/200""
        },
        {
            ""id"": ""3492d51d-0e04-4be9-afc7-a84f3e14fb9a"",
            ""creatorId"": ""825fb828-1987-440f-af23-48bb201df08f"",
            ""name"": ""world2"",
            ""description"": ""string"", 
            ""thumbnail_URL"": ""https://picsum.photos/id/237/200""
        },
        {
            ""id"": ""8e096bae-8898-49cf-a0e4-0abe0d79c03a"",
            ""creatorId"": ""825fb828-1987-440f-af23-48bb201df08f"",
            ""name"": ""world3"",
            ""description"": ""string"",
            ""thumbnail_URL"": ""https://picsum.photos/200?grayscale""
        },
    ]";

    // Start is called before the first frame update
    async void Start()
    {

        // TODO: Get from backend API
        // userWorlds = await LocalGetUserWorlds();
        userWorlds = await GetUserWorlds();
        currentWorldObject.SetActive(true);

        navbarManager = navBarObject.GetComponent<NavbarManager>();
        navbarManager.SetCurrentPanel(myWorldsPanel);

        if (userWorlds == null || userWorlds.Count == 0)
        {
            currentWorldObject.SetActive(false);
            leftArrowButton.SetActive(false);
            rightArrowButton.SetActive(false);
            enterWorldButton.SetActive(false);
            return;
        } else {
            currentWorldObject.SetActive(true);
            leftArrowButton.SetActive(true);
            rightArrowButton.SetActive(true);
            enterWorldButton.SetActive(true);
        }

        worldSwiper = currentWorldObject.GetComponent<ImageSwiper>();
        if (worldSwiper != null)
        {

            Debug.Log("Setting up worldswiper: " + userWorlds.Count);
            worldSwiper.SetupUserWorlds(userWorlds); // A new method to safely initialize ImageSwiper with loaded data
        }

    }

    async Task<List<HTTPClient.UserWorld>> LocalGetUserWorlds()
    {
        await Task.Delay(3000);
        List<HTTPClient.UserWorld> userWorldList = JsonConvert.DeserializeObject<List<HTTPClient.UserWorld>>(LOCAL_USER_WORLDS);
        return userWorldList;
    }

    async Task<List<HTTPClient.UserWorld>> GetUserWorlds()
    {
        return await httpClient.GetUserWorlds(httpClient.MyId);
    }

    public void OnPressEnter()
    {
        // Set current world id and navigate to game
        if (worldSwiper != null && worldSwiper.GetCurrentWorldId() != null) {
            httpClient.WorldId = new Guid(worldSwiper.GetCurrentWorldId());
            SceneNavigator.LoadGame();
        }
    }

    public void OnPressCreateWorld()
    {
        if(createWorldPanel == null)
            Debug.LogError("createWorldPanel is null");
        else
            createWorldPanel.SetActive(true);        

        if(navbarManager == null)
            Debug.LogError("navbarManager is null");
        else
            navbarManager.SetCurrentPanel(createWorldPanel);
        myWorldsPanel.SetActive(false);
    }

    public void OnPressAddNewWorld()
    {
        myWorldsPanel.SetActive(false);
        addWorldPanel.SetActive(true);
        navbarManager.SetCurrentPanel(addWorldPanel);
    }

    async Task<bool> LocalAddUserToWorld(Guid worldId, Guid userId)
    {
        await Task.Delay(1000);

        userWorlds.Add(
            new HTTPClient.UserWorld
            {
                id = worldId,
                creatorId = userId,
                name = "Existing world",
                description = "newly added world example",
                thumbnail_URL = "https://picsum.photos/201"
            }
        );
        return true;
    }

    public async Task<string> AddWorld(string worldCode)
    {

        Debug.Log("Adding world" + " " + worldCode);
        // TODO: This is for local testing only. Comment out for deploy.
        // HTTPClient.AddUserToWorldResponse addWorldResponse = await LocalAddUserToWorld(new Guid(), new Guid());

        // TODO: Add backend api. get the worldId from response and add to userWorlds
        Guid? nullableWorldId = await httpClient.GetWorldIdFromWorldCode(worldCode);
        if (nullableWorldId == null || nullableWorldId == Guid.Empty)
        {
            Debug.Log("World not found");
            return "Invalid world code. Please try again.";
        }
        Guid worldId = nullableWorldId.Value;

        // Validate if world is already in user's worlds
        for (int i = 0; i < userWorlds.Count; i++)
        {
            if (userWorlds[i].id == worldId)
            {
                Debug.Log("World already exists in user's worlds");
                return "You've already added this world.";
            }
        }

        bool addWorldSuccessful = await httpClient.AddUserToWorld(worldId, httpClient.MyId);

        if (addWorldSuccessful) // if successfully added to user's worlds on backend
        {

            if (userWorlds != null && userWorlds.Count == 0) // if user had no worlds before and now added successfully, show the world and the arrow buttons
            {
                currentWorldObject.SetActive(true);
                leftArrowButton.SetActive(true);
                rightArrowButton.SetActive(true);
                enterWorldButton.SetActive(true);
            }
            // TODO: Uncomment this if using for backend
            userWorlds = await GetUserWorlds(); // re-render all of user's worlds
            worldSwiper.AddWorld();
            return "success";
        } else {
            return "Failed to add world. Please try again.";
        }
    }

    public async Task LeaveWorld(string worldId)
    {
        // TODO: Local Testing version. Comment out if you want to test backend
        // userWorlds.RemoveAt(worldSwiper.currentIndex);
        // bool removeWorldSuccessful = true; // comment out this line to connect with backend API.

        // TODO: Backend version:
        bool removeWorldSuccessful = await httpClient.RemoveWorldFromList(new Guid(worldId), httpClient.MyId);

        if (removeWorldSuccessful)
        {
            // Get the updated list of worlds
            userWorlds = await GetUserWorlds(); // TODO: Uncomment this for backend version 
            // re-render user's worlds
            worldSwiper.RemoveWorld();

            if (userWorlds == null || userWorlds.Count == 0)
            {
                currentWorldObject.SetActive(false);
                leftArrowButton.SetActive(false);
                rightArrowButton.SetActive(false);
                enterWorldButton.SetActive(false);
            }
        }
    }
}

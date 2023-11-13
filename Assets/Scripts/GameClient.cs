using UnityEngine;
using Clients;
using System;


public class GameClient : MonoBehaviour
{

    private Guid userId;
    public GameObject currentPlayerPrefab;
    // public GameObject NPCPlayerPrefab;
    public SignalRClient signalRClient;
    public Transform mainMap;

    private HTTPClient httpClient;
    private HTTPClient.UserData currentUserData;

    void Start()
    {
        httpClient = HTTPClient.Instance; // get httpClient
        userId = httpClient.MyId; // TODO: Uncomment and delete line below
        InitializeGame();

    }

    void Update()
    {

    }

    async void InitializeGame()
    {
        currentUserData = await HTTPClient.Instance.GetUser(userId);
        if (currentUserData != null)
        {
            Debug.Log("InitializeUserData: " + currentUserData.firstName);
            BuildAllCharacters(); // Call this method after user data is initialized
            await SignalRClient.Initialize(userId, currentUserData.firstName); // TODO: Change first name to user first name later
            signalRClient = SignalRClient.Instance;
            signalRClient.RegisterUpdateLocationHandler(); // register updateLocation handler
        }
        else
        {
            Debug.LogError("Failed to initialize user data.");
        }
    }

    void BuildAllCharacters()
    {
        // userId = new Guid("be05eea4-f13b-41fe-ade7-e09ff6044bda");
        // Debug.Log("BuildAllCharacters userId: " + userId); 
        // Debug.Log("BuildAllCharacters userId HTTP Client: " + HTTPClient.Instance.MyId);

        // TODO: Uncomment below for local testing
        ////////////////////////////////////////////////////

        // httpClient.currentUserData.firstName = "Evan";
        // httpClient.currentUserData.lastName = "Phillips";
        // string xCoordinateString = PlayerPrefs.GetString("lastKnownX", "0");
        // string yCoordinateString = PlayerPrefs.GetString("lastKnownY", "0");
        // httpClient.currentUserData.lastKnownX = int.Parse(xCoordinateString);
        // httpClient.currentUserData.lastKnownY = int.Parse(yCoordinateString);

        ////////////////////////////////////////////////////

        // ADD back httpClient.currentUserData if testing locally
        // TODO: Handle error codes
        // if (currentUserData == null)
        // { 
        //     Debug.Log("error getting user");
        //     return;
        // }

        GameObject currentUserGO = Instantiate(currentPlayerPrefab, mainMap);
        currentUserGO.tag = "Player";
        PlayerMovement playerMovementScript = currentUserGO.GetComponent<PlayerMovement>();
        playerMovementScript.InteractButton = GameObject.Find("InteractButton");
        playerMovementScript.InteractButton.SetActive(false);
        CharacterComponent characterComponent = currentUserGO.GetComponent<CharacterComponent>();
        characterComponent.SetPosition(currentUserData.lastKnownX, currentUserData.lastKnownY, 0); // Add back httpClient.currentUserData if testing locally


        GameObject georgeWashingtonGO = Instantiate(currentPlayerPrefab, mainMap);
        georgeWashingtonGO.tag = "Player";
        PlayerMovement georgeWashingtonMovementScript = georgeWashingtonGO.GetComponent<PlayerMovement>();
        Transform georgeMainCamera = georgeWashingtonGO.transform.Find("Main Camera");
        // If the child exists, destroy it
        if (georgeMainCamera != null)
        {
            Destroy(georgeMainCamera.gameObject);
        }
        if (georgeWashingtonMovementScript != null)
        {
            Destroy(georgeWashingtonMovementScript);
        }
        
        CharacterComponent georgeWashingtonComponent = georgeWashingtonGO.GetComponent<CharacterComponent>();
        georgeWashingtonComponent.SetPosition(-10, 10, 0); // Add back httpClient.currentUserData if testing locally
        georgeWashingtonComponent.SetUserId(new Guid("a7374288-d61d-439e-9c56-855a61cba425")); // Add back httpClient.currentUserData if testing locally

    }
    
}

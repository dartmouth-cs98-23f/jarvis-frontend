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

    void Awake()
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
            Debug.Log("InitializeGame: " + currentUserData.firstName);
            Debug.Log("InitializeGame: " + currentUserData.lastKnownX + ", " + currentUserData.lastKnownY);
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

    // c8268729-e4f5-4df3-85fc-51779d7c2b35
    //d2e12e14-5df1-4a0e-92f1-11f2f10e5880

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
        // If the camera exists, destroy it
        // destroy mainCamera and playerMovement script for NPC
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
        georgeWashingtonComponent.SetUserId(new Guid("55cd50d5-7775-4dd2-b632-a502a031ac41")); // Add back httpClient.currentUserData if testing locally



        GameObject yodaGO = Instantiate(currentPlayerPrefab, mainMap);
        yodaGO.tag = "Player";
        PlayerMovement yodaMovementScript = yodaGO.GetComponent<PlayerMovement>();
        Transform yodaMainCamera = yodaGO.transform.Find("Main Camera");
        // destroy mainCamera and playerMovement script for NPC
        if (yodaMainCamera != null)
        {
            Destroy(yodaMainCamera.gameObject);
        }
        if (yodaMovementScript != null)
        {
            Destroy(yodaMovementScript);
        }
        
        CharacterComponent yodaComponent = yodaGO.GetComponent<CharacterComponent>();
        yodaComponent.SetPosition(10, -3, 0); // Add back httpClient.currentUserData if testing locally
        yodaComponent.SetUserId(new Guid("f7dd290b-faab-4c15-b8b9-38cff0895559")); // Add back httpClient.currentUserData if testing locally

    }
    
}

using UnityEngine;
using Clients;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;


public class GameClient : MonoBehaviour
{

    private Guid userId;
    public GameObject currentUserPrefab;
    public GameObject otherUserPrefab;
    public GameObject AgentPrefab;
    public GameObject georgePrefab;
    public GameObject yodaPrefab;
    public SignalRClient signalRClient;
    public Transform mainMap;

    private HTTPClient httpClient;
    private HTTPClient.UserData currentUserData;
    private List<HTTPClient.UserData> allUsers = new List<HTTPClient.UserData>();


    void Awake()
    {
        httpClient = HTTPClient.Instance; // get httpClient
        userId = httpClient.MyId;
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
            Debug.Log("InitializeGame: " + currentUserData.username);
            Debug.Log("InitializeGame: " + currentUserData.location.coordX + ", " + currentUserData.location.coordY);
            // BuildAllCharacters(); // Call this method after user data is initialized
            BuildAllCharactersV2(); // Call this method after user data is initialized

            await SignalRClient.Initialize(httpClient.AuthToken, currentUserData.username); // TODO: Change first name to user first name later
            signalRClient = SignalRClient.Instance;
            // signalRClient.RegisterUpdateLocationHandler(); // register updateLocation handler
        }
        else
        {
            Debug.LogError("Failed to initialize user data.");
        }
    }


    void BuildAllCharactersV2()
    {
        BuildAllUsers();
        // BuildAllAgents();
    }

    // void BuildAllAgents()
    // {
    //     return;
    // }

    async void BuildAllUsers()
    {
        allUsers = await httpClient.GetWorldUsers(httpClient.CurrentWorldId);
        foreach (HTTPClient.UserData user in allUsers)
        {
            GameObject userPrefab = GetUserPrefab(user.id);
            GameObject userGO = Instantiate(userPrefab, mainMap); // TODO: Replace georgePrefab with actual user prefab
            userGO.tag = "Player";

            if (user.id == httpClient.MyId)
            {
                PlayerMovement userMovementScript = userGO.GetComponent<PlayerMovement>();
                userMovementScript.InteractButton = GameObject.Find("InteractButton");
                userMovementScript.InteractButton.SetActive(false);
                userMovementScript.SetTilemap(GameObject.Find("Tilemap").GetComponent<Tilemap>());
            } else {
                OtherPlayerMovement otherUserMovementScript = userGO.GetComponent<OtherPlayerMovement>();

            }

            CharacterComponent userComponent = userGO.GetComponent<CharacterComponent>();
            userComponent.SetPosition(user.location.coordX, user.location.coordY, 0);
            userComponent.SetUserId(user.id);
        }
    }

    // TODO: Update this method to generate the actual prefab of the user
    GameObject GetUserPrefab(Guid userId)
    {
        if (userId == this.userId)
        {
            return currentUserPrefab;
        }
        else
        {
            return otherUserPrefab;
        }
    }



    // c8268729-e4f5-4df3-85fc-51779d7c2b35
    //d2e12e14-5df1-4a0e-92f1-11f2f10e5880

    void BuildAllCharacters()
    {
        // TODO: Uncomment below for local testing
        //////////////////////////////////////////////////

        string xCoordinateString = PlayerPrefs.GetString("coordX", "0");
        string yCoordinateString = PlayerPrefs.GetString("coordY", "0");
        currentUserData.location.coordX = int.Parse(xCoordinateString);
        currentUserData.location.coordY = int.Parse(yCoordinateString);

        //////////////////////////////////////////////////

        // ADD back httpClient.currentUserData if testing locally
        // TODO: Handle error codes
        if (currentUserData == null)
        { 
            Debug.Log("error getting user");
            return;
        }

        GameObject currentUserGO = Instantiate(currentUserPrefab, mainMap);
        currentUserGO.tag = "Player";

        Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        PlayerMovement playerMovementScript = currentUserGO.GetComponent<PlayerMovement>();
        playerMovementScript.InteractButton = GameObject.Find("InteractButton");
        playerMovementScript.InteractButton.SetActive(false);
        playerMovementScript.SetTilemap(tilemap);
        CharacterComponent characterComponent = currentUserGO.GetComponent<CharacterComponent>();
        characterComponent.SetPosition(currentUserData.location.coordX, currentUserData.location.coordY, 0); // Add back httpClient.currentUserData if testing locally


        GameObject georgeWashingtonGO = Instantiate(georgePrefab, mainMap);
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



        GameObject yodaGO = Instantiate(yodaPrefab, mainMap);
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

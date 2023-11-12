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

    void Start()
    {
        temp();
        InitSignalRClient();
        BuildAllCharacters();
    }

    void Update()
    {
        SignalRListener();
    }

    async void BuildAllCharacters()
    {
        HTTPClient httpClient = HTTPClient.Instance;
        // userId = httpClient.MyId; // TODO: Uncomment and delete line below
        userId = new Guid("be05eea4-f13b-41fe-ade7-e09ff6044bda");
        Debug.Log("userId" + userId); 
        // TODO: remove all "httpClient." from currentUserData
        // HTTPClient.UserData currentUserData = await httpClient.GetUser(userId); // TODO: Uncomment and delete lines below
        ////////////////////////////////////////////////////

        httpClient.currentUserData.firstName = "Evan";
        httpClient.currentUserData.lastName = "Phillips";
        string xCoordinateString = PlayerPrefs.GetString("lastKnownX", "0");
        string yCoordinateString = PlayerPrefs.GetString("lastKnownY", "0");
        httpClient.currentUserData.lastKnownX = int.Parse(xCoordinateString);
        httpClient.currentUserData.lastKnownY = int.Parse(yCoordinateString);

        ////////////////////////////////////////////////////

        Debug.Log("currentUserData" + httpClient.currentUserData);
        // TODO: Handle error codes
        if (httpClient.currentUserData == null)
        { 
            Debug.Log("error getting user");
            return;
        }

        GameObject currentUserGO = Instantiate(currentPlayerPrefab, mainMap);
        PlayerMovement playerMovementScript = currentUserGO.GetComponent<PlayerMovement>();
        playerMovementScript.InteractButton = GameObject.Find("InteractButton");
        playerMovementScript.InteractButton.SetActive(false);
        CharacterComponent characterComponent = currentUserGO.GetComponent<CharacterComponent>();
        characterComponent.SetPosition(httpClient.currentUserData.lastKnownX, httpClient.currentUserData.lastKnownY, 0);
    }


    void InitSignalRClient()
    {
        // Initialize SignalR connection
        signalRClient = SignalRClient.Instance;
    }
    

    void SignalRListener(){
        signalRClient.RegisterUpdateLocationHandler();
    }

    // TODO: Delete this method and use normal CreateAccount shi
    // Doing this rn so we can skip create account screen
    async void temp(){
        await SignalRClient.Initialize("Evan", "https://simyou.azurewebsites.net/unity");
        Debug.Log("SignalRClient just initialized");
    }
}

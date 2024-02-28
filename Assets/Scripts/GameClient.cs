using UnityEngine;
using Clients;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Newtonsoft.Json;


public class CharacterType
{
    public static string User = "user";
    public static string Agent = "agent";
}

public class GameClient : MonoBehaviour
{

    private Guid userId;
    private Guid worldId;
    public Guid eggId;
    public GameObject currentUserPrefab;
    public GameObject otherUserPrefab;
    public GameObject AgentPrefab;
    public GameObject EggPrefab;
    public GameObject georgePrefab;
    public GameObject yodaPrefab;
    public Transform mainMap;

    private HTTPClient httpClient;
    private HTTPClient.UserData currentUserData;

    private List<HTTPClient.UserData> allUsers;
    private List<HTTPClient.AgentData> allAgents;
    private HashSet<Guid> characterIdSet = new HashSet<Guid>();
    public AgentInfoManager agentInfoManager;
    void OnEnable()
    {
        httpClient = HTTPClient.Instance; // get httpClient
        // TODO: Uncomment below for backend connection
        userId = httpClient.MyId;
        worldId = httpClient.CurrentWorldId;
        allUsers = new List<HTTPClient.UserData>();
        allAgents = new List<HTTPClient.AgentData>();
        Debug.Log("userId: " + userId);
        Debug.Log("worldId: " + worldId);

        // TODO: Comment below out for backend connection
        // userId = new Guid("c0c973f7-5f80-437e-8418-f3c401780274");
        // worldId = new Guid("3b490737-6d3f-4bb8-9593-15e8a1c80dab");

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
            // BuildAllCharacters(); // Call this method after user data is initialized
            BuildAllCharactersV2(); // Call this method after user data is initialized
        }
        else
        {
            Debug.LogError("Failed to initialize user data.");
        }
    }

    void BuildAllCharactersV2()
    {
        BuildAllUsers();
        BuildAllAgents();
    }

    async void BuildAllAgents()
    {
        allAgents = await httpClient.GetWorldAgents(worldId);
        foreach (HTTPClient.AgentData agent in allAgents)
        {
            if (characterIdSet != null && characterIdSet.Contains(agent.id))
            {
                Debug.Log("Agent with id: " + agent.id + " already exists. Skipping...");
                continue;
            } else {
                characterIdSet.Add(agent.id);
            }
            GameObject agentPrefab = GenerateAgentPrefab(agent);
            GameObject agentGO = Instantiate(agentPrefab, mainMap); // TODO: Replace georgePrefab with actual user prefab
            agentGO.tag = CharacterType.Agent;

            CharacterComponent agentComponent = agentGO.GetComponent<CharacterComponent>();
            agentComponent.SetPosition(agent.location.coordX, agent.location.coordY, 0);
            agentComponent.SetCharacterId(agent.id);
            agentComponent.SetCharacterType(CharacterType.Agent);

            // DisableCharacterRigidBody(agentComponent);
        }
    }

    // Disable the Rigidbody2D to stop character from moving due to collisions
    void DisableCharacterRigidBody(CharacterComponent characterComponent)
    {
        Rigidbody2D characterRigidbody = characterComponent.GetComponent<Rigidbody2D>();
        if (characterRigidbody != null)
        {
            characterRigidbody.bodyType = RigidbodyType2D.Static; 
        }
    }

    GameObject GenerateAgentPrefab(HTTPClient.AgentData agent)
    {
        if (agent.isHatched) {
            return AgentPrefab; // TODO: Add logic to get actual agent prefab
        } else {
            return EggPrefab; // TODO: Add logic for this to display the incubation time and to interact with it - nurture it
        }
    }
    

    async void BuildAllUsers()
    {
        allUsers = await httpClient.GetWorldUsers(worldId);
        foreach (HTTPClient.UserData user in allUsers)
        {
            // Debug.Log("Building user: " + user.username + " with id: " + user.id + " at location: " + user.location.coordX + ", " + user.location.coordY);
            if (characterIdSet != null && characterIdSet.Contains(user.id))
            {
                Debug.Log("User with id: " + user.id + " already exists. Skipping...");
                continue;
            } else {
                characterIdSet.Add(user.id);
            }

            GameObject userPrefab = GenerateUserPrefab(user.id);
            GameObject userGO = Instantiate(userPrefab, mainMap); // TODO: Replace georgePrefab with actual user prefab
            userGO.tag = CharacterType.User;

            CharacterComponent userComponent = userGO.GetComponent<CharacterComponent>();
            userComponent.SetCharacterId(user.id);
            userComponent.SetCharacterType(CharacterType.User);

            if (user.id == userId)
            {
                PlayerMovement userMovementScript = userGO.GetComponent<PlayerMovement>();
                userMovementScript.InteractButton = GameObject.Find("ChatButton");
                userMovementScript.NurtureButton = GameObject.Find("NurtureButton");
                userMovementScript.InteractButton.SetActive(false);
                userMovementScript.NurtureButton.SetActive(false);
                userMovementScript.SetTilemap(GameObject.Find("Tilemap").GetComponent<Tilemap>());
            } else {
                OtherPlayerMovement otherUserMovementScript = userGO.GetComponent<OtherPlayerMovement>();
                // Disable the Rigidbody2D to stop character from moving due to collisions
                DisableCharacterRigidBody(userComponent);
            }

            BodyPartsManager bpComponent = userGO.GetComponent<BodyPartsManager>();
            bpComponent.SetSprite(user.spriteAnimations); // TODO: Replace with user.spriteAnimations
            
            if (user.location == null)
            {
                Debug.Log("User location is null. Setting coordinates to 0, 0");
                userComponent.SetPosition(0, 0, 0);
                continue;
            } else {
                Debug.Log("Setting user " + user.username + " location to: " + user.location.coordX + ", " + user.location.coordY);
                userComponent.SetPosition(user.location.coordX, user.location.coordY, 0);
            }
            
            // Disable the Rigidbody2D to stop character from moving due to collisions
            // DisableCharacterRigidBody(userComponent);
        }
    }

    // TODO: Update this method to generate the actual prefab of the user
    GameObject GenerateUserPrefab(Guid userId)
    {
        if (userId == this.userId)
        {
            Debug.Log("Generating main user prefab with userId: " + userId);
            return currentUserPrefab;   // this prefab would have the playerMovement script attached
        }
        else
        {
            Debug.Log("Generating other user prefab with userId: " + userId);
            return otherUserPrefab; // this prefab would have the otherPlayerMovement script attached
        }
    }

    public void OnNurturePressed(){
        agentInfoManager.SetAgentID(eggId);
    }

    // c8268729-e4f5-4df3-85fc-51779d7c2b35
    //d2e12e14-5df1-4a0e-92f1-11f2f10e5880
    // @Deprecated: This is from V1 (MVP)
    // void BuildAllCharacters()
    // {
    //     // TODO: Uncomment below for local testing
    //     //////////////////////////////////////////////////

    //     string xCoordinateString = PlayerPrefs.GetString("coordX", "0");
    //     string yCoordinateString = PlayerPrefs.GetString("coordY", "0");
    //     currentUserData.location.coordX = int.Parse(xCoordinateString);
    //     currentUserData.location.coordY = int.Parse(yCoordinateString);

    //     //////////////////////////////////////////////////

    //     // ADD back httpClient.currentUserData if testing locally
    //     // TODO: Handle error codes
    //     if (currentUserData == null)
    //     { 
    //         Debug.Log("error getting user");
    //         return;
    //     }

    //     GameObject testGO = Instantiate(currentUserPrefab, mainMap);
    //     testGO.tag = "Player";
    //     CharacterComponent testCharacterComponent = testGO.GetComponent<CharacterComponent>();
    //     testCharacterComponent.SetPosition(currentUserData.location.coordX, currentUserData.location.coordY, 0);

    //     GameObject currentUserGO = Instantiate(currentPlayerPrefab, mainMap);
    //     currentUserGO.tag = "Player";

    //     Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();

    //     PlayerMovement playerMovementScript = currentUserGO.GetComponent<PlayerMovement>();
    //     playerMovementScript.InteractButton = GameObject.Find("ChatButton");
    //     playerMovementScript.InteractButton.SetActive(false);
    //     playerMovementScript.SetTilemap(tilemap);
    //     CharacterComponent characterComponent = currentUserGO.GetComponent<CharacterComponent>();
    //     characterComponent.SetPosition(currentUserData.location.coordX, currentUserData.location.coordY, 0); // Add back httpClient.currentUserData if testing locally


    //     GameObject georgeWashingtonGO = Instantiate(georgePrefab, mainMap);
    //     georgeWashingtonGO.tag = "Player";
    //     PlayerMovement georgeWashingtonMovementScript = georgeWashingtonGO.GetComponent<PlayerMovement>();
    //     Transform georgeMainCamera = georgeWashingtonGO.transform.Find("Main Camera");
    //     // If the camera exists, destroy it
    //     // destroy mainCamera and playerMovement script for NPC
    //     if (georgeMainCamera != null)
    //     {
    //         Destroy(georgeMainCamera.gameObject);
    //     }
    //     if (georgeWashingtonMovementScript != null)
    //     {
    //         Destroy(georgeWashingtonMovementScript);
    //     }
        
    //     CharacterComponent georgeWashingtonComponent = georgeWashingtonGO.GetComponent<CharacterComponent>();
    //     georgeWashingtonComponent.SetPosition(-10, 10, 0); // Add back httpClient.currentUserData if testing locally
    //     georgeWashingtonComponent.SetCharacterId(new Guid("55cd50d5-7775-4dd2-b632-a502a031ac41")); // Add back httpClient.currentUserData if testing locally



    //     GameObject yodaGO = Instantiate(yodaPrefab, mainMap);
    //     yodaGO.tag = "Player";
    //     PlayerMovement yodaMovementScript = yodaGO.GetComponent<PlayerMovement>();
    //     Transform yodaMainCamera = yodaGO.transform.Find("Main Camera");
    //     // destroy mainCamera and playerMovement script for NPC
    //     if (yodaMainCamera != null)
    //     {
    //         Destroy(yodaMainCamera.gameObject);
    //     }
    //     if (yodaMovementScript != null)
    //     {
    //         Destroy(yodaMovementScript);
    //     }
        
    //     CharacterComponent yodaComponent = yodaGO.GetComponent<CharacterComponent>();
    //     yodaComponent.SetPosition(10, -3, 0); // Add back httpClient.currentUserData if testing locally
    //     yodaComponent.SetCharacterId(new Guid("f7dd290b-faab-4c15-b8b9-38cff0895559")); // Add back httpClient.currentUserData if testing locally

    // }
    
}

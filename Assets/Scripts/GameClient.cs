using UnityEngine;
using Clients;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.Concurrent;


public class CharacterType
{
    public static string User = "user";
    public static string Agent = "agent";
    public static string Egg = "egg";
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
    public Transform eggTransform;

    private HTTPClient httpClient;
    private HTTPClient.UserData currentUserData;

    private List<HTTPClient.UserData> allUsers;
    private List<HTTPClient.AgentData> allAgents;
    private HashSet<Guid> characterIdSet = new HashSet<Guid>();
    public TrainAgentManager trainAgentManager;
    public SpriteLoader spriteLoader;
    private ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

    void OnEnable()
    {
        httpClient = HTTPClient.Instance; // get httpClient
        userId = httpClient.MyId;
        worldId = httpClient.WorldId;
        allUsers = new List<HTTPClient.UserData>();
        allAgents = new List<HTTPClient.AgentData>();
        Debug.Log("userId: " + userId);
        Debug.Log("worldId: " + worldId);


        InitializeGame();
    }

    public void Enqueue(Action action)
    {
        _actions.Enqueue(action);
    }

    void Update()
    {
        while (_actions.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }

    async void InitializeGame()
    {
        currentUserData = await HTTPClient.Instance.GetUser(userId);
        if (currentUserData != null)
        {
            Debug.Log("InitializeGame: " + currentUserData.username);
            BuildAllCharactersV2(); // Call this method after user data is initialized
        }
        else
        {
            Debug.LogError("Failed to initialize user data.");
        }

        // Initialize the signalR handlers
        SignalRClient.Instance.OnUserAddedToWorldHandler(this);
        SignalRClient.Instance.OnUserRemovedFromWorldHandler(this);
        SignalRClient.Instance.OnAgentAddedToWorldHandler(this);
        SignalRClient.Instance.UserOnlineCheckHandler();
    }

    async void BuildAllCharactersV2()
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
            
            if (agent.isHatched){
                BuildAgent(agent);
            }
            else {
                BuildEgg(agent.hatchTime, agent.createdTime, agent.location.coordX, agent.location.coordY, agent);
            }
        }
    }

    GameObject GenerateAgentPrefab(HTTPClient.AgentData agent)
    {
        if (agent.isHatched) {
            Debug.Log("Generating agent prefab with id: " + agent.id);
            return AgentPrefab; 
        } else {
            Debug.Log("Generating egg prefab with id: " + agent.id);
            return EggPrefab; 
        }
    }
    

    async void BuildAllUsers()
    {
        allUsers = await httpClient.GetWorldUsers(worldId);
        foreach (HTTPClient.UserData user in allUsers)
        {
            BuildUser(user);
        }
    }

    void BuildUser(HTTPClient.UserData user)
    {
        // Debug.Log("Building user: " + user.username + " with id: " + user.id + " at location: " + user.location.coordX + ", " + user.location.coordY);
        if (characterIdSet != null && characterIdSet.Contains(user.id))
        {
            Debug.Log("User with id: " + user.id + " already exists. Skipping...");
            return;
        } else {
            characterIdSet.Add(user.id);
        }

        GameObject userPrefab = GenerateUserPrefab(user.id);
        GameObject userGO = Instantiate(userPrefab, mainMap);
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
            otherUserMovementScript.userId = user.id;
        }

        BodyPartsManager bpComponent = userGO.GetComponent<BodyPartsManager>();
        bpComponent.SetSprite(user.spriteAnimations); 
        
        if (user.location == null)
        {
            Debug.Log("User location is null. Setting coordinates to 0, 0");
            userComponent.SetPosition(0, 0, 0);
        } else {
            Debug.Log("Setting user " + user.username + " location to: " + user.location.coordX + ", " + user.location.coordY);
            userComponent.SetPosition(user.location.coordX, user.location.coordY, 0);
        }
    }

    // TODO: This method is called by SignalR to add a user to the world. It hasn't been tested yet
    public async void AddUserToWorld(Guid userId)
    {
        HTTPClient.UserData newUser = await httpClient.GetUser(userId);
        if (newUser != null) {
            Enqueue(() =>
            {
                BuildUser(newUser);
            });
        }
    }

    // TODO: This method is called by SignalR to remove a user from the world. It hasn't been tested yet
    public async void RemoveUserFromWorld(Guid userId)
    {
        Enqueue(() =>
        {
            // Find all game objects with the User tag
            GameObject[] userGOs = GameObject.FindGameObjectsWithTag(CharacterType.User);

            // Iterate over each GameObject to find the one with the matching userId
            foreach (GameObject userGO in userGOs)
            {
                CharacterComponent userComponent = userGO.GetComponent<CharacterComponent>();
                if (userComponent != null && userComponent.GetCharacterId() == userId)
                {
                    // If the userId matches, destroy the GameObject
                    Destroy(userGO);
                    break; // Exit the loop if the user is found and removed
                }
            }
        });

    }

    // TODO: This method is called by SignalR to add a user to the world. It hasn't been tested yet
    public async void AddAgentToWorld(Guid agentId)
    {
        HTTPClient.AgentData newAgent = await httpClient.GetAgent(agentId);
        if (newAgent != null) {
            if (characterIdSet != null && characterIdSet.Contains(newAgent.id))
            {
                Debug.Log("Agent with id: " + agentId + " already exists. Skipping...");
                return;
            } else {
                characterIdSet.Add(agentId);
            }
            Enqueue(() =>
            {
                BuildEgg(newAgent.hatchTime, newAgent.createdTime, newAgent.location.coordX, newAgent.location.coordY, newAgent);
            });
        }
    }

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

    public void BuildEgg(DateTime hatchTime, DateTime createdTime, int x, int y, HTTPClient.AgentData agent){
        GameObject eggGO = Instantiate(EggPrefab, eggTransform);
        eggGO.GetComponent<EggPrefabManager>().SetEggDetails(hatchTime, createdTime, agent.username);
        eggGO.GetComponent<CharacterComponent>().SetPosition(x, y, 0);
        eggGO.GetComponent<CharacterComponent>().SetCharacterId(agent.id);
        eggGO.GetComponent<CharacterComponent>().SetCharacterType(CharacterType.Egg);
        eggGO.tag = CharacterType.Egg;
    }
    
    public void BuildAgent(HTTPClient.AgentData agent){
        GameObject agentGO = Instantiate(AgentPrefab, mainMap);
        agentGO.tag = CharacterType.Agent;
        CharacterComponent agentComponent = agentGO.GetComponent<CharacterComponent>();
        agentComponent.SetCharacterType(CharacterType.Agent);
        agentComponent.SetPosition(agent.location.coordX, agent.location.coordY, 0);
        agentComponent.SetCharacterId(agent.id);

        spriteLoader.LoadSprite(agent.sprite_URL, (sprite) => {
            agentGO.GetComponent<SpriteRenderer>().sprite = sprite;
        });
    }

    public void OnNurturePressed(){
        Debug.Log("Nurture pressed");
        trainAgentManager.SetAgentID(eggId);
    }


}

using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Clients {

public class HTTPClient
{
    private static HTTPClient instance;

    //TODO: Delete currentUserData
    public UserData currentUserData = new UserData();

    private readonly HttpClient httpClient = new HttpClient();
    // private const string url = "http://localhost:5087";  
    private const string url = "https://api.simugameservice.lekina.me/";  

    private Guid myId;
    private Guid worldId;
    private Dictionary<Guid, Location> userLocations = new Dictionary<Guid, Location>(); // userId: location info about user

    private HTTPClient() { }

    public static HTTPClient Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HTTPClient();
            }
            return instance;
        }
    }

    // private void OnDestroy()
    // {
    //     // Dispose of the HttpClient when the script is destroyed to prevent resource leaks
    //     httpClient.Dispose();
    // }

    public async Task<bool> RegisterUser(string firstName, string lastName, string email, string password)
{
    string apiUrl = $"{url}/Authentication/register";

    try
    {
        UserRegistrationData userData = new UserRegistrationData
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        string jsonRequest = JsonConvert.SerializeObject(userData);
        HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            UserRegistrationResponse registrationResponse = JsonConvert.DeserializeObject<UserRegistrationResponse>(jsonResponse);
            Debug.Log("User registered successfully. ID: " + registrationResponse.userId + ", Response String: " + registrationResponse.responseString);
            myId = registrationResponse.userId;

            return true; // Registration successful
        }
        else
        {
            // Handle other HTTP status codes if needed
            Debug.LogError("RegisterUser Error: " + response.StatusCode);
            return false; // Registration failed
        }
    }
    catch (HttpRequestException e)
    {
        // Handle other exceptions if needed
        Debug.LogError("Register HTTP Request Exception: " + e.Message);
        return false; // Registration failed due to exception
    }
}

    public async Task<bool> Login(string email, string password)
{
    string apiUrl = $"{url}/authentication/login";

    Debug.Log("login called");
    try
    {
        UserLoginData loginData = new UserLoginData
        {
            Email = email,
            Password = password
        };

        string jsonRequest = JsonConvert.SerializeObject(loginData);
        HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

        Debug.Log("before response");

        HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
        Debug.Log("after response" + response);

        if (response.IsSuccessStatusCode)
        {
            Debug.Log("response is success status code");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            UserRegistrationResponse registrationResponse = JsonConvert.DeserializeObject<UserRegistrationResponse>(jsonResponse);
            Debug.Log("User logged in successfully. ID: " + registrationResponse.userId + ", Response String: " + registrationResponse.responseString);
            myId = registrationResponse.userId;

            return true; // Registration successful
        }
        else
        {
            // Handle other HTTP status codes if needed
            Debug.LogError("Login Error: " + response.StatusCode);
            return false; // Registration failed
        }
    }
    catch (HttpRequestException e)
    {
        // Handle other exceptions if needed
        Debug.LogError("Login HTTP Request Exception: " + e.Message);
        return false; // Registration failed due to exception
    }
}

    public async Task<bool> PostResponses(List<string> answers)
{
    string apiUrl = $"{url}/users/{myId}/responses";

    try
    {
        // Serialize the list of answers directly to JSON
        string jsonRequest = JsonConvert.SerializeObject(answers);
        HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            Debug.Log("Responses posted successfully.");
            return true; // Responses posted successfully
        }
        else
        {
            Debug.LogError("PostResponsesError: " + response.StatusCode);
            return false; // Posting responses failed
        }
    }
    catch (HttpRequestException e)
    {
        Debug.LogError("PostResponses HTTP Request Exception: " + e.Message);
        return false; // Posting responses failed due to exception
    }
}

// TODO: This method should be called when in proximity to another character
public async Task<UserData> GetUser(Guid userId)
{
    Debug.Log("Called GetUser in httpClient userid: " + userId.ToString());
    string apiUrl = $"{url}/users/{userId}";

    try
    {
        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            UserData userData = JsonConvert.DeserializeObject<UserData>(jsonResponse);
            return userData;
        }
        else
        {
            Debug.LogError("GetUser Error: " + response.StatusCode);
            return null; // May need to change null
        }
    }
    catch (HttpRequestException e)
    {
        Debug.LogError("GetUser HTTP Request Exception: " + e.Message);
        return null; 
    }
}

public async Task<List<ChatMessage>> GetChatHistory(Guid senderId, Guid receiverId) {
    string apiUrl = $"{url}/chats/history?userA_Id={senderId}&userB_Id={receiverId}";

    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

    if (response.IsSuccessStatusCode) {
        string jsonResponse = await response.Content.ReadAsStringAsync();
        List<ChatMessage> chatHistory = JsonConvert.DeserializeObject<List<ChatMessage>>(jsonResponse);
    
        return chatHistory;
    } else {
        Debug.LogError("GetChatHistory Error: " + response.StatusCode);
        return null; // May need to change null
    }
    
}
// Gets the users that are in the world
public async Task<List<UserData>> GetWorldUsers(Guid worldId) {
    string apiUrl = $"{url}/worlds/{worldId}/users";

    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

    if (response.IsSuccessStatusCode) {
        string jsonResponse = await response.Content.ReadAsStringAsync();
        List<UserData> worldUsers = JsonConvert.DeserializeObject<List<UserData>>(jsonResponse);
    
        return worldUsers;
    } else {
        Debug.LogError("GetWorldUsers Error: " + response.StatusCode);
        return null; // May need to change null
    }
    
}

// Gets a list of hatched agent ids
public async Task<List<HatchedData>> GetHatched(Guid worldId) {
    string apiUrl = $"{url}/worlds/{worldId}/agents/hatched";

    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

    if (response.IsSuccessStatusCode) {
        string jsonResponse = await response.Content.ReadAsStringAsync();
        List<HatchedData> hatchedAgents = JsonConvert.DeserializeObject<List<HatchedData>>(jsonResponse);
    
        return hatchedAgents;
    } else {
        Debug.LogError("GetHatched Error: " + response.StatusCode);
        return null; // May need to change null
    }
}
// Gets a list of incubating agent ids
public async Task<List<IncubatingData>> GetIncubating(Guid worldId) {
    string apiUrl = $"{url}/worlds/{worldId}/agents/incubating";

    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

    if (response.IsSuccessStatusCode) {
        string jsonResponse = await response.Content.ReadAsStringAsync();
        List<IncubatingData> incubatingAgents = JsonConvert.DeserializeObject<List<IncubatingData>>(jsonResponse);
    
        return incubatingAgents;

    } else {
        Debug.LogError("GetIncubating Error: " + response.StatusCode);
        return null; // May need to change null
    }
}

public async Task<AgentData> GetAgent(Guid agentId) {
    string apiUrl = $"{url}/agents/{agentId}";

    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

    if (response.IsSuccessStatusCode) {
        string jsonResponse = await response.Content.ReadAsStringAsync();
        AgentData agent = JsonConvert.DeserializeObject<AgentData>(jsonResponse);
    
        return agent;
    } else {
        Debug.LogError("GetAgent Error: " + response.StatusCode);
        return null; // May need to change null
    }
}

public async Task<bool> CreateAgent(string username, string description, Guid creatorId, int incubationDurationInHours)
{
    string apiUrl = $"{url}/agents";

    try
    {
        CreateAgentData createAgentData = new CreateAgentData
        {
            Username = username,
            Description = description,
            CreatorId = creatorId,
            IncubationDurationInHours = incubationDurationInHours
        };

        string jsonRequest = JsonConvert.SerializeObject(createAgentData);
        HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            CreateAgentResponse createAgentResponse = JsonConvert.DeserializeObject<CreateAgentResponse>(jsonResponse);
            Debug.Log("Agent created successfully, with ID: " + createAgentResponse.agentId);
            return true; // Create agent successful
        }
        else
        {
            Debug.LogError("Create Agent Error: " + response.StatusCode);
            return false; // Create agent failed
        }
    }
    catch (HttpRequestException e)
    {
        // Handle other exceptions if needed
        Debug.LogError("Login HTTP Request Exception: " + e.Message);
        return false; // Registration failed due to exception
    }
}

    public async Task<List<UserQuestion>> GetUserQuestions()
    {
        string apiUrl = $"{url}/questions/users";

        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            List<UserQuestion> userQuestions = JsonConvert.DeserializeObject<List<UserQuestion>>(jsonResponse);
            return userQuestions;
        }
        else
        {
            Debug.LogError("GetUserQuestions Error: " + response.StatusCode);
            return null;
        }
    }

    public class UserQuestion
    {
        public Guid id;
        public string question;
    }

    public class HatchedData
    {
        public Guid id;
        public DateTime hatchTime;
    }

    public class IncubatingData
    {
        public Guid id;
        public DateTime hatchTime;
    }

    public class CharacterData
    {
        public Guid id;
        public string username;
        public string summary;
        public Location location;
        public string sprite_URL;
        public string sprite_headshot_URL;
        public string createdTime;
    }
    [System.Serializable]
    public class AgentData : CharacterData
    {
        public string description;
        public Guid creatorId;
        public bool isHatched;
        public string hatchTime;
    }

    [System.Serializable]
    public class UserData : CharacterData
    {
        public string email;
        public bool isOnline;
    }

[System.Serializable]
public class CreateAgentData
{
    public string Username;
    public string Description;
    public Guid CreatorId;
    public int IncubationDurationInHours;
}

    [System.Serializable]
    public class CreateAgentResponse
    {
        public Guid agentId;
    }

    [System.Serializable]
    public class UpdateSprite
    {
        public string Description;
        public bool isURL;
    }

    [System.Serializable]
    public class Location
    {

        [JsonProperty("x_coord")]
        public int coordX;
        [JsonProperty("y_coord")]
        public int coordY;
    }
    [System.Serializable]
    public class ChatMessage
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("senderId")]
        public Guid SenderId { get; set; }

        [JsonProperty("receiverId")]
        public Guid ReceiverId { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("isGroupChat")]
        public bool IsGroupChat { get; set; }
        [JsonProperty("isOnline")]
        public bool IsOnline { get; set; }

        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }

    }

    public class UserWorld
    {
        public Guid id;
        public string name;
        public string description;
        public string thumbnail_url;
    }
    public Guid MyId
    {
        get { return myId; }
    }

    public Guid CurrentWorldId
    {
        get { return worldId; }
    }

}
}

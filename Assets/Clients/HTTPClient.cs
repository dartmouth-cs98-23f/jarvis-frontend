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
        private const string url = "https://api.simugameservice.lekina.me";  

        private Guid myId;
        private Guid worldId;
        private string authToken;
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

        public async Task<bool> RegisterUser(string username, string email, string password)
        {
            string apiUrl = $"{url}/authentication/register";

            try
            {
                UserRegistrationData userData = new UserRegistrationData
                {   
                    username = username,
                    email = email,
                    password = password
                };

                string jsonRequest = JsonConvert.SerializeObject(userData);
                HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    UserRegistrationResponse registrationResponse = JsonConvert.DeserializeObject<UserRegistrationResponse>(jsonResponse);
                    Debug.Log("User registered successfully. ID: " + registrationResponse.id.ToString() + ", Response String: " + registrationResponse.authToken);
                    myId = registrationResponse.id;
                    authToken = registrationResponse.authToken;
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

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

        public async Task<bool> LoginUser(string email, string password)
        {
            string apiUrl = $"{url}/authentication/login";

            Debug.Log("login called");
            try
            {
                UserLoginData loginData = new UserLoginData
                {
                    email = email,
                    password = password
                };

                string jsonRequest = JsonConvert.SerializeObject(loginData);
                HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    UserLoginResponse loginResponse = JsonConvert.DeserializeObject<UserLoginResponse>(jsonResponse);
                    Debug.Log("User logged in successfully. ID: " + loginResponse.id.ToString() + ", Response String: " + loginResponse.authToken);
                    myId = loginResponse.id;
                    authToken = loginResponse.authToken;
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

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

        public class PostResponsesRequest
        {
            public Guid targetId;
            public Guid responderId;
            public List<PostResponse> responses;
        }

        public class PostResponse
        {
            public Guid questionId;
            public string response;
        }

        public async Task<bool> PostResponses(Guid targetId, Guid responderId, List<PostResponse> responses)
        {
            string apiUrl = $"{url}/questions/responses";

            try
            {
                PostResponsesRequest req = new PostResponsesRequest{
                    targetId = targetId, 
                    responderId = responderId, 
                    responses = responses,
                };
                string jsonRequest = JsonConvert.SerializeObject(req);
                HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Responses posted successfully. Count: " + req.responses.Count);
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

        public async Task<List<UserQuestion>> GetUserQuestions()
        {
            string apiUrl = $"{url}/questions/users";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<UserQuestion> userQuestions = JsonConvert.DeserializeObject<List<UserQuestion>>(jsonResponse);
                Debug.Log("GetUserQuestons response: " + userQuestions.Count);
                return userQuestions;
            }
            else
            {
                Debug.LogError("GetUserQuestions Error: " + response.StatusCode);
                return null;
            }
        }


        // Gets the worlds that a user have
        public async Task<List<UserWorld>> GetUserWorlds(Guid id) {
            string apiUrl = $"{url}/users/{id}/worlds";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode) {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<UserWorld> userWorlds = JsonConvert.DeserializeObject<List<UserWorld>>(jsonResponse);

                return userWorlds;
            } else {
                Debug.LogError("GetUserWorlds Error: " + response.StatusCode);
                return null; // May need to change null
            }
        }

        public async Task<Guid?> GetWorldIdFromWorldCode(string worldCode) {
            string apiUrl = $"{url}/code/{worldCode}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode) {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                GetWorldIdFromWorldCodeResponse getWorldIdResponse = JsonConvert.DeserializeObject<GetWorldIdFromWorldCodeResponse>(jsonResponse);
                Debug.Log("GetWorldIdFromWorldCode response: " + getWorldIdResponse.id);
                return getWorldIdResponse.id;
            } else {
                Debug.LogError("GetWorldIdFromWorldCode Error: " + response.StatusCode);

                return null; // May need to change null
            }
        }



        public class UserSummary {
            public string summary;
        }

        public class AddUserToWorldResponse
        {
            public Guid id;
            public Guid creatorId;
            public string name;
            public string description;
            public string thumbnail_URL;
        }

        public class GetWorldIdFromWorldCodeResponse
        {
            public Guid id;
        }

        public class CreateWorldData
        {   
            public Guid creatorId;
            public string name;
            public string description;
        }

        public class CreateWorldResponse
        {
            public Guid id;
            public string name;
            public string thumbnail_URL;
        }

        public async Task<string> GetUserSummary(Guid userId)
        {
            Debug.Log("Called GetUserSummary in httpClient userid: " + userId.ToString());
            string apiUrl = $"{url}/users/{userId}/summary";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    UserSummary userSummary = JsonConvert.DeserializeObject<UserSummary>(jsonResponse);
                    return userSummary.summary;
                }
                else
                {
                    Debug.Log("GetUserSummary Error: " + response.StatusCode);
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("GetUserSummary HTTP Request Exception: " + e.Message);
                return null; 
            }
        }

        public class UpdateUserSummaryData
        {
            public string summary;
        }


        public async Task<bool> UpdateUserSummary(Guid userId, string summary)
        {
            string apiUrl = $"{url}/users/{userId}/summary";

            try
            {
                UpdateUserSummaryData req = new UpdateUserSummaryData{
                    summary = summary
                };
                string jsonRequest = JsonConvert.SerializeObject(req);
                HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Summary updated successfully.");
                    return true;
                }
                else
                {
                    Debug.LogError("UpdateUserSummary: " + response.StatusCode);
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("UpdateUserSummary HTTP Request Exception: " + e.Message);
                return false; // Update summary failed due to exception
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

        public async Task<bool> DeleteWorld(Guid worldId)
        {
            string apiUrl = $"{url}/worlds/{worldId}";

            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("World deleted successfully from user with world id: " + worldId);
                    return true;
                }
                else
                {
                    Debug.LogError("Delete world failed: " + response.StatusCode);
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                // Handle other exceptions if needed
                Debug.LogError("HTTP Request Exception: " + e.Message);
                return false; // Delete world failed due to exception
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

        // Gets the agents that are in the world
        public async Task<List<AgentData>> GetWorldAgents(Guid worldId) {
            string apiUrl = $"{url}/worlds/{worldId}/agents";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode) {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<AgentData> worldAgents = JsonConvert.DeserializeObject<List<AgentData>>(jsonResponse);
            
                return worldAgents;
            } else {
                Debug.LogError("GetWorldAgents Error: " + response.StatusCode);
                return null;
            }
            
        }

        public async Task<IdData> CreateAgent(string username, string description, Guid creatorId, int incubationDurationInHours)
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
                    IdData agentId = JsonConvert.DeserializeObject<IdData>(jsonResponse);
                    Debug.Log("Agent created successfully, with ID: " + agentId);
                    return agentId; // Create agent successful
                }
                else
                {
                    Debug.LogError("Create Agent Error: " + response.StatusCode);
                    return null; // Create agent failed
                }
            }
            catch (HttpRequestException e)
            {
                // Handle other exceptions if needed
                Debug.LogError("Login HTTP Request Exception: " + e.Message);
                return null; // Registration failed due to exception
            }
        }

        public async Task<List<ResponseData>> GetResponses(Guid agentId){
            string apiUrl = $"{url}/questions/responses/{agentId}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode) {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<ResponseData> responses = JsonConvert.DeserializeObject<List<ResponseData>>(jsonResponse);
            
                return responses;
            } else {
                Debug.LogError("GetResponses Error: " + response.StatusCode);
                return null; // May need to change null
            }
        }

        public async Task<List<QuestionResponseData>> GetQuestionResponse(Guid agentId, Guid questionId){
            string apiUrl = $"{url}/questions/responses/{agentId}/{questionId}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode) {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<QuestionResponseData> responses = JsonConvert.DeserializeObject<List<QuestionResponseData>>(jsonResponse);
            
                return responses;
            } else {
                Debug.LogError("GetQuestionResponse Error: " + response.StatusCode);
                return null; // May need to change null
            }
        }

        public async Task<List<QuestionData>> GetAgentQuestions(){
            string apiUrl = $"{url}/questions/agents";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode) {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<QuestionData> questions = JsonConvert.DeserializeObject<List<QuestionData>>(jsonResponse);
            
                return questions;
            } else {
                Debug.LogError("GetAgentQuestions Error: " + response.StatusCode);
                return null; // May need to change null
            }
        }
        public async Task<CreateWorldResponse> CreateWorld(Guid creatorId, string name, string description)
        {
            string apiUrl = $"{url}/worlds";

            try
            {
                CreateWorldData createWorldData = new CreateWorldData
                {
                    creatorId = creatorId,
                    name = name,
                    description = description
                };

                string jsonRequest = JsonConvert.SerializeObject(createWorldData);
                HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    CreateWorldResponse createWorldResponse = JsonConvert.DeserializeObject<CreateWorldResponse>(jsonResponse);
                    Debug.Log("World created successfully, with ID: " + createWorldResponse.id);
                    return createWorldResponse; // Create world successful
                }
                else
                {
                    Debug.LogError("Create world Error: " + response.StatusCode);
                    return null; // Create world failed
                }
            }
            catch (HttpRequestException e)
            {
                // Handle other exceptions if needed
                Debug.LogError("CreateWorld HTTP Request Exception: " + e.Message);
                return null;
            }
        }

        public class UserQuestion
        {
            public Guid id;
            public string question;
        }

        public async Task<bool> RemoveUserFromWorld(Guid userId)
        {
            string apiUrl = $"{url}/worlds/{worldId}/users/{userId}";
            try
            {
                // Send the DELETE request
                HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);
                
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // No content is returned for a successful deletion
                    return true;
                }
                else
                {
                    // Log the error if the request fails
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception e)
            {
                // Log any exceptions that occur
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveWorldFromList(Guid worldId, Guid userId)
        {
            string apiUrl = $"{url}/worlds/{worldId}/users/{userId}";
            Debug.Log("Removing world with id: " + worldId + " from user with id: " + userId);
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("World removed successfully to user with world id: " + worldId + " and user id: " + userId);
                    return true;
                }
                else
                {
                    Debug.LogError("Remove world failed: " + response.StatusCode);
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                // Handle other exceptions if needed
                Debug.LogError("HTTP Request Exception: " + e.Message);
                return false; // Remove world failed due to exception
            }
        }

        public async Task<AddUserToWorldResponse> AddUserToWorld(Guid worldId, Guid userId)
        {
            string apiUrl = $"{url}/worlds/{worldId}/users/{userId}";

            try
            {
                // no content needed for this post request. this is just needed for Post request
                HttpContent content = new StringContent("", System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    AddUserToWorldResponse addWorldResponse = JsonConvert.DeserializeObject<AddUserToWorldResponse>(jsonResponse);
                    Debug.Log("World added successfully to user with world id: " + addWorldResponse.id + " and creator id: " + addWorldResponse.creatorId + " and name: " + addWorldResponse.name + " and description: " + addWorldResponse.description + " and thumbnail_URL: " + addWorldResponse.thumbnail_URL);
                    return addWorldResponse;
                }
                else
                {
                    Debug.LogError("Add world failed: " + response.StatusCode);
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                // Handle other exceptions if needed
                Debug.LogError("HTTP Request Exception: " + e.Message);
                return null; // Registration failed due to exception
            }
        }

        public async Task<IdData> GetWorldCreator(){
            string apiUrl = $"{url}/worlds/{worldId}/creator";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode) {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                IdData creatorId = JsonConvert.DeserializeObject<IdData>(jsonResponse);
            
                return creatorId;
            } else {
                Debug.LogError("GetWorldCreator Error: " + response.StatusCode);
                return null; // May need to change null
            }
        }

        public async Task<bool> AddAgentToWorld(Guid agentId)
        {
            string apiUrl = $"{url}/worlds/{worldId}/agents";

            try
            {
                // Create a request object containing the agent's ID
                var requestData = new
                {
                    agentId = agentId
                };

                // Serialize the request object to JSON
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                // Send the POST request
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.Log("Responses posted successfully.");
                    return true; // Responses posted successfully
                }
                else
                {
                    Debug.LogError("AddAgentToWorldError: " + response.StatusCode);
                    return false; // Posting responses failed
                }
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("AddAgentToWorld HTTP Request Exception: " + e.Message);
                return false; // Posting responses failed due to exception
            }
        }

        public class CharacterData
        {
            public Guid id;
            public string username;
            public string summary;
            public Location location;
            public string sprite_URL;
            public string sprite_headshot_URL;
            public DateTime createdTime;
        }
        [System.Serializable]
        public class AgentData : CharacterData
        {
            public string description;
            public Guid creatorId;
            public bool isHatched;
            public DateTime hatchTime;
        }

        [System.Serializable]
        public class UserData : CharacterData
        {
            public string email;
            public bool isOnline;
        }

        [System.Serializable]
        public class UserRegistrationData
        {
            public string username;
            public string email;
            public string password;
        }

        [System.Serializable]
        public class UserLoginData
        {
            public string email;
            public string password;
        }

        [System.Serializable]
        public class UserRegistrationResponse
        {
            public Guid id;
            public string authToken;
        }

        [System.Serializable]
        public class UserLoginResponse
        {
            public Guid id;
            public string authToken;
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
        public class HatchedData
        {
            public Guid id;
            public DateTime hatchedTime;
        }
        [System.Serializable]
        public class IncubatingData
        {
            public Guid id;
            public DateTime hatchedTime;
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
            public Guid creatorId;
            public string name;
            public string description;
            public string thumbnail_URL;
        }
        
        [System.Serializable]
        public class ResponseData
        {
            public Guid responderId;
            public Guid questionId;
            public string response;
        }
        [System.Serializable]
        public class QuestionData
        {
            public Guid id;
            public string question;
        }

        [System.Serializable]
        public class QuestionResponseData
        {
            public Guid responderId;
            public string response;
        }

        [System.Serializable]
        public class IdData
        {
            public Guid id;
        }
        public Guid MyId
        {
            get { return myId; }
        }
        public Guid WorldId
        {
            get { return worldId; }
        }

        public string AuthToken
        {
            get { return authToken; }
        }

        public Guid CurrentWorldId { get; set; }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;


namespace Clients {

public class HTTPClient
{
    private static HTTPClient instance;
    private readonly HttpClient httpClient = new HttpClient();
    private const string url = "http://localhost:5087";  
    private string myId;

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

    private void OnDestroy()
    {
        // Dispose of the HttpClient when the script is destroyed to prevent resource leaks
        httpClient.Dispose();
    }

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
            Debug.LogError("Error: " + response.StatusCode);
            return false; // Registration failed
        }
    }
    catch (HttpRequestException e)
    {
        // Handle other exceptions if needed
        Debug.LogError("HTTP Request Exception: " + e.Message);
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
            Debug.LogError("Error: " + response.StatusCode);
            return false; // Posting responses failed
        }
    }
    catch (HttpRequestException e)
    {
        Debug.LogError("HTTP Request Exception: " + e.Message);
        return false; // Posting responses failed due to exception
    }
}

public async Task<UserData> GetUser(string userId)
{
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
            // Handle other HTTP status codes if needed
            Debug.LogError("Error: " + response.StatusCode);
            return null;
        }
    }
    catch (HttpRequestException e)
    {
        // Handle other exceptions if needed
        Debug.LogError("HTTP Request Exception: " + e.Message);
        return null; 
    }
}

[System.Serializable]
public class UserData
{
    public string firstName;
    public string lastName;
    public string email;
    public int lastKnownX;
    public int lastKnownY;
    public bool isLoggedIn;
    public DateTime createdTime;
    public DateTime lastActiveTime;
}


    [System.Serializable]
    public class UserRegistrationData
    {
        public string FirstName;
        public string LastName;
        public string Password;
        public string Email;
    }

    [System.Serializable]
    public class UserRegistrationResponse
    {
        public string userId;
        public string responseString;
    }

}
}

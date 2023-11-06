using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Clients {

public class HTTPClient
{
    private readonly HttpClient httpClient = new HttpClient();
    private const string url = "http://localhost:5087";  

    private void OnDestroy()
    {
        // Dispose of the HttpClient when the script is destroyed to prevent resource leaks
        httpClient.Dispose();
    }

    public async Task<bool> RegisterUser(string firstName, string lastName, string email, string password)
{
    string apiUrl = url + "/Authentication/register";

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
            Debug.Log("User registered successfully. ID: " + registrationResponse.Id + ", AuthToken: " + registrationResponse.AuthToken);

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
        public string Id;
        public string AuthToken;
    }
}
}

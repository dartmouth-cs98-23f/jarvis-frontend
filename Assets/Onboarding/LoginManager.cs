using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Clients;

public class LoginManager : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public GameObject LoginPanel;
    public GameObject LandingPanel;

    public async void OnLoginButtonPressed()
    {   
        string email = emailInput.text;
        string password = passwordInput.text;

        // TODO: Add an UI error message for users
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.Log("Please fill out all fields");
            return;
        }

        bool loginSuccessful = true;
        // TODO: Replace this with signin API call
        // bool loginSuccessful = await SignIn(email, password);

        if (loginSuccessful)
        {
            SceneNavigator.LoadGame();
            Debug.Log("Successfully logged in with email: " + email + " and password: " + password);
        }
        else // TODO: Add UI error message for users
        {
            Debug.Log("Failed to login due to backend error");
        }
    }

    public void OnBackButtonPressed()
    {
        LoginPanel.SetActive(false);
        LandingPanel.SetActive(true);
    }

    private async Task<bool> SignIn(string email, string password)
    {
        HTTPClient httpClient = HTTPClient.Instance;
        bool loginSuccessful = true;         // comment out this line to connect with backend API.
        // bool registrationSuccessful = await httpClient.RegisterUser(firstname, lastname, email, password); // TODO: update firstname lastname with username and uncomment
        if (loginSuccessful){
            return true;
        }
        else{
            return false; 
        }
    }

}

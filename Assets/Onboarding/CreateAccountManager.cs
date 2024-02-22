using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class CreateAccountNavigator : MonoBehaviour
{

    public InputField usernameInput;
    public InputField emailInput;
    public InputField passwordInput;
    public InputField confirmPasswordInput;
    public GameObject LandingPanel;
    public GameObject RegisterPanel;
    public GameObject InfoPanel;
    public Text usernameErrorText;
    public Text emailErrorText;
    public Text passwordErrorText;
    public Text confirmPasswordErrorText;
    private HTTPClient httpClient = HTTPClient.Instance;

    void Start()
    {
        RegisterPanel.SetActive(true);
        usernameInput.onValueChanged.AddListener(delegate { ValidateUsername(); });
        emailInput.onValueChanged.AddListener(delegate { ValidateEmail(); });
        passwordInput.onValueChanged.AddListener(delegate { ValidatePassword(); });
        confirmPasswordInput.onValueChanged.AddListener(delegate { ValidateConfirmPassword(); });
    }


    bool ValidateUsername()
    {
        string username = usernameInput.text;
        string error = InputValidation.ValidateUsername(username);
        usernameErrorText.text = error;
        if (string.IsNullOrEmpty(error)) 
        {
            return true;
        } else
        {
            return false;
        }
    }
    bool ValidateEmail()
    {
        string email = emailInput.text;
        string emailError = InputValidation.ValidateEmail(email);
        emailErrorText.text = emailError;
        if (string.IsNullOrEmpty(emailError)) 
        {
            return true;
        } else
        {
            return false;
        }
    }

    bool ValidatePassword()
    {
        string password = passwordInput.text;
        string passwordError = InputValidation.ValidatePassword(password);
        passwordErrorText.text = passwordError;
        if (string.IsNullOrEmpty(passwordError)) 
        {
            return true;
        } else
        {
            return false;
        }
    }

    bool ValidateConfirmPassword()
    {
        string error = InputValidation.ValidateConfirmPassword(passwordInput.text, confirmPasswordInput.text);
        confirmPasswordErrorText.text = error;
        if (string.IsNullOrEmpty(error)) 
        {
            return true;
        } else
        {
            return false;
        }
    }

    public async void OnCreateAccountButtonClicked()
    {   
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        bool usernameIsValid = ValidateUsername();
        bool emailIsValid = ValidateEmail();
        bool passwordIsValid = ValidatePassword();
        bool confirmPasswordIsValid = ValidateConfirmPassword();

        if (usernameIsValid && emailIsValid && passwordIsValid && confirmPasswordIsValid) 
        {
            // TODO: Get error message from backend and display it
            bool registrationSuccessful = await CreateAccount(username, email, password, confirmPassword);

            if (registrationSuccessful)
            {
                NavigateToInfoPanel();
                Debug.Log("Successfully created account with username: " + username + " email: " + email + " and password: " + password + "");
                await SignalRClient.Initialize(httpClient.AuthToken);
            }
            else
            {
                Debug.Log("Failed to create account due to backend error");
            }
        }
    }

    public void OnBackButtonPressed()
    {
        RegisterPanel.SetActive(false);
        LandingPanel.SetActive(true);
    }

    // returns true if the account was successfully created, false otherwise
    // "successful"
    // "error: email already exists"
    //  "error: password must be at least 8 characters"
    // "error: passwords do not match"
    // "error: email is not valid"
    private async Task<bool> CreateAccount(string username, string email, string password, string confirmPassword)
    {
        // bool registrationSuccessful = true;         // comment out this line to connect with backend API.
        bool registrationSuccessful = await httpClient.RegisterUser(username, email, password); 
        if (registrationSuccessful){
            return true;
    }
        else{
            return false; 
        }
    }

    private void NavigateToInfoPanel()
    {
        RegisterPanel.SetActive(false);
        InfoPanel.SetActive(true);
    }
}

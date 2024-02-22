using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Clients;
using System.Text.RegularExpressions;

public class LoginManager : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public GameObject LoginPanel;
    public GameObject LandingPanel;
    public Text emailErrorText;
    public Text passwordErrorText;
    private HTTPClient httpClient = HTTPClient.Instance;

    void Start()
    {
        emailInput.onValueChanged.AddListener(delegate { ValidateEmail(); });
        passwordInput.onValueChanged.AddListener(delegate { ValidatePassword(); });
    }

    public async void OnLoginButtonPressed()
    {   
        bool emailIsValid = ValidateEmail();
        bool passwordIsValid = ValidatePassword();
        string email = emailInput.text;
        string password = passwordInput.text;
        if (emailIsValid && passwordIsValid) {
            // TODO: Replace this with signin API call
            // bool loginSuccessful = true;
            bool loginSuccessful = await httpClient.LoginUser(email, password);
            if (loginSuccessful)
            {
                SceneNavigator.LoadMyWorlds(); // TODO: Replace this with navigating to logged in user's worlds scene
                Debug.Log("Successfully logged in with email: " + emailInput.text + " and password: " + passwordInput.text);
                await SignalRClient.Initialize(httpClient.AuthToken);
            }
            else // TODO: Add UI error message for users
            {
                Debug.Log("Failed to login due to backend error");
            }
        }
    }

    public void OnBackButtonPressed()
    {
        LoginPanel.SetActive(false);
        LandingPanel.SetActive(true);
    }

    bool ValidateEmail()
    {
        string emailError = InputValidation.ValidateEmail(emailInput.text);
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
        string passwordError = InputValidation.ValidatePassword(passwordInput.text);
        passwordErrorText.text = passwordError;
        if (string.IsNullOrEmpty(passwordError)) 
        {
            return true;
        } else
        {
            return false;
        }
    }

}

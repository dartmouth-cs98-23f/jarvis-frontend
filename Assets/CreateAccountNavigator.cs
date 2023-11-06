using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class CreateAccountNavigator : MonoBehaviour
{

    public InputField firstNameInput;
    public InputField lastNameInput;
    public InputField emailInput;
    public InputField passwordInput;
    public InputField confirmPasswordInput;
    public GameObject RegisterPanel;
    public GameObject QuestionPanels;
    // Start is called before the first frame update
    void Start()
    {
        RegisterPanel.SetActive(true);
        QuestionPanels.SetActive(false);
    }

    public async void OnCreateAccountButtonClicked()
    {
        string firstName = firstNameInput.text;
        string lastName = lastNameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        // TODO: Add an UI error message for users
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            Debug.Log("Please fill out all fields");
            return;
        }

        // TODO: Add UI error message for users
        if (password != confirmPassword)
        {
            Debug.Log("Passwords do not match");
            return;
        }

        // TODO: Get error message from backend and display it
        bool registrationSuccessful = await CreateAccount(firstName, lastName, email, password, confirmPassword);

        if (registrationSuccessful)
        {
            NavigateToQuestionPanels();
            Debug.Log("Successfully created account with first name: " + firstName + ", last name: " + lastName + " email: " + email + " and password: " + password + "");
        }
        else
        {
            Debug.Log("Failed to create account due to backend error");
        }
    }

    // TODO: this is a temporary method that should be calling the backend's create account method instead
    // returns true if the account was successfully created, false otherwise
    // "successful"
    // "error: email already exists"
    //  "error: password must be at least 8 characters"
    // "error: passwords do not match"
    // "error: email is not valid"
    private async Task<bool> CreateAccount(string firstName, string lastName, string email, string password, string confirmPassword)
{
    HTTPClient httpClient = HTTPClient.Instance;
    bool registrationSuccessful = await httpClient.RegisterUser(firstName, lastName, email, password);
    if (registrationSuccessful){
        return true;
    }
    else{
        return false; 
    }
}


    private void NavigateToQuestionPanels()
    {
        RegisterPanel.SetActive(false);
        QuestionPanels.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

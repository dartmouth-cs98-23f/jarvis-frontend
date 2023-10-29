using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccount : MonoBehaviour
{

    public InputField firstNameInput;
    public InputField lastNameInput;
    public InputField emailInput;
    public InputField passwordInput;
    public InputField confirmPasswordInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnCreateAccountButtonClicked()
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

        // TODO: Get error message from backend and display it
        if (createAccount(firstName, lastName, email, password, confirmPassword))
        {
            Debug.Log("Successfully created account with first name: " + firstName + ", last name: " + lastName + " email: " + email + " and password: " + password + "");
        } else
        {
            Debug.Log("Failed to create account due to backend error");
        }
    }

    // TODO: this is a temporary method that should be calling the backend's create account method instead
    // returns true if the account was successfully created, false otherwise
    private bool createAccount(string firstName, string lastName, string email, string password, string confirmPassword)
    {
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

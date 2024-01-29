using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class InputValidation : MonoBehaviour
{
    public static string ValidateUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return "Username is required.";
        }
        else if (username.Length < 3)
        {
            return "Username must be at least 3 characters long.";
        }
        else if (username.Length > 10)
        {
            return "Username must be at most 10 characters long.";
        }
        else if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
        {
            return "Username can only contain letters, numbers, and underscores.";
        }
        return ""; // No error
    }
    public static string ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return "Email address is required.";
        }
        else if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            return "Please enter a valid email address.";
        }

        // Add any other specific checks here if necessary

        return ""; // No error
    }

    public static string ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return "Password is required.";
        }
        else if (password.Length < 6)
        {
            return "Password must be at least 6 characters long.";
        }

        // Add any other specific password checks here (e.g., complexity)

        return ""; // No error
    }

    public static string ValidateConfirmPassword(string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(confirmPassword))
        {
            return "Confirm password is required.";
        }
        else if (password != confirmPassword)
        {
            return "Passwords do not match.";
        }

        return ""; // No error
    }

    public static string CheckEmpty(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "This field is required.";
        }
        return ""; // No error
    }
}

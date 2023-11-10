using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class IntroQuestions : MonoBehaviour
{
    
    public InputField question1Input;
    public InputField question2Input;
    public InputField question3Input;
    public InputField question4Input;
    private int clickCount = 0; // Counter variable to keep track of OnQuestionClicked calls
    public List<string> answers = new List<string>();

    
public async void OnQuestionClicked()
{
    clickCount++; // Increment the counter every time the function is called

    string currentAnswer = GetAnswerForQuestion(clickCount);
    
    // Check the count and validate answers accordingly
    if (string.IsNullOrEmpty(currentAnswer))
    {
        // TODO: Add UI error message for users
        Debug.Log($"Please provide an answer for question {clickCount} before proceeding.");
        return;
    }
    else if (clickCount >= 4)
    {
        Debug.Log("All answers provided:");
        answers.Add(GetAnswerForQuestion(1));
        answers.Add(GetAnswerForQuestion(2));
        answers.Add(GetAnswerForQuestion(3));
        answers.Add(GetAnswerForQuestion(4));
        
        // Reset the click count for future use if needed
        clickCount = 0;

        HTTPClient httpClient = HTTPClient.Instance;
        bool registrationSuccessful = await httpClient.PostResponses(answers);

        if (registrationSuccessful)
        {
            Debug.Log("Successfully posted responses");
        }
        else
        {
            Debug.Log("Failed to create account due to backend error");
        }
    }

    // Helper function to get the answer based on the click count
    string GetAnswerForQuestion(int questionNumber)
    {
        switch (questionNumber)
        {
            case 1:
                return question1Input.text;
            case 2:
                return question2Input.text;
            case 3:
                return question3Input.text;
            case 4:
                return question4Input.text;
            default:
                return null;
        }
    }
}

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class CollabInfoComponent : MonoBehaviour
{
    public TextMeshProUGUI questionTMP;
    public TextMeshProUGUI answeredByTMP;
    public int questionId;
    public GameObject viewAnswersPanel;
    public GameObject typeAnswerPanel;
    public List<string> answers;
    public Button viewAnswersButton;

    // A method to set the collab details
    public void SetQuestionDetails(string question, int id)
    {
        questionId = id;
        questionTMP.text = "Q" + id.ToString() + ". " + question;
        
    }
    public void SetAnswerDetails()
    {
        string finalText = "";

        // Loop through each answer
        for (int i = 0; i < answers.Count - 1; i++)
        {
            // Retrieve the answer at index i
            string ans = answers[i];

            // Append the answer and a comma to the final text
            finalText += ans + ", ";
        }

        // Append the last answer (without a comma)
        if (answers.Count > 0)
        {
            finalText += answers[answers.Count - 1];
        }

        // Set the text to the TextMeshProUGUI component
        answeredByTMP.text = "Answered by: " + finalText;
    }

    public void Unanswered(){
        viewAnswersButton.gameObject.SetActive(false);
        answeredByTMP.gameObject.SetActive(false);
    }

    public void ViewAnswers()
    {
        // Toggle the panel visibility
        if (viewAnswersPanel != null)
        {
            viewAnswersPanel.SetActive(!viewAnswersPanel.activeSelf);
        }
        else
        {
            Debug.LogError("View Answers Panel reference is not set.");
        }
    }

    public void Answer()
    {
        // Toggle the panel visibility
        if (typeAnswerPanel != null)
        {
            typeAnswerPanel.SetActive(!typeAnswerPanel.activeSelf);
        }
        else
        {
            Debug.LogError("Type Answers Panel reference is not set.");
        }
    }

}

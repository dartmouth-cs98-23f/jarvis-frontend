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
    public GameObject viewAnswersPanel;
    public GameObject typeAnswerPanel;
    public List<string> answers;
    public Button viewAnswersButton;
    public Button answersButton;
    public ViewAnswersManager viewAnswersManager;
    public AddAnswerManager addAnswerManager;
    public Sprite spriteHead;
    public TextMeshProUGUI username;
    public Guid agentID;
    public Guid questionID;

    // A method to set the collab details
    public void SetQuestionDetails(string question, int id)
    {
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
    public void Answered(){
        answersButton.gameObject.SetActive(false);
    }

    public void ViewAnswers()
    {
        viewAnswersManager.SetIDs(agentID, questionID);
        viewAnswersManager.SetPanelDetails(spriteHead, username.text, questionTMP.text);
        viewAnswersButton.interactable = false;
        viewAnswersManager.DisplayAnswers();
        // Toggle the panel visibility
        if (viewAnswersPanel != null)
        {
            viewAnswersPanel.SetActive(!viewAnswersPanel.activeSelf);
            viewAnswersButton.interactable = true;
        }
        else
        {
            Debug.LogError("View Answers Panel reference is not set.");
        }
    }

    public void Answer()
    {
        addAnswerManager.SetIDs(agentID, questionID);
        addAnswerManager.SetPanelDetails(spriteHead, username.text, questionTMP.text);
        answersButton.interactable = false;
        // Toggle the panel visibility
        if (typeAnswerPanel != null)
        {
            typeAnswerPanel.SetActive(!typeAnswerPanel.activeSelf);
            answersButton.interactable = true;
        }
        else
        {
            Debug.LogError("Type Answers Panel reference is not set.");
        }
    }

}

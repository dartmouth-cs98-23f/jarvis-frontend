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

    // A method to set the collab details
    public void SetCollabDetails(string question, int id, string answeredBy)
    {
        questionId = id;
        questionTMP.text = "Q" + id.ToString() + ". " + question;
        answeredByTMP.text = "Answered by: " + answeredBy;
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

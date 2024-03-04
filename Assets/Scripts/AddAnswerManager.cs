using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Clients;
using Newtonsoft.Json;


public class AddAnswerManager : MonoBehaviour
{
    public InputField answerInput;
    public string answer;
    private Guid agentID;
    private Guid questionID;
    private HTTPClient httpClient = HTTPClient.Instance;
    public GameObject spriteHead;
    public GameObject agentName;
    public GameObject question;
    public Text answerError;
    public SideMenu sideMenuManager;

    void Start(){
        answerInput.onValueChanged.AddListener(delegate { CheckEmpty(answerInput, answerError); });
    }

    bool CheckEmpty(InputField inputField, Text errorText)
    {
        string str = inputField.text;
        string error = InputValidation.CheckEmpty(str);
        errorText.text = error;
        if (string.IsNullOrEmpty(error)) 
        {
            return true;
        } else
        {
            return false;
        }
    }
    public void ResetInputFields()
    {
        // Reset the input fields
        answerInput.text = "";
    }

    public void SetIDs(Guid agentId, Guid questionId){
        agentID = agentId;
        questionID = questionId;
    }

    public void SetPanelDetails(Sprite sprite, string name, string q){
        spriteHead.GetComponent<Image>().sprite = sprite;
        agentName.GetComponent<TextMeshProUGUI>().text = name;
        question.GetComponent<TextMeshProUGUI>().text = q;
    }

    public async void SendAnswer()
    {
        try
        {
            // Get the answer from the input field
            answer = answerInput.text;
            
            bool answerIsValid = CheckEmpty(answerInput, answerError);

            if (answerIsValid){
                // Call the PostResponses method to send the answer
                HTTPClient.PostResponseResp sendAnswerResponse = await httpClient.PostResponse(agentID, httpClient.MyId, questionID, answer);
                if (sendAnswerResponse != null)
                {
                    sideMenuManager.ToggleTypeAnswerPanel();
                    Debug.Log("Answer sent successfully. Agent summary is now " + sendAnswerResponse.summary);
                    ResetInputFields();
                }
                else
                {
                    Debug.LogError("Failed to send answer.");
                    // Add error handling logic here if needed
                }
            }
            else {
                Debug.Log("Answer not valid");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error while sending answer: " + e.Message);
        }
    }
}

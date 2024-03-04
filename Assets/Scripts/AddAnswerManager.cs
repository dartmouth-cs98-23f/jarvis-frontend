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

        Debug.Log("answer test " + answer);

        // Call the PostResponses method to send the answer
        HTTPClient.PostResponseResp sendAnswerResponse = await httpClient.PostResponse(agentID, httpClient.MyId, questionID, answer);

        if (sendAnswerResponse != null)
        {
            Debug.Log("Answer sent successfully. Agent summary is now " + sendAnswerResponse.summary);
        }
        else
        {
            Debug.LogError("Failed to send answer.");
            // Add error handling logic here if needed
        }
    }
    catch (Exception e)
    {
        Debug.LogError("Error while sending answer: " + e.Message);
    }
}

}

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
    private HTTPClient httpClient = HTTPClient.Instance;
    public GameObject spriteHead;
    public GameObject agentName;
    public GameObject question;

    public void ResetInputFields()
    {
        // Reset the input fields
        answerInput.text = "";
    }

    public void SetAgentID(Guid agentId){
        agentID = agentId;
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
        string answer = answerInput.text;

        // Create a list to store the question-response data
        List<HTTPClient.PostResponse> responses = new List<HTTPClient.PostResponse>();

        // Add the answer to the responses list
        HTTPClient.PostResponse response = new HTTPClient.PostResponse();
        response.response = answer;
        responses.Add(response);

        // Call the PostResponses method to send the answer
        HTTPClient.PostResponseResp sendAnswerResponse = await httpClient.PostResponses(agentID, httpClient.MyId, responses);

        if (sendAnswerResponse != null)
        {
            Debug.Log("Answer sent successfully.");
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
        // Add additional error handling logic here if needed
    }
}

}

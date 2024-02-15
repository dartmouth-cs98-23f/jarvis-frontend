using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Clients;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;

public class ViewAnswersManager : MonoBehaviour
{
    public GameObject content;
    public GameObject questionAnswerPrefab;
    public GameObject spriteHead;
    public GameObject username;
    private Guid questionID;
    public GameObject question;
    private HTTPClient httpClient = HTTPClient.Instance;
    private Guid agentID;
    
    public void SetIDs(Guid agentId, Guid questionId){
        agentID = agentId;
        questionID = questionId;
    }

    public void SetPanelDetails(Sprite sprite, string name, string q){
        spriteHead.GetComponent<Image>().sprite = sprite;
        username.GetComponent<TextMeshProUGUI>().text = name;
        question.GetComponent<TextMeshProUGUI>().text = q;
    }
    public void localDisplayAnswers(){
        string responsesJson = @"
        [
            {
                ""responserId"": ""1c0f8e3b-1c0f-4c25-96f3-14e1a5d0e693"",
                ""response"": ""What is your favorite color?""
            },
            {
                ""responderId"": ""2b5d9f6c-2b5d-4a12-9f1d-8cfeabf68e8d"",
                ""response"": ""How many siblings do you have?""
            },
            {
                ""responderId"": ""3d0a4e6f-3d0a-4b39-ba7c-9e2d7d8fbdbe"",
                ""response"": ""What is your favorite food?""
            },
            {
                ""responderId"": ""4e8c9a2b-4e8c-45d1-a68b-1f39d1c6a802"",
                ""response"": ""Where were you born?""
            }
        ]";

        List<HTTPClient.QuestionResponseData> testResponses = JsonConvert.DeserializeObject<List<HTTPClient.QuestionResponseData>>(responsesJson);

        foreach (HTTPClient.QuestionResponseData questionResponseData in testResponses){
            GameObject questionResponseGO = Instantiate(questionAnswerPrefab, content.transform);
            questionResponseGO.tag = "QuestionAnswerPrefab";

            ViewAnswersInfoComponent viewAnswersInfoComponent = questionResponseGO.GetComponent<ViewAnswersInfoComponent>();
            viewAnswersInfoComponent.SetDetails("Alan", questionResponseData.response);
        }
    }

    public async void DisplayAnswers(){
        List<HTTPClient.QuestionResponseData> responses = await httpClient.GetQuestionResponse(agentID, questionID);

        foreach (HTTPClient.QuestionResponseData questionResponseData in responses){
            GameObject questionResponseGO = Instantiate(questionAnswerPrefab, content.transform);
            questionResponseGO.tag = "QuestionAnswerPrefab";

            ViewAnswersInfoComponent viewAnswersInfoComponent = questionResponseGO.GetComponent<ViewAnswersInfoComponent>();
            HTTPClient.UserData user = await httpClient.GetUser(questionResponseData.responderId);

            viewAnswersInfoComponent.SetDetails(user.username, questionResponseData.response);
        }

    }

    public void CloseViewAnswersPanel()
    {
        // Iterate through each child of the CollabListPanel
        foreach (Transform child in content.transform)
        {
            // Check if the child has the specified tag
            if (child.gameObject.tag == "QuestionAnswerPrefab")
            {
                // Destroy the prefab instances
                Destroy(child.gameObject);
            }
        }
    }
}

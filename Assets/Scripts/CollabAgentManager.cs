using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Clients;
using Newtonsoft.Json;

public class CollabAgentManager : MonoBehaviour
{
    public GameObject content;

    [System.Serializable]
    public class CollabInfo
    {
        public string username;
        public string response;
    }
    public GameObject collabAgentPrefab;
    public GameObject collabAgentPanel;
    public GameObject viewAnswersPanel;
    public GameObject typeAnswerPanel;
    public TextMeshProUGUI agentName;
    public GameObject spriteHead;
    private Guid agentID;
    private HTTPClient httpClient = HTTPClient.Instance;
    public SpriteLoader spriteLoader;
    public ViewAnswersManager viewAnswersManager;
    public AddAnswerManager addAnswerManager;

    public void SetAgentID(Guid agentId){
        agentID = agentId;
        DisplayCollabQuestions();
    }
    public void SetPanelDetails(Sprite sprite, TextMeshProUGUI name){
        spriteHead.GetComponent<Image>().sprite = sprite;
        agentName.text = name.text;
    }
    public void localDisplayCollabQuestions()
    {
        int count = 1;
        string questionsJson = @"
        [
            {
                ""id"": ""1c0f8e3b-1c0f-4c25-96f3-14e1a5d0e693"",
                ""question"": ""What is your favorite color?""
            },
            {
                ""id"": ""2b5d9f6c-2b5d-4a12-9f1d-8cfeabf68e8d"",
                ""question"": ""How many siblings do you have?""
            },
            {
                ""id"": ""3d0a4e6f-3d0a-4b39-ba7c-9e2d7d8fbdbe"",
                ""question"": ""What is your favorite food?""
            },
            {
                ""id"": ""4e8c9a2b-4e8c-45d1-a68b-1f39d1c6a802"",
                ""question"": ""Where were you born?""
            }
        ]";

        List<HTTPClient.QuestionData> testQuestions = JsonConvert.DeserializeObject<List<HTTPClient.QuestionData>>(questionsJson);
        List<CollabInfo> dummy1 = new List<CollabInfo>();
        List<CollabInfo> dummy2 = new List<CollabInfo>();
        List<CollabInfo> dummy3 = new List<CollabInfo>();
        List<CollabInfo> dummy4 = new List<CollabInfo>();

        CollabInfo collab1 = new CollabInfo();
        collab1.response = "How would Obi Wan Kenobi react to a meteor falling from the sky";
        collab1.username = "Vico";
        dummy1.Add(collab1);

        CollabInfo collab2 = new CollabInfo();
        collab2.response = "Is Obi Wan him?";
        collab2.username = "Alan";
        dummy1.Add(collab2);

        CollabInfo collab3 = new CollabInfo();
        collab3.response = "What color is Obi Wan's lightsaber?";
        collab3.username = "Vico";
        dummy2.Add(collab3);

        CollabInfo collab4 = new CollabInfo();
        collab4.response = "Is Obi Wan the best Jedi ever?";
        collab4.username = "Alan";
        dummy3.Add(collab4);

        for (int i = 0; i < testQuestions.Count; i++)
        {
            HTTPClient.QuestionData indQuestion = testQuestions[i];
            GameObject collabListGO = Instantiate(collabAgentPrefab, content.transform);
            collabListGO.tag = "CollabInfoPrefab";

            // Access child components directly
            CollabInfoComponent collabInfoComponent = collabListGO.GetComponent<CollabInfoComponent>();
            collabInfoComponent.viewAnswersPanel = viewAnswersPanel;
            collabInfoComponent.typeAnswerPanel = typeAnswerPanel;
            collabInfoComponent.viewAnswersManager = viewAnswersManager;
            collabInfoComponent.addAnswerManager = addAnswerManager;
            collabInfoComponent.spriteHead = spriteHead.GetComponent<Image>().sprite;
            collabInfoComponent.username = agentName;
            collabInfoComponent.agentID = agentID;
            collabInfoComponent.questionID = indQuestion.id;

            // Set player details dynamically
            collabInfoComponent.SetQuestionDetails(indQuestion.question, count);

            if (i == 0){
            foreach (CollabInfo info in dummy1){
                collabInfoComponent.answers.Add(info.username);
            }
            }
            else if (i == 1){
            foreach (CollabInfo info in dummy2){
                collabInfoComponent.answers.Add(info.username);
            }
            }
            else if (i == 2){
            foreach (CollabInfo info in dummy3){
                collabInfoComponent.answers.Add(info.username);
            }
            }
            else if (i == 3){
                collabInfoComponent.Unanswered();
            }

            collabInfoComponent.SetAnswerDetails();
            count++;
            collabAgentPanel.SetActive(true);
        }

    }

    public async void DisplayCollabQuestions(){
        HTTPClient.AgentData agent = await httpClient.GetAgent(agentID);

        agentName.GetComponent<TextMeshProUGUI>().text = agent.username;

        List<HTTPClient.QuestionData> questions = await httpClient.GetAgentQuestions();
        int count = 1;

        // This loop sets the question text in each prefab
        foreach (HTTPClient.QuestionData questionObj in questions){
            GameObject collabListGO = Instantiate(collabAgentPrefab, content.transform);
            collabListGO.tag = "CollabInfoPrefab";

            CollabInfoComponent collabInfoComponent = collabListGO.GetComponent<CollabInfoComponent>();
            collabInfoComponent.viewAnswersPanel = viewAnswersPanel;
            collabInfoComponent.typeAnswerPanel = typeAnswerPanel;
            collabInfoComponent.viewAnswersManager = viewAnswersManager;
            collabInfoComponent.addAnswerManager = addAnswerManager;
            collabInfoComponent.spriteHead = spriteHead.GetComponent<Image>().sprite;
            collabInfoComponent.username = agentName;
            collabInfoComponent.agentID = agentID;
            collabInfoComponent.questionID = questionObj.id;

            // Set player details dynamically
            collabInfoComponent.SetQuestionDetails(questionObj.question, count);

            List<HTTPClient.QuestionResponseData> responses = await httpClient.GetQuestionResponse(agentID, questionObj.id);
            
            // This loop figures out who has answered each question
            // Create a HashSet to store unique usernames
            HashSet<string> uniqueUsernames = new HashSet<string>();

            foreach (HTTPClient.QuestionResponseData responseObj in responses)
            {
                // Get the user data for the responder
                HTTPClient.UserData user = await httpClient.GetUser(responseObj.responderId);
                
                // Check if the username is not already in the HashSet
                if (!uniqueUsernames.Contains(user.username))
                {
                    // Add the username to the HashSet and the collabInfoComponent.answers list
                    uniqueUsernames.Add(user.username);
                    collabInfoComponent.answers.Add(user.username);
                    if (httpClient.MyId == user.id){
                        collabInfoComponent.Answered();
                    }
                }
            }

            if (collabInfoComponent.answers.Count == 0){
                collabInfoComponent.Unanswered();
            }
            collabInfoComponent.SetAnswerDetails();
            count++;
        }

        // // Call the LoadSprite method with the desired URL
        // spriteLoader.LoadSprite(agent.sprite_headshot_URL, (sprite) => {

        //         spriteHead.GetComponent<Image>().sprite = sprite;
        //     });
        collabAgentPanel.SetActive(true);
    }

    // Deletes instantiations of the prefab that shows up on the collab list when the panel is closed out
    public void CloseCollabListPanel()
    {
        // Iterate through each child of the CollabListPanel
        foreach (Transform child in content.transform)
        {
            // Check if the child has the specified tag
            if (child.gameObject.tag == "CollabInfoPrefab")
            {
                // Destroy the prefab instances
                Destroy(child.gameObject);
            }
        }
    }
}

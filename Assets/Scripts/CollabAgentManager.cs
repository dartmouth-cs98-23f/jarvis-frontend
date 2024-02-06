using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollabAgentManager : MonoBehaviour
{
    public GameObject content;

    [System.Serializable]
    public class CollabInfo
    {
        public string question;
        public string answeredBy;
        public int id;
    }

    public List<CollabInfo> collabList;
    public GameObject collabAgentPrefab;
    public GameObject viewAnswersPanel;
    public GameObject typeAnswerPanel;
    public InputField answerInput;
    public string answer;

    public void localDisplayCollabQuestions()
    {
        List<CollabInfo> dummyCollabList = new List<CollabInfo>();

        // Dummy Question 1
        CollabInfo collab1 = new CollabInfo();
        collab1.question = "How would Obi Wan Kenobi react to a meteor falling from the sky?";
        collab1.id = 1;
        collab1.answeredBy = "Vico, Alan";
        dummyCollabList.Add(collab1);

        // Dummy Question 2
        CollabInfo collab2 = new CollabInfo();
        collab2.question = "Is Obi Wan him?";
        collab2.id = 2;
        collab2.answeredBy = "Vico, Alan";
        dummyCollabList.Add(collab2);

        // Dummy Question 3
        CollabInfo collab3 = new CollabInfo();
        collab3.question = "What color is Obi Wan's lightsaber?";
        collab3.id = 3;
        collab3.answeredBy = "Vico, Alan";
        dummyCollabList.Add(collab3);

        // Dummy Question 4
        CollabInfo collab4 = new CollabInfo();
        collab4.question = "Is Obi Wan the best Jedi ever?";
        collab4.id = 4;
        collab4.answeredBy = "Vico, Alan";
        dummyCollabList.Add(collab4);

        // Dummy Question 5
        CollabInfo collab5 = new CollabInfo();
        collab5.question = "Tell me a time when you and Obi Wan had fun.";
        collab5.id = 5;
        collab5.answeredBy = "Vico, Alan";
        dummyCollabList.Add(collab5);

        // Dummy Question 6
        CollabInfo collab6 = new CollabInfo();
        collab6.question = "Lowkey isn't Yoda cooler though?";
        collab6.id = 6;
        collab6.answeredBy = "Vico, Alan";
        dummyCollabList.Add(collab6);
        

        for (int i = 0; i < dummyCollabList.Count; i++)
        {
            CollabInfo collabInfo = dummyCollabList[i];
            GameObject collabListGO = Instantiate(collabAgentPrefab, content.transform);
            collabListGO.tag = "CollabInfoPrefab";

            // Access child components directly
            CollabInfoComponent collabInfoComponent = collabListGO.GetComponent<CollabInfoComponent>();
            TextMeshProUGUI question = collabInfoComponent.questionTMP; 
            int questionId = collabInfoComponent.questionId; 
            collabInfoComponent.viewAnswersPanel = viewAnswersPanel;
            collabInfoComponent.typeAnswerPanel = typeAnswerPanel;

            // Set player details dynamically
            collabInfoComponent.SetCollabDetails(collabInfo.question, collabInfo.id, collabInfo.answeredBy);
        }
    }

    public void StoreInput()
    {
        answer = answerInput.text;

        // TODO: Send this information to the backend upon clicking the confirm button
    }

    public void ResetInputFields()
    {
        // Reset the input fields
        answerInput.text = "";
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

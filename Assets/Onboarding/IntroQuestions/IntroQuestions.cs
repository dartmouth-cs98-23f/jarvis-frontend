using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;

public class IntroQuestions : MonoBehaviour
{
    
    public GameObject questionPanelsContainer;
    public List<GameObject> allQuestionPanels = new List<GameObject>();
    public GameObject questionPanelPrefab;
    public GameObject characterSummaryPanel;
    public List<Text> errorTexts = new List<Text>();
    public List<InputField> questionInputFields = new List<InputField>();
    public List<string> answers = new List<string>();
    private HTTPClient httpClient = HTTPClient.Instance;
    private int questionIdx = 0; // index of the current question. Note: 0th-index refers to design character question panel


    void Start()
    {
        GenerateQuestionPanels();
    }

    async void GenerateQuestionPanels()
    {
        // TODO: Comment to connect to backend
        List<HTTPClient.UserQuestion> userQuestions = await LocalGetUserQuestions();
        // List<HTTPClient.UserQuestion> userQuestions = await GetUserQuestions();

        foreach (HTTPClient.UserQuestion question in userQuestions)
        {
            Transform questionPanelsContainerTransform = questionPanelsContainer.transform; 
            GameObject newQuestionPanel = Instantiate(questionPanelPrefab, questionPanelsContainerTransform);
            newQuestionPanel.SetActive(false);
            allQuestionPanels.Add(newQuestionPanel);
            
            // Find the InputField in the instantiated panel
            InputField inputField = newQuestionPanel.GetComponentInChildren<InputField>();
            if (inputField != null)
            {
                questionInputFields.Add(inputField);
            }

            // Find and collect error Text components
            Text[] texts = newQuestionPanel.GetComponentsInChildren<Text>(true); // 'true' to include inactive children
            foreach (Text txt in texts)
            {
                if (txt.gameObject.name == "ErrorText")
                {
                    errorTexts.Add(txt);
                }
            }
        }

        // Use FindDeepChild to find the NextButton in the last panel
        // Set the last panel's next button text to Finish
        // show first question panel
        if (allQuestionPanels.Count > 0)
        {
            GameObject lastPanel = allQuestionPanels[allQuestionPanels.Count - 1];
            Transform nextButtonTransform = FindDeepChildHelper(lastPanel.transform, "NextButton");
            if (nextButtonTransform != null)
            {
                Button nextButton = nextButtonTransform.GetComponent<Button>();
                if (nextButton != null)
                {
                    Text buttonText = nextButton.GetComponentInChildren<Text>();
                    if (buttonText != null) buttonText.text = "Finish";
                }
            }
            allQuestionPanels[0].SetActive(true);
        }
    }

    private async Task<List<HTTPClient.UserQuestion>> GetUserQuestions()
    {
        return await httpClient.GetUserQuestions();
    }

    private async Task<List<HTTPClient.UserQuestion>> LocalGetUserQuestions()
    {
        await Task.Delay(1000);
        return new List<HTTPClient.UserQuestion>
        {
            new HTTPClient.UserQuestion
            {
                id = new System.Guid(),
                question = "How old are you?"
            },
            new HTTPClient.UserQuestion
            {
                id = new System.Guid(),
                question = "What is your occupation?"
            },
            new HTTPClient.UserQuestion
            {
                id = new System.Guid(),
                question = "How do you like to waste your time?"
            },
            new HTTPClient.UserQuestion
            {
                id = new System.Guid(),
                question = "How would you describe your communication style? Funny? Serious? Formal? Do you speak in riddles or poems?"
            },
        };
    }
    public void OnBackPressed()
    {
        if (questionIdx > 0)
        {
            allQuestionPanels[questionIdx].SetActive(false);
            questionIdx--;
            allQuestionPanels[questionIdx].SetActive(true);
        }
    }
    
    public async void OnNextPressed()
    {
        string currentAnswer = questionInputFields[questionIdx].text;
    
        if (string.IsNullOrEmpty(currentAnswer))
        {
            errorTexts[questionIdx].text = "Please field in the blank.";
        }
        else if (questionIdx >= questionInputFields.Count-1)
        {
            for (int i = 0; i < questionInputFields.Count; i++)
            {
                answers.Add(questionInputFields[i].text);
            }
            
            // Reset the question count for future use if needed
            questionIdx = 0;

            // TODO: Comment this out when backend is ready
            bool postAnswerSuccessful = true;
            // bool postAnswerSuccessful = await httpClient.PostResponses(answers);

            if (postAnswerSuccessful)
            {
                Debug.Log("Successfully posted answers to backend");
                NavigateToCharacterSummaryPanel();
            }
            else
            {
                Debug.Log("Failed to post responses due to backend error");
            }
        } 
        else
        {
            errorTexts[questionIdx].text = ""; // clear current error message
            NavigateToNextPanel();
        }
    }

    void NavigateToNextPanel()
    {
        allQuestionPanels[questionIdx].SetActive(false);
        questionIdx++;
        allQuestionPanels[questionIdx].SetActive(true);
    }

    void NavigateToCharacterSummaryPanel()
    {
        // hide both the question panel container and the last question
        questionPanelsContainer.SetActive(false);
        allQuestionPanels[questionIdx].SetActive(false);
        characterSummaryPanel.SetActive(true);
    }

    private Transform FindDeepChildHelper(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;
            
            Transform found = FindDeepChildHelper(child, childName);
            if (found != null)
                return found;
        }
        return null; // Not found
    }


}

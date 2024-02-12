using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;
using System;
using System.Threading.Tasks;

public class IntroQuestions : MonoBehaviour
{
    
    public GameObject questionPanelsContainer;
    public List<GameObject> allQuestionPanels = new List<GameObject>();
    private List<HTTPClient.UserQuestion> userQuestions = new List<HTTPClient.UserQuestion>();
    public GameObject questionPanelPrefab;
    public GameObject characterSummaryPanel;
    public List<Text> errorTexts = new List<Text>();
    public List<InputField> questionInputFields = new List<InputField>();
    public List<HTTPClient.PostResponse> answers = new List<HTTPClient.PostResponse>();
    public GameObject loadingPanel;
    private HTTPClient httpClient = HTTPClient.Instance;
    private int questionIdx = 0; // index of the current question. Note: 0th-index refers to design character question panel

    void Start()
    {
        GenerateQuestionPanels();
    }

    async void GenerateQuestionPanels()
    {
        // TODO: Comment to connect to backend
        userQuestions = await LocalGetUserQuestions();
        // userQuestions = await GetUserQuestions();

        foreach (HTTPClient.UserQuestion question in userQuestions)
        {
            Transform questionPanelsContainerTransform = questionPanelsContainer.transform; 
            GameObject newQuestionPanel = Instantiate(questionPanelPrefab, questionPanelsContainerTransform);
            newQuestionPanel.SetActive(false);
            allQuestionPanels.Add(newQuestionPanel);

            // Set the question text
            Transform questionTextTransform = FindDeepChildHelper(newQuestionPanel.transform, "Question");
            questionTextTransform.GetComponent<Text>().text = question.question;
            
            // Find and store pointers to the InputField in the instantiated panel
            InputField inputField = newQuestionPanel.GetComponentInChildren<InputField>();
            if (inputField != null)
            {
                questionInputFields.Add(inputField);
            }

            // Find and store pointers to the error Text components
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

            InitializeButtonListeners();
        }


    }

    void InitializeButtonListeners()
    {
        foreach (GameObject panel in allQuestionPanels)
        {
            // set next button listeners
            Transform nextButtonTransform = FindDeepChildHelper(panel.transform, "NextButton");
            if (nextButtonTransform != null)
            {
                Button nextButton = nextButtonTransform.GetComponent<Button>();
                nextButton.onClick.AddListener(OnNextPressed);
            }

            // set back button listeners
            Transform backButtonTransform = FindDeepChildHelper(panel.transform, "BackButton");
            if (backButtonTransform != null)
            {
                Button backButton = backButtonTransform.GetComponent<Button>();
                backButton.onClick.AddListener(OnBackPressed);
            }
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
                question = "How would you describe your communication style?"
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
            Debug.Log("Setting error text: " + questionIdx);
            errorTexts[questionIdx].text = "Please field in the blank.";
        }
        else if (questionIdx >= questionInputFields.Count-1) // if is last question
        {
            StartLoading();
            bool saveSuccessful = await SaveAnswers(); // save answers to backend
            if (saveSuccessful) {
                // Reset the question count for future use if needed
                questionIdx = 0;
                try {
                    HTTPClient.UserData userData = await httpClient.GetUser(httpClient.MyId);
                    // GetUser sprite to display
                    string userSummary = await httpClient.GetUserSummary(httpClient.MyId);
                    if (userData!= null && userSummary != null && userData.sprite_URL != null && userData.sprite_URL != "" && userSummary != "")
                    {
                        FinishLoading(userData.username, userData.sprite_URL, userSummary);
                    } else {
                        // TODO: Add UI Error message for users
                        Debug.Log("Failed to get user data or user summary due to backend error");
                    }
                } catch (Exception e) {
                    Debug.Log("Failed to get user data or user summary: " + e.Message); // TODO: Add UI error message on failure to create world
                }
            } else {
                Debug.Log("Failed to save answers due to backend error");
            }

        } 
        else
        {
            errorTexts[questionIdx].text = ""; // clear current error message
            NavigateToNextPanel();
        }
    }

    async void StartLoading()
    {
        loadingPanel.SetActive(true);

        // hide both the question panel container and the last question
        questionPanelsContainer.SetActive(false);
        allQuestionPanels[questionIdx].SetActive(false);
    }

    async void FinishLoading(string username, string sprite_URL, string userSummary)
    {
        loadingPanel.SetActive(false);
        characterSummaryPanel.GetComponent<CharacterSummaryManager>().SetCharacterSummary(username, sprite_URL, userSummary);
        NavigateToCharacterSummaryPanel();
    }

    async Task<bool> SaveAnswers()
    {
        for (int i = 0; i < questionInputFields.Count; i++)
        {
            answers.Add(new HTTPClient.PostResponse {
                questionId = userQuestions[i].id,
                response = questionInputFields[i].text
            });
        }

        // TODO: Comment this out when backend is ready. Check if PostAnswer returns true
        bool postAnswerSuccessful = await LocalPostResponses();
        // bool postAnswerSuccessful = httpClient.PostResponses(httpClient.MyId, httpClient.MyId, answers);

        if (postAnswerSuccessful)
        {
            return true;
        }
        else
        {
            Debug.Log("Failed to post responses due to backend error");
            return false;
        }
    }

    async Task<bool> LocalPostResponses()
    {
        await Task.Delay(1000);
        Debug.Log("Successfully posted responses: " + answers);
        return true;
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

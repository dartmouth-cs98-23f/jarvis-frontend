using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class IntroQuestions : MonoBehaviour
{
    public GameObject[] questionPanels;
    public GameObject questionPanel;
    public GameObject characterSummaryPanel;

    public List<Text> errorTexts = new List<Text>();

    public List<InputField> questionInputFields = new List<InputField>();
    private int questionIdx = 0; // index of the current question. Note: 0th-index refers to design character question panel
    public List<string> answers = new List<string>();

    public void OnBackPressed()
    {
        if (questionIdx > 0)
        {
            questionPanels[questionIdx].SetActive(false);
            questionIdx--;
            questionPanels[questionIdx].SetActive(true);
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
            // HTTPClient httpClient = HTTPClient.Instance;
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
        questionPanels[questionIdx].SetActive(false);
        questionIdx++;
        questionPanels[questionIdx].SetActive(true);
    }

    void NavigateToCharacterSummaryPanel()
    {
        // hide both the question panel container and the last question
        questionPanel.SetActive(false);
        questionPanels[questionIdx].SetActive(false);
        characterSummaryPanel.SetActive(true);
    }

}

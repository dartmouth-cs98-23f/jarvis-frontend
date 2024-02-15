using System;
using System.Collections;
using System.Collections.Generic;
using Clients;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;


public class UserProfileManager : MonoBehaviour
{
    public Text username;
    public Image userImage;
    public Text userSummary;

    public InputField userSummaryInputField;
    public GameObject inputFieldContainer;
    public GameObject summaryContainer;
    public GameObject editButton;
    public GameObject saveButton;
    private HTTPClient httpClient = HTTPClient.Instance;

    // Start is called before the first frame update
    void OnEnable()
    {
        InitializeUserProfile();
    }

    private async Task InitializeText()
    {
        await Task.Delay(1000);
        // TODO: Replace this text with user's actual summary by getting it from the backend
        userSummary.text = "Silly silly user summary is silly";
        // userSummary.text = await httpClient.GetUserSummary(httpClient.MyId);

        userSummaryInputField.text = userSummary.text;
        userSummaryInputField.placeholder.GetComponent<Text>().text = userSummary.text;
    }

    public async Task InitializeUserProfile()
    {

        // TODO: Switch this method for backend API
        HTTPClient.UserData userData = await LocalGetUser(httpClient.MyId);
        // HTTPClient.UserData userData = await httpClient.GetUser(httpClient.MyId);
        if (userData != null)
        {
            Debug.Log("Initializing user profile with username: " + username + " and sprite_URL: " + userData.sprite_URL);
            this.username.text = "@" + userData.username;
            StartCoroutine(LoadCharacterSprite(userData.sprite_URL));
            InitializeText();
        }
    }
    private async Task<HTTPClient.UserData> LocalGetUser(Guid userId)
    {
        // Simulate a delay to mimic network request time
        await Task.Delay(2000);

        return new HTTPClient.UserData
        {
            id = new Guid(),
            username = "Current User",
            email = "currentuser@example.com",
            location = new HTTPClient.Location { coordX = 10, coordY = 20 },
            createdTime = DateTime.Parse("2024-01-01T00:01:00Z"),
            isOnline = true,
            sprite_URL = "https://ibb.co/XZYT5xg",
            sprite_headshot_URL = "https://ibb.co/XZYT5xg"
        };
    }
    IEnumerator LoadCharacterSprite(string sprite_URL)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(sprite_URL);
        yield return uwr.SendWebRequest(); // Wait for the download to complete

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            userImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to download image: " + uwr.error);
        }

        uwr.Dispose(); // Manually dispose of the UnityWebRequest
    }


    public async void OnPressSave()
    {
        // TODO: Switch this method for backend API
        bool updateSummarySuccessful = true;
        // bool updateSummarySuccessful = await httpClient.UpdateUserSummary(httpClient.MyId, userSummaryInputField.text);
        if (updateSummarySuccessful)
        {
            editButton.SetActive(true);
            saveButton.SetActive(false);
            summaryContainer.SetActive(true);
            inputFieldContainer.SetActive(false);
            userSummary.text = userSummaryInputField.text;
        }
        else
        {
            // TODO: Add UI error message on failure to update summary
            Debug.Log("Failed to update summary. Please try again.");
        }
    }

    public void OnPressEdit()
    {
        editButton.SetActive(false);
        saveButton.SetActive(true);
        summaryContainer.SetActive(false);
        inputFieldContainer.SetActive(true);
    }
}
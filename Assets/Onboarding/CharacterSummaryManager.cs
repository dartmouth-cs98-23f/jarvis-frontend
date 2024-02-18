using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CharacterSummaryManager : MonoBehaviour
{
    public Text username;
    public Image userImage;
    public Text userSummary;


    // Use OnEnable so that it will be called before SetCharacterSummary
    void OnEnable()
    {
        // TODO: Connect to backend to get user data
        username.GetComponent<Text>().text = "@" + "testUsername";
        userSummary.GetComponent<Text>().text = "You are a compassionate, wise, and brave Jedi Master. You're known for your strategic mind, moral integrity, mentorship skills, patience, and a subtle sense of humor. You are a compassionate, wise, and brave Jedi Master. You're known for your strategic mind, moral integrity, mentorship skills, patience, and a subtle sense of humor.";
        userImage.sprite = Resources.Load<Sprite>("Shapes/obi_wan_kenobi");
        Debug.Log("CharacterSummaryManager OnEnable");
    }

    public void SetCharacterSummary(string username, string sprite_URL, string userSummary)
    {
        Debug.Log("setting character summary");
        this.username.GetComponent<Text>().text = "@" + username;
        this.userSummary.GetComponent<Text>().text = userSummary;
        if (!string.IsNullOrEmpty(sprite_URL) || Uri.IsWellFormedUriString(sprite_URL, UriKind.Absolute))
        {
            Debug.Log("loading character sprite");
            StartCoroutine(LoadCharacterSprite(sprite_URL));
        }
    }

    IEnumerator LoadCharacterSprite(string sprite_URL)
    {
        // Basic validation of the URL
        if (string.IsNullOrEmpty(sprite_URL) || !Uri.IsWellFormedUriString(sprite_URL, UriKind.Absolute))
        {
            Debug.LogError($"Invalid or malformed URL: {sprite_URL}");
            yield break; // Exit the coroutine early
        }

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
}

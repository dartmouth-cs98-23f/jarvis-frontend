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
    public GameObject userPrefabObject;


    // Use OnEnable so that it will be called before SetCharacterSummary
    void OnEnable()
    {
        // TODO: Connect to backend to get user data
        username.GetComponent<Text>().text = "@" + "testUsername";
        userSummary.GetComponent<Text>().text = "You are a compassionate, wise, and brave Jedi Master. You're known for your strategic mind, moral integrity, mentorship skills, patience, and a subtle sense of humor. You are a compassionate, wise, and brave Jedi Master. You're known for your strategic mind, moral integrity, mentorship skills, patience, and a subtle sense of humor.";
        userPrefabObject.GetComponent<BodyPartsManager>().SetSprite(new List<int> { 0, 0, 0, 0 });
    }

    public void SetCharacterSummary(string username, List<int> spriteAnimations, string userSummary)
    {
        Debug.Log("setting character summary");
        this.username.GetComponent<Text>().text = "@" + username;
        this.userSummary.GetComponent<Text>().text = userSummary;
        userPrefabObject.GetComponent<BodyPartsManager>().SetSprite(spriteAnimations);
    }

    // @Deprecated. This was used for when backend stored user's sprite as sprite_URL. Now it's been changed to spriteAnimations
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

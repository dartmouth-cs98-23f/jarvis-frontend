using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GeneratedWorldManager : MonoBehaviour
{
    public GameObject generatedWorldPanel;
    public GameObject myWorldsPanel;
    public GameObject navbarPanel;
    public Text worldNameText;
    public Image displayThumbnail;
    public void OnPressPlay()
    {
        // TODO: Navigate to game map, remove the bottom where it navigates to MyWorlds. Its 
        // for local testing only
        generatedWorldPanel.SetActive(false);
        myWorldsPanel.SetActive(true);
        navbarPanel.GetComponent<NavbarManager>().NavigateBackToMyWorlds();
    }

    public void SetGeneratedWorld(string worldName, string thumbnail_URL)
    {
        worldNameText.text = worldName;
        StartCoroutine(LoadWorldSprite(thumbnail_URL));
    }

    IEnumerator LoadWorldSprite(string thumbnail_URL)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(thumbnail_URL);
        yield return uwr.SendWebRequest(); // Wait for the download to complete

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            Sprite worldThumbnailSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            displayThumbnail.sprite = worldThumbnailSprite;
        }
        else
        {
            Debug.LogError("Failed to download image: " + uwr.error);
        }

        uwr.Dispose(); // Manually dispose of the UnityWebRequest
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Clients;

public class GeneratedWorldManager : MonoBehaviour
{
    public GameObject generatedWorldPanel;
    public GameObject myWorldsPanel;
    public GameObject navbarPanel;
    public Text worldNameText;
    public Image displayThumbnail;
    private Guid worldId;
    private HTTPClient httpClient = HTTPClient.Instance;

    public void OnPressPlay()
    {
        generatedWorldPanel.SetActive(false);
        myWorldsPanel.SetActive(true);
        navbarPanel.GetComponent<NavbarManager>().NavigateBackToMyWorlds();
        
        Debug.Log("Navigating to game map, current world id: " + worldId.ToString());
        httpClient.CurrentWorldId = worldId;
        SceneNavigator.LoadGame();
    }

    public void SetGeneratedWorld(Guid worldId, string worldName, string thumbnail_URL)
    {
        this.worldId = worldId;
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

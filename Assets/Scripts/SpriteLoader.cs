using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;

public class SpriteLoader : MonoBehaviour
{
    public Sprite sprite;

    public void LoadSprite(string url, Action<Sprite> onSpriteLoaded)
    {
        StartCoroutine(LoadSpriteCoroutine(url, onSpriteLoaded));
    }

    IEnumerator LoadSpriteCoroutine(string url, Action<Sprite> onSpriteLoaded)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite createdSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            // Assign the sprite to the sprite component
            sprite = createdSprite;
            onSpriteLoaded(sprite);
        }
        else
        {
            Debug.Log("Failed to load image: " + www.error);
        }
    }
}

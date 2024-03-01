using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ChatMessageComponent : MonoBehaviour
{
    // public TextMeshProUGUI senderNameGO;
    public TextMeshProUGUI displayContentGO;
    public Text displayUsername;
    public Image userOnlineStatus;

    // A method to set the chat details
    public void SetChatDetails(string username, string content, bool isOnline)
    {
        Debug.Log("In ChatMessageComponent: " + username + " " + content + " " + isOnline);
        displayUsername.text = username;
        displayContentGO.text = content;
        if (isOnline)
        {
            userOnlineStatus.sprite = Resources.Load<Sprite>("Shapes/green_circle");
        } else
        {
            userOnlineStatus.sprite = Resources.Load<Sprite>("Shapes/hollow_circle");
        }
    }
}

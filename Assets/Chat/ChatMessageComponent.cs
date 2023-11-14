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
    public Image displayUserImage;

    // A method to set the chat details
    public void SetChatDetails(Sprite profileImage, string content)
    {
        displayUserImage.sprite = profileImage;
        displayContentGO.text = content; // TODO: get sender name from backend instead of Guid
    }
}

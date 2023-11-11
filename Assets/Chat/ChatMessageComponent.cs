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

    // A method to set the chat details
    public void SetChatDetails(Guid senderId, string content)
    {
        displayContentGO.text = senderId.ToString() + ": " + content; // TODO: get sender name from backend instead of Guid
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChatMessage
{
    public string senderId;
    public string receiverId;
    public string content;
    public bool isGroupChat;
    public DateTime createdTime; // TODO: Check backend format for createdTime
}

public class ChatManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string otherUserID;
    public InputField messageInputField;
    public string chatTestJsonString = @"
    {
        ""1"": {
            ""senderId"": ""someSenderId"",
            ""receiverId"": ""someReceiverId"",
            ""content"": ""someContent"",
            ""isGroupChat"": false,
            ""createdTime"": ""someCreatedTime""
        },
        ""2"": {
            ""senderId"": ""someOtherSenderId"",
            ""receiverId"": ""someOtherReceiverId"",
            ""content"": ""someOtherContent"",
            ""isGroupChat"": false,
            ""createdTime"": ""someOtherCreatedTime""
        }
    }";

    void Start()
    {
        initializeChatHistory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // send message to backend
    void onSendMessage() // TODO: fjaeiowf
    {
        ChatMessage message =  new ChatMessage();
        message.senderId = "placeholderId";
        message.receiverId = otherUserID;
        message.content = text;
        message.isGroupChat = false;
        message.createdTime = DateTime.Now; // TODO: check if this is auto-generated on backend
        SendMessage(otherUserID, messageInputField.text);
    }

    void SendMessage(string otherUserID, string text)
    {

    }


    void initializeChatHistory()
    {
        string chatJsonString = GetChatHistory(otherUserID);

    }

    // TODO: replace this with the actual backend call
    void GetChatHistory(string otherUserID)
    {
        return chatTestJsonString;
    }

}

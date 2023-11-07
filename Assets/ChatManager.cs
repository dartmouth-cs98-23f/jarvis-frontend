using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Chat
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
    void Start()
    {
        initializeChatHistory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initializeChatHistory()
    {
        string chatJsonString = GetChatHistory(otherUserID);

    }

}

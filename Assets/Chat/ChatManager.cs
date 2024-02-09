using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Clients;
using System.Threading.Tasks;

[Serializable]

public static class JsonHelper
{
    public static List<HTTPClient.ChatMessage> ConvertJsonToChatList(string json)
    {
        List<HTTPClient.ChatMessage> chatList = JsonConvert.DeserializeObject<List<HTTPClient.ChatMessage>>(json);
        chatList.Sort((x, y) => DateTime.Compare(x.CreatedTime, y.CreatedTime));           // Sort the list based on the createdTime
        return chatList;
    }

}

public static class StringParser
{
    public static string ParseInput(string input)
    {
        Debug.Log("Input pre parse" + input);
        string cleaned = input.Replace("\\n", "\n").Replace("\"", "").Replace("\\", "");
        Debug.Log("Input post parse" + cleaned);
        return cleaned;
    }
}

namespace ChatManager{
public class ChatManager : MonoBehaviour
{
    private HTTPClient httpClient = HTTPClient.Instance;
    public Guid currentUserId = HTTPClient.Instance.MyId;
    private Guid otherUserID; 
    private Guid yodaID = new Guid("f7dd290b-faab-4c15-b8b9-38cff0895559");
    private Guid georgeWashID = new Guid("55cd50d5-7775-4dd2-b632-a502a031ac41");
    public InputField messageInputField;
    public Transform contentPanel;
    public GameObject chatMessagePrefab;
    public Text cardUsername;
    public Image cardProfileImage;
    HashSet<Guid> generatedMessageIds = new HashSet<Guid>();

    private Sprite userImage; // TODO: replace with actual image of user once backend api can handle it
    private Sprite otherUserImage; // TODO: replace with actual image of other user once backend api can handle it

    private string chatTestJsonString = @"
    [
        {
            ""id"": ""44dcb24a-3fb6-4533-be6f-7c56eb259c89"",
            ""senderId"": ""f7dd290b-faab-4c15-b8b9-38cff0895559"",
            ""receiverId"": ""55cd50d5-7775-4dd2-b632-a502a031ac41"",
            ""content"": ""This is old text"",
            ""isGroupChat"": false,
            ""createdTime"": ""2022-05-21T08:20:00Z""
        },
        {
            ""id"": ""21526023-eb1b-4b1a-be79-beb0d714dc45"",
            ""senderId"": ""55cd50d5-7775-4dd2-b632-a502a031ac41"",
            ""receiverId"": ""f7dd290b-faab-4c15-b8b9-38cff0895559"",
            ""content"": ""This is the newest text"",
            ""isGroupChat"": false,
            ""createdTime"": ""2023-11-07T14:45:30Z""
        },
        {
            ""id"": ""e23ae41c-b1da-4679-982f-2e38154b217b"",
            ""senderId"": ""55cd50d5-7775-4dd2-b632-a502a031ac41"",
            ""receiverId"": ""f7dd290b-faab-4c15-b8b9-38cff0895559"",
            ""content"": ""This is new textThis is new text\nThis is new textThis is new text\nThis is new text\nThis is new textThis is new textThis is new textThis is new textThis is new textThis is new textThis is new text"",
            ""isGroupChat"": false,
            ""createdTime"": ""2023-11-07T13:45:30Z""
        },
    ]";

    public List<HTTPClient.ChatMessage> sortedChatMessages;

    void Start()
    {
        // TODO: replace with loading the actual images of the users once backend api can handle
        otherUserID = new Guid(PlayerPrefs.GetString("CollidedUserId"));

        // Initialize user images
        userImage = GetUserImage(currentUserId);
        otherUserImage = GetUserImage(otherUserID);

        Debug.Log("In chat manager, otherUserID: " + otherUserID.ToString());
        SignalRClient.Instance.RegisterSendMessageHandler(this);
        BuildOtherUserProfile();
        BuildChatHistory();

    }
    
    // Update is called once per frame
    void Update()
    {
        // Check if there is a received message to process
        if (!string.IsNullOrEmpty(SignalRClient.SenderId))
        {
            Debug.Log("Got message, going to generate GCMO");
            // Create a ChatMessage object and reset the static variables
            GenerateChatMessageObject(new HTTPClient.ChatMessage
            {
                SenderId = new Guid(SignalRClient.SenderId),
                ReceiverId = currentUserId,
                Content = StringParser.ParseInput(SignalRClient.Message),
                IsGroupChat = false,
                CreatedTime = DateTime.UtcNow
            });


            // Reset the static variables to indicate that the message has been processed
            SignalRClient.SenderId = null;
            SignalRClient.Message = null;
        }
    }



    Sprite GetUserImage(Guid userId)
    {
        if (userId.ToString() == yodaID.ToString()) {
            return Resources.Load<Sprite>("Shapes/master_yoda_head");
        } else if (userId.ToString() == georgeWashID.ToString())
        {
            return Resources.Load<Sprite>("Shapes/george_washington_head");
        } else if (userId.ToString() == currentUserId.ToString()) {
            return Resources.Load<Sprite>("Shapes/user_head");
        }
        else {
            return Resources.Load<Sprite>("Shapes/npc_head");
        }
    }

    public async void BuildOtherUserProfile()
    {

        HTTPClient.UserData otherUser = await httpClient.GetUser(otherUserID);
        if (otherUser == null){
            return;
        }
        cardUsername.GetComponent<Text>().text = otherUser.username;
        cardProfileImage.sprite = otherUserImage;
    }

    // TODO: This is a temporary method for frontend testing
    // A method to add a new chat entry
    public void AddNewChatEntry(Guid id, HTTPClient.ChatMessage message)
    {
        // Parse the existing JSON string into a JObject
        JArray chatsArray = JArray.Parse(chatTestJsonString);

        // Create a new chat entry as a JObject
        JObject newChat = new JObject
        {
            ["id"] = id.ToString(),
            ["senderId"] = message.SenderId.ToString(),
            ["receiverId"] = message.ReceiverId.ToString(),
            ["content"] = message.Content,
            ["isGroupChat"] = message.IsGroupChat,
            ["createdTime"] = message.CreatedTime.ToString("o")  // "o" for round-trip date/time pattern
        };

        // Add the new chat entry to the chats object
        chatsArray.Add(newChat);

        // Serialize the JObject back to a string
        chatTestJsonString = chatsArray.ToString(Formatting.Indented); // If you want it pretty-printed
    }

    // send message to backend
    public void onClickSendMessage() //add to unity button
    {

        // TODO: Switch for backend API
        // LocalSendChat(otherUserID, messageInputField.text);
        SendChat(otherUserID, messageInputField.text);
        // Debug.Log("after adding new chat entry" + chatTestJsonString);
        messageInputField.text = "";
    }

    async void BuildChatHistory()
    {
        sortedChatMessages = await httpClient.GetChatHistory(currentUserId, otherUserID);
         if (sortedChatMessages == null) 
        {
            return;
        }
        sortedChatMessages.Sort((x, y) => DateTime.Compare(x.CreatedTime, y.CreatedTime)); 
        
        // Sort the list based on the createdTime
        foreach (HTTPClient.ChatMessage chatMessage in sortedChatMessages)
        {
            chatMessage.Content = StringParser.ParseInput(chatMessage.Content);

            if (generatedMessageIds.Add(chatMessage.Id)) // if chatMessage has not been created yet
            {
                GenerateChatMessageObject(chatMessage);
            }
        }

    }

    // For local frontend testing
    void LocalBuildChatHistory()
    {
        sortedChatMessages = JsonHelper.ConvertJsonToChatList(LocalGetChatHistory(otherUserID));
        foreach (HTTPClient.ChatMessage chatMessage in sortedChatMessages)
        {
            if (generatedMessageIds.Add(chatMessage.Id)) // if chatMessage has not been created yet
            {
                GenerateChatMessageObject(chatMessage);
            }
        }
    }

    public void GenerateChatMessageObject(HTTPClient.ChatMessage chatMessage)
    {
        // Debug.Log("Calling GCMO");
        GameObject chatGO = Instantiate(chatMessagePrefab, contentPanel);
        ChatMessageComponent chatMessageComponent = chatGO.GetComponent<ChatMessageComponent>();
        string messageContent = StringParser.ParseInput(chatMessage.Content);
        if (chatMessage.SenderId == currentUserId)
        {
            chatMessageComponent.SetChatDetails(userImage, messageContent);
        }
        else
        {
            chatMessageComponent.SetChatDetails(otherUserImage, messageContent);
        }
    }

    async void SendChat(Guid receiverId, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        
        HTTPClient.ChatMessage message = new HTTPClient.ChatMessage();
        message.SenderId = currentUserId;
        message.ReceiverId = receiverId;
        message.Content = content;
        message.IsGroupChat = false;
        message.CreatedTime = DateTime.UtcNow; // TODO: check if this is auto-generated on backend
        // AddNewChatEntry(Guid.NewGuid(), message); 

        await SignalRClient.Instance.SendChat(otherUserID, content);

        GenerateChatMessageObject(message);
    }

    // For local frontend testing
    void LocalSendChat(Guid receiverId, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        
        // Uncomment this for local testing
        HTTPClient.ChatMessage message =  new HTTPClient.ChatMessage();
        message.SenderId = currentUserId;
        message.ReceiverId = receiverId;
        message.Content = content;
        message.IsGroupChat = false;
        message.CreatedTime = DateTime.UtcNow; // TODO: check if this is auto-generated on backend
        AddNewChatEntry(Guid.NewGuid(), message);
    }


    // For local frontend testing
    string LocalGetChatHistory(Guid otherUserID)
    {
        // return chatTestJsonString;
        return chatTestJsonString; // replace this line with above for local testing
    }
}
}
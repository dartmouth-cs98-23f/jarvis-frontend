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
public class ChatMessage
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("senderId")]
    public Guid SenderId { get; set; }

    [JsonProperty("receiverId")]
    public Guid ReceiverId { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("isGroupChat")]
    public bool IsGroupChat { get; set; }

    [JsonProperty("createdTime")]
    public DateTime CreatedTime { get; set; }
}

public static class JsonHelper
{
    // public static List<ChatMessage> ConvertJsonToChatList(string json)
    // {
    //     // format date time for deserialization
    //     // TODO: replace "yyyy-MM-ddTHH:mm:ssZ" with the actual date-time format used in JSON string
    //     IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ" };

    //     // Deserialize the JSON into a dictionary
    //     var dict = JsonConvert.DeserializeObject<Dictionary<string, ChatMessage>>(json, dateTimeConverter);

    //     // Iterate over the dictionary to set the id property of each ChatMessage
    //     foreach (var entry in dict)
    //     {
    //         entry.Value.id = entry.Key; // Set the id of ChatMessage to the key from the dictionary
    //     }

    //     // Create a list from the dictionary values
    //     List<ChatMessage> chatList = new List<ChatMessage>(dict.Values);

    //     // Sort the list based on the createdTime
    //     chatList.Sort((x, y) => DateTime.Compare(x.createdTime, y.createdTime));

    //     return chatList;
    // }

    public static List<ChatMessage> ConvertJsonToChatList(string json)
    {
        List<ChatMessage> chatList = JsonConvert.DeserializeObject<List<ChatMessage>>(json);
        chatList.Sort((x, y) => DateTime.Compare(x.CreatedTime, y.CreatedTime));           // Sort the list based on the createdTime
        return chatList;
    }

}

public class ChatManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Guid currentUserId = new Guid("cf11a140-99b3-4e07-9108-249f2b66533d"); // TODO: This is a temporary testing value. Update value when changing from previous scene
    private Guid otherUserID = new Guid("c75ed4cb-2406-41ec-b3d7-2b2cf0806c52");  // TODO: This is a temporary testing value. Update value when changing from previous scene
    // private HTTPClient httpClient = HTTPClient.Instance;
    public InputField messageInputField;
    public Transform contentPanel;
    public GameObject chatMessagePrefab;
    public GameObject otherUserName;
    HashSet<Guid> generatedMessageIds = new HashSet<Guid>();

    private string chatTestJsonString = @"
    [
        {
            ""id"": ""44dcb24a-3fb6-4533-be6f-7c56eb259c89"",
            ""senderId"": ""cf11a140-99b3-4e07-9108-249f2b66533d"",
            ""receiverId"": ""c75ed4cb-2406-41ec-b3d7-2b2cf0806c52"",
            ""content"": ""This is old text"",
            ""isGroupChat"": false,
            ""createdTime"": ""2022-05-21T08:20:00Z""
        },
        {
            ""id"": ""21526023-eb1b-4b1a-be79-beb0d714dc45"",
            ""senderId"": ""cf11a140-99b3-4e07-9108-249f2b66533d"",
            ""receiverId"": ""c75ed4cb-2406-41ec-b3d7-2b2cf0806c52"",
            ""content"": ""This is the newest text"",
            ""isGroupChat"": false,
            ""createdTime"": ""2023-11-07T14:45:30Z""
        },
        {
            ""id"": ""e23ae41c-b1da-4679-982f-2e38154b217b"",
            ""senderId"": ""c75ed4cb-2406-41ec-b3d7-2b2cf0806c52"",
            ""receiverId"": ""cf11a140-99b3-4e07-9108-249f2b66533d"",
            ""content"": ""This is new text"",
            ""isGroupChat"": false,
            ""createdTime"": ""2023-11-07T13:45:30Z""
        },
    ]";

    public List<ChatMessage> sortedChatMessages;

    void Start()
    {
        // TODO: Get current user information and the other user information
        BuildOtherUserProfile();
    }

    // Update is called once per frame
    void Update()
    {
        BuildChatHistory();
    }

    public async void BuildOtherUserProfile()
    {
        // TODO: Replace code with actual code to get user information
        // HTTPClient.UserData otherUser = await httpClient.GetUser(otherUserID);
        HTTPClient.UserData otherUser = new HTTPClient.UserData();
        otherUser.firstName = "John";
        otherUser.lastName = "Doe";
        if (otherUser == null){
            return;
        }
        otherUserName.GetComponent<Text>().text = otherUser.firstName + " " + otherUser.lastName;
    }

    // TODO: This is a temporary method for frontend testing
    // A method to add a new chat entry
    public void AddNewChatEntry(Guid id, ChatMessage message)
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
        SendMessage(otherUserID, messageInputField.text);
        Debug.Log("after adding new chat entry" + chatTestJsonString);
        messageInputField.text = "";
    }

    void BuildChatHistory()
    {
        string chatJsonString = GetChatHistory(otherUserID);
        sortedChatMessages = JsonHelper.ConvertJsonToChatList(chatJsonString);
        foreach (ChatMessage chatMessage in sortedChatMessages)
        {
            if (generatedMessageIds.Add(chatMessage.Id)) // if chatMessage has not been created yet
            {
                GenerateChatMessageObject(chatMessage);

            }
        }

        // Debug.Log("BuildChatHistory called: " + sortedChatMessages);
    }

    void GenerateChatMessageObject(ChatMessage chatMessage)
    {
        GameObject chatGO = Instantiate(chatMessagePrefab, contentPanel);
        ChatMessageComponent chatMessageComponent = chatGO.GetComponent<ChatMessageComponent>();
        chatMessageComponent.SetChatDetails(chatMessage.SenderId, chatMessage.Content);
    }

    // TODO: replace this with backend api logic here
    void SendMessage(Guid receiverId, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        
        // TODO: Remove this after backend api is implemented
        ChatMessage message =  new ChatMessage();
        message.SenderId = currentUserId;
        message.ReceiverId = receiverId;
        message.Content = content;
        message.IsGroupChat = false;
        message.CreatedTime = DateTime.Now; // TODO: check if this is auto-generated on backend
        AddNewChatEntry(Guid.NewGuid(), message);

        // HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
        // HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
        // if (response.IsSuccessStatusCode)
        // {
        //     string jsonResponse = await response.Content.ReadAsStringAsync();
        //     SendMessageResponse messageResponse = JsonConvert.DeserializeObject<SendMessageResponse>(jsonResponse);
        //     Debug.Log("User sent message successfully.);
        //     return true; // Registration successful
        // }
    }

    // TODO: replace this with backend api logic here
    string GetChatHistory(Guid otherUserID)
    {
        return chatTestJsonString;
    }
}

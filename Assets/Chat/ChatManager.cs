using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Clients;
using System.Threading.Tasks;


// For testing:
// agent uuids:
//      yoda: f7dd290b-faab-4c15-b8b9-38cff0895559
//      george washington: 55cd50d5-7775-4dd2-b632-a502a031ac41
// user uuids:
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
    // public Guid currentUserId = HTTPClient.Instance.MyId; // TODO: uncomment for actual backend testing
    // private Guid otherCharacterId; // TODO: uncomment for actual backend testing

    public Guid currentUserId = new Guid("55cd50d5-7775-4dd2-b632-a502a031ac41"); // TODO: Comment for backend testing
    public Guid otherCharacterId = new Guid("f7dd290b-faab-4c15-b8b9-38cff0895559"); // TODO: Comment for backend testing
    private Guid yodaID = new Guid("f7dd290b-faab-4c15-b8b9-38cff0895559");
    private Guid georgeWashID = new Guid("55cd50d5-7775-4dd2-b632-a502a031ac41");
    public GameObject AskMeQuestionButton;
    public InputField messageInputField;
    public Transform contentPanel;
    public GameObject chatMessagePrefab;
    public Text displayOtherCharacterName;
    public Image displayOtherCharacterHeadImage;
    public Image displayOtherCharacterActivityStatus;
    HashSet<Guid> generatedMessageIds = new HashSet<Guid>();
    private HTTPClient.UserData currentUserData;
    private HTTPClient.CharacterData otherCharacterData;
    private string otherCharacterType; // check if the other character is user or agent

    private string chatTestJsonString = @"
    [
        {
            ""id"": ""44dcb24a-3fb6-4533-be6f-7c56eb259c89"",
            ""senderId"": ""f7dd290b-faab-4c15-b8b9-38cff0895559"",
            ""receiverId"": ""55cd50d5-7775-4dd2-b632-a502a031ac41"",
            ""isOnline"": true,
            ""content"": ""This is old text"",
            ""isGroupChat"": false,
            ""createdTime"": ""2022-05-21T08:20:00Z""
        },
        {
            ""id"": ""21526023-eb1b-4b1a-be79-beb0d714dc45"",
            ""senderId"": ""55cd50d5-7775-4dd2-b632-a502a031ac41"",
            ""receiverId"": ""f7dd290b-faab-4c15-b8b9-38cff0895559"",
            ""content"": ""This is the newest text"",
            ""isOnline"": false,
            ""isGroupChat"": false,
            ""createdTime"": ""2023-11-07T14:45:30Z""
        },
        {
            ""id"": ""e23ae41c-b1da-4679-982f-2e38154b217b"",
            ""senderId"": ""55cd50d5-7775-4dd2-b632-a502a031ac41"",
            ""receiverId"": ""f7dd290b-faab-4c15-b8b9-38cff0895559"",
            ""content"": ""This is new textThis is new text\nThis is new textThis is new text\nThis is new text\nThis is new textThis is new textThis is new textThis is new textThis is new textThis is new textThis is new text"",
            ""isOnline"": true,
            ""isGroupChat"": false,
            ""createdTime"": ""2023-11-07T13:45:30Z""
        },
    ]";

    public List<HTTPClient.ChatMessage> sortedChatMessages;

    async void Start()
    {
        // TODO: Uncomment these when connecting with main game map to get Ids and type accordingly
        // otherCharacterId = new Guid(PlayerPrefs.GetString("CollidedCharacterId"));
        // otherCharacterType = PlayerPrefs.GetString("CollidedCharacterType");
        string otherCharacterType = "agent";
        if (otherCharacterType == "user")
        {
            // TODO: Comment for backend
            otherCharacterData = await LocalGetUser(otherCharacterId);
            HTTPClient.UserData otherUserData = otherCharacterData as HTTPClient.UserData;
            if (otherUserData != null && otherUserData.isOnline)
            {
                AskMeQuestionButton.SetActive(false);
            } else { // if other user is offline, display button
                AskMeQuestionButton.SetActive(true);
            }
            // TODO: Uncomment for backend
            // otherCharacterData = await GetUser(otherCharacterId);

        } else { // if the other character is an agent
            otherCharacterData = await LocalGetAgent(otherCharacterId);
            // otherCharacterData = await GetAgent(otherCharacterId);
            AskMeQuestionButton.SetActive(true);
        }

        // TODO: Switch for backend
        currentUserData = await LocalGetUser(currentUserId);
        // currentUserData = await GetUser(currentUserId);

        // Initialize other user's head sprite

        StartCoroutine(GetCharacterHeadSprite(otherCharacterData.sprite_headshot_URL, userHeadSprite => {
            BuildOtherCharacterProfile(otherCharacterData.username, userHeadSprite);
        }));

        Debug.Log("In chat manager, otherCharacterId: " + otherCharacterId.ToString());
        // TODO: Uncomment for backend
        // SignalRClient.Instance.RegisterSendMessageHandler(this);

        // TODO: Switch for backend.
        LocalBuildChatHistory();
        // BuildChatHistory();
    }

    public async void OnPressAskMeQuestion()
    {
        // TODO: Add backend API call to send bump response
        AskMeQuestionButton.SetActive(false);
        await Task.Delay(15000); // delay showing the button again to prevent users from spamming
        AskMeQuestionButton.SetActive(true);
    }

    private async Task<HTTPClient.UserData> LocalGetUser(Guid userId)
    {
        // Simulate a delay to mimic network request time
        await Task.Delay(1000);

        // Return a fake user based on the userId
        if (userId == currentUserId)
        {
            return new HTTPClient.UserData
            {
                id = currentUserId,
                username = "Current User",
                email = "currentuser@example.com",
                location = new HTTPClient.Location { coordX = 10, coordY = 20 },
                createdTime = DateTime.Parse("2024-01-01T00:01:00Z"),
                isOnline = true,
                sprite_URL = "https://example.com/currentuser_sprite.png",
                sprite_headshot_URL = "https://ibb.co/XZYT5xg"
            };
        }
        else if (userId == otherCharacterId)
        {
            return new HTTPClient.UserData
            {
                id = otherCharacterId,
                username = "Other User",
                email = "otheruser@example.com",
                location = new HTTPClient.Location { coordX = 30, coordY = 40 },
                createdTime = DateTime.Parse("2024-01-02T00:01:00Z"),
                isOnline = true,
                sprite_URL = "https://example.com/otheruser_sprite.png",
                sprite_headshot_URL = "https://picsum.photos/id/237/200"
            };
        }

        // Return null or throw an exception if the userId does not match
        Debug.LogWarning($"User with ID {userId} not found.");
        return null;
    }

    private async Task<HTTPClient.UserData> GetUser(Guid userId)
    {
        return await httpClient.GetUser(userId);
    }

    private async Task<HTTPClient.AgentData> LocalGetAgent(Guid agentId)
    {
        // Simulate a delay to mimic network request time
        await Task.Delay(1000);
        if (agentId == yodaID)
        {
            return new HTTPClient.AgentData
            {
                id = yodaID,
                username = "Yoda",
                description = "Yoda is a fictional character in the Star Wars universe, first appearing in the 1980 film The Empire Strikes Back. He is a small, green humanoid alien who is powerful with the Force. In his first appearance in the original trilogy, Yoda is described as the Jedi master of Obi-Wan Kenobi and subsequently trains Luke Skywalker to use the Force.",
                summary = "Jedi Master",
                location = new HTTPClient.Location { coordX = 10, coordY = 20 },
                creatorId = currentUserId,
                createdTime = DateTime.Parse("2024-01-01T00:01:00Z"),
                sprite_URL = "https://example.com/yoda_sprite.png",
                sprite_headshot_URL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRy-nPenacnE-7cGk5y6w7X_5OEU72JWVlgu-8L3xCl5Q&s",
                isHatched = true,
                hatchTime = DateTime.Parse("2024-01-01T00:01:00Z")
            };
        } else if (agentId == georgeWashID)
        {
            return new HTTPClient.AgentData
            {
                id = georgeWashID,
                username = "George Washington",
                description = "George Washington was an American political leader, military general, statesman, and Founding Father who served as the first president of the United States from 1789 to 1797. Previously, he led Patriot forces to victory in the nation's War for Independence. He presided at the Constitutional Convention of 1787, which established the U.S. Constitution and a federal government. Washington has been called the 'Father of His Country' for his manifold leadership in the formative days of the new nation.",
                summary = "First President of the United States",
                location = new HTTPClient.Location { coordX = 10, coordY = 20 },
                creatorId = currentUserId,
                createdTime = DateTime.Parse("2024-01-01T00:01:00Z"),
                sprite_URL = "https://example.com/gw_sprite.png",
                sprite_headshot_URL = "https://media.istockphoto.com/id/1362881287/vector/portrait-of-george-washington.jpg?s=1024x1024&w=is&k=20&c=4_Pgre2yHXslO_bisQGltoPS6dpt2Vbfz_khojcj-Po=",
                isHatched = true,
                hatchTime = DateTime.Parse("2024-01-01T00:01:00Z")
            }; 
        } else {
            Debug.LogWarning($"Agent with ID {agentId} not found.");
            return null;
        }
    }


    // TODO: Write the actual method for backend API
    private async Task<HTTPClient.AgentData> GetAgent(Guid agentId)
    {
        // return await httpClient.GetAgent(agentId);
        return null;
    }
    
    // Update is called once per frame
    void Update()
    {
        // TODO: Uncomment for signalR client connection
        // Check if there is a received message to process
        // if (!string.IsNullOrEmpty(SignalRClient.SenderId))
        // {
        //     Debug.Log("Got message, going to generate GCMO");
        //     // Create a ChatMessage object and reset the static variables
            // GenerateChatMessageObject(new HTTPClient.ChatMessage
            // {
            //     SenderId = new Guid(SignalRClient.SenderId),
            //     ReceiverId = currentUserId,
            //     Content = StringParser.ParseInput(SignalRClient.Message),
            //     IsOnline = SignalRClient.IsOnline,
            //     IsGroupChat = false,
            //     CreatedTime = DateTime.UtcNow
            // });


        //     // Reset the static variables to indicate that the message has been processed
        //     SignalRClient.SenderId = null;
        //     SignalRClient.Message = null;
        //     SignalRClient.IsOnline = null;
        // }
    }

    IEnumerator GetCharacterHeadSprite(string url, System.Action<Sprite> onCompleted)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
        yield return uwr.SendWebRequest();
        Debug.Log("GetCharHeadSprite URL: " + url);
        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Setting char head sprite");
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            Sprite characterHeadSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            onCompleted?.Invoke(characterHeadSprite); // Invoke the callback with the loaded sprite
        }
        else
        {
            Debug.LogError("Failed to download image: " + uwr.error);
            onCompleted?.Invoke(null);
        }
        uwr.Dispose();
    }

    private async void BuildOtherCharacterProfile(string username, Sprite charHeadSprite)
    {
        if (otherCharacterData == null){
            return;
        }

        displayOtherCharacterName.GetComponent<Text>().text = username;
        displayOtherCharacterHeadImage.sprite = charHeadSprite;

        // set activity status
        if (otherCharacterType == "user") {
            HTTPClient.UserData otherUserData = otherCharacterData as HTTPClient.UserData;
            if (otherUserData != null && otherUserData.isOnline)
            {
                displayOtherCharacterActivityStatus.sprite = Resources.Load<Sprite>("Shapes/green_circle");
            }
            else
            {
                displayOtherCharacterActivityStatus.sprite = Resources.Load<Sprite>("Shapes/red_circle");
            }
        } else { // if is agent
            displayOtherCharacterActivityStatus.sprite = Resources.Load<Sprite>("Shapes/blue_circle");
        }
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
            ["isOnline"] = message.IsOnline,
            ["createdTime"] = message.CreatedTime.ToString("o")  // "o" for round-trip date/time pattern
        };

        Debug.Log("newChat: " + newChat.ToString());

        // Add the new chat entry to the chats object
        chatsArray.Add(newChat);

        // Serialize the JObject back to a string
        chatTestJsonString = chatsArray.ToString(Formatting.Indented); // If you want it pretty-printed
        LocalBuildChatHistory();
    }

    // send message to backend
    public void onClickSendMessage() //add to unity button
    {

        // TODO: Switch for backend API
        LocalSendChat(otherCharacterId, messageInputField.text);
        // SendChat(otherCharacterId, messageInputField.text);
        // Debug.Log("after adding new chat entry" + chatTestJsonString);
        messageInputField.text = "";
    }

    async void BuildChatHistory()
    {
        sortedChatMessages = await httpClient.GetChatHistory(currentUserId, otherCharacterId);
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
        sortedChatMessages = JsonHelper.ConvertJsonToChatList(LocalGetChatHistory(otherCharacterId));
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
            chatMessageComponent.SetChatDetails(currentUserData.username, messageContent, chatMessage.IsOnline);
        }
        else
        {
            chatMessageComponent.SetChatDetails(otherCharacterData.username, messageContent, chatMessage.IsOnline);
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
        message.IsOnline = true; // TODO: Check if this is auto-generated on backend
        message.CreatedTime = DateTime.UtcNow; // TODO: check if this is auto-generated on backend

        await SignalRClient.Instance.SendChat(otherCharacterId, content);
        
        // TODO: Add logic to check SendChat result. Chat message object should only be generated 
        // after SendChat is successful
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
        HTTPClient.ChatMessage message = new HTTPClient.ChatMessage();
        message.SenderId = currentUserId;
        message.ReceiverId = receiverId;
        message.Content = content;
        message.IsGroupChat = false;
        message.IsOnline = true; // TODO: Check logic of this and if this is generated on backend
        message.CreatedTime = DateTime.UtcNow; // TODO: check if this is auto-generated on backend
        AddNewChatEntry(Guid.NewGuid(), message);
    }


    // For local frontend testing
    string LocalGetChatHistory(Guid otherCharacterId)
    {
        // return chatTestJsonString;
        return chatTestJsonString; // replace this line with above for local testing
    }
}
}
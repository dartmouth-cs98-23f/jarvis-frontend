using Microsoft.AspNetCore.SignalR.Client;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;


namespace Clients {

public class SignalRClient
{
    private readonly string username;
    private readonly HubConnection _connection;
    private Dictionary<Guid, Location> userLocations = new Dictionary<Guid, Location>(); // userId: location info about user
    private static SignalRClient instance;

    private SignalRClient(string url, string authToken)
    {
        // _connection = new HubConnectionBuilder()
        //     .WithUrl(url, options =>
        //     { 
        //         options.AccessTokenProvider = () => Task.FromResult(authToken);
        //     })
        //     .Build();
        _connection = new HubConnectionBuilder()
            .WithUrl($"{url}?access_token={authToken}")
            .Build();
    }

    public static SignalRClient Instance
    {
        get
        {
            if (instance == null)
            {
                throw new Exception("SignalRClient has not been initialized. Call Initialize method first.");
            }
            return instance;
        }
    }

    public static async Task Initialize(string authToken)
    {
        Debug.Log("In SignalR Initialize method");
        if (instance == null)
        {
//             string baseURL = "http://localhost:5000/unity";
            string baseURL = "https://api.simugameservice.lekina.me/unity";
            Debug.Log("Initializing SignalRClient with URL:" + baseURL + "with token: " + authToken);
            instance = new SignalRClient(baseURL, authToken);
            Debug.Log("Post instance assignment" + instance);
            await instance.ConnectAsync();
            if (instance._connection.State == HubConnectionState.Connected)
            {
                Debug.Log("IM ACTUALLY CONNECTED");
            }
            else{
                Debug.Log("I am not actually connected");
            }
        } else {
            Debug.Log("SignalRClient already initialized");
        }
    }

    public static bool IsConnected()
    {
        return instance != null && instance._connection.State == HubConnectionState.Connected;
    }

    /// <summary>
    /// Connects the client to the server.
    /// </summary>
    /// <returns></returns>
    public async Task ConnectAsync()
    {
        await _connection.StartAsync();
    }

    /// <summary>
    /// Disconnects the client from the server.
    /// </summary>
    /// <returns></returns>
    public async Task DisconnectAsync()
    {
        await _connection.StopAsync();
    }

    /// <summary>
    /// Broadcasts a message to all connected clients.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task BroadcastMessage(string funcName, string message)
    {
        await _connection.SendAsync(funcName, username, message);
    }

    /// <summary>
    /// Registers a handler for incoming messages.
    /// </summary>
    /// @Deprecated. This has now been changed to the MessageHandler method.
    public void RegisterReceiveMessageHandler()
    {
        _connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Console.WriteLine($"({user}): {message}");
        });
    }

    // Calls the backend method UpdateLocation with the current location of the user
    public async Task UpdateLocation(int xCoordinate, int yCoordinate)
    {
            var location = new Location
            {
                X_coord = xCoordinate,
                Y_coord = yCoordinate
            };

            try
            {
                await _connection.SendAsync("UpdateLocation", location);
                Debug.Log("update location sent " + location.X_coord + " " + location.Y_coord);
            } catch (Exception ex) 
            {
                Debug.Log("In updateLocation not Connected, failed to update");
            }
    }

     public void RegisterUpdateLocationHandler(OtherPlayerMovement otherPlayerMovementScript)
    {
        // TODO: Refactor handleError out into a combined handler
        _connection.On<string>("HandleError", (error) =>
        {
            Debug.Log($"Error: {error}");
        });
        _connection.On<Guid, Location>("UpdateLocationHandler", (userId, location) =>
        {
            Debug.Log($"User {userId} moved to X: {location.X_coord}, Y: {location.Y_coord}");
            
//             userLocations[userId] = new Location { X_coord = location.X_coord, Y_coord = location.Y_coord }; // TODO: Check if this is needed
            otherPlayerMovementScript.UpdateLocation(userId, location.X_coord, location.Y_coord);
        });
    }

    /// <summary>
    /// Sends a <paramref name="message"/> to the user with the given <paramref name="receiverId"/>
    /// </summary>
    /// <param name="receiverId">The ID of the message target</param>
    /// <param name="message">The actual contents of the message</param>
    /// <returns></returns>
    public async Task SendChat(Guid receiverId, string message)
    {
        Debug.Log("Sending chat message to " + receiverId + " with message: " + message);
        try
        {
            await _connection.SendAsync("SendChat", receiverId, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }


    /// <summary>
    /// Handles a message received from another client through the server.
    /// </summary>
    /// <param name="ChatResponse">The chat message object from the server</param>
    /// <returns></returns>
    public void ChatHandler(ChatManager.ChatManager chatManager)
    {
        _connection.On<ChatResponse>("ChatHandler", (ChatResponse chatResp) =>
        {
            Debug.Log($"Message {chatResp.Id} received from user id: {chatResp.SenderId} sending to {chatResp.ReceiverId}. User is online: {chatResp.IsSenderOnline}. Message: {chatResp.Content}");
            chatManager.ReceiveMessage(chatResp.Id, chatResp.SenderId, chatResp.Content, chatResp.IsSenderOnline);
        });
    }


    /// <summary>
    /// Handles a message received from another client through the server.
    /// </summary>
    /// <param name="sender">The client (can also be the server) sending the message</param>
    /// <param name="message">The content of the message</param>
    /// <returns></returns>
    public void MessageHandler()
    {
        _connection.On<string>("MessageHandler", (string message) =>
        {
            Debug.Log($"Message received from server: {message}");
        });
    }


    /// <summary>
    /// Handles a server request that checks if the user is online.
    /// The server will routinely send this request to check if the client is still logged in.
    /// The client should respond by sending a <see cref="IUnityServer.PingServer"/> request.
    /// </summary>
    /// <returns></returns>
    public async Task UserOnlineCheckHandler()
    {
        _connection.On("UserOnlineCheckHandler", async () =>
        {
            Debug.Log("Server checking if this client is online");
            await PingServer(); // Respond to the server that the user is online
        });
    }


    /// <summary>
    /// Notifies the server that the user is online. We use this to keep track of active users. 
    /// </summary>
    /// <returns></returns>
    public async Task PingServer()
    {
        try
        {
            await _connection.SendAsync("PingServer");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }    
    }

    /// <summary>
    /// Handles a server request that notifies clients when a world's user logs into the server.
    /// </summary>
    /// <param name="userId">The ID of the newly-logged in user</param>
    /// <returns></returns>
    public void OnUserLoggedInHandler()
    {
        _connection.On<Guid>("OnUserLoggedInHandler", (Guid userId) =>
        {
            Debug.Log($"User {userId} has logged in. Setting them to online in world...");
        });
    }

    /// <summary>
    /// Handles a server request that notifies clients when a world's user logs into the server.
    /// </summary>
    /// <param name="userId">The ID of the newly-logged in user</param>
    /// <returns></returns>
    public void OnUserLoggedOutHandler()
    {
        _connection.On<Guid>("OnUserLoggedOutHandler", (Guid userId) =>
        {
            Debug.Log($"User {userId} has logged out. Setting them to offline in world...");
        });
    }


    // This hits the OnUserLoggedInHandler endpoint but is specifically used in chat manager to update the display
    // of the other user's online status UI
    public void OnUserLoggedInChatHandler(ChatManager.ChatManager chatManager)
    {
        _connection.On<Guid>("OnUserLoggedInHandler", (Guid userId) =>
        {
            Debug.Log($"User {userId} has logged in. Setting them to online in chat...");
            chatManager.SetUserIsOnline(userId, true);
        });
    }

    // This hits the OnUserLoggedOutHandler endpoint but is specifically used in chat manager to update the display
    // of the other user's online status UI
    public void OnUserLoggedOutChatHandler(ChatManager.ChatManager chatManager)
    {
        _connection.On<Guid>("OnUserLoggedOutHandler", (Guid userId) =>
        {
            Debug.Log($"User {userId} has logged out. Setting them to offline in chat...");
            chatManager.SetUserIsOnline(userId, false);
        });
    }


    /// <summary>
    /// Handles a server request that notifies clients when a new agent is added to the world.
    /// </summary>
    /// <param name="agentId">The ID of the newly-added agent</param>
    /// <returns></returns>
    public void OnAgentAddedToWorldHandler(GameClient gameClient)
    {
        _connection.On<Guid>("OnAgentAddedHandler", (Guid agentId) =>
        {
            Debug.Log($"Agent {agentId} has been added to world. Adding agent to world...");
            gameClient.AddAgentToWorld(agentId);
        });
    }

    /// <summary>
    /// Handles a server request that notifies clients when a new user joins the world.
    /// </summary>
    /// <param name="userId">The ID of the newly-added user</param>
    /// <returns></returns>
    public void OnUserAddedToWorldHandler(GameClient gameClient)
    {
        _connection.On<Guid>("OnUserAddedToWorldHandler", (Guid userId) =>
        {
            Debug.Log($"User {userId} has joined the world. Adding user to world...");
            gameClient.AddUserToWorld(userId);
        });
    }


    /// <summary>
    /// Handles a server request that notifies clients when a user leaves the world.
    /// </summary>
    /// <param name="userId">The ID of the user who left the world</param>
    /// <returns></returns>
    public void OnUserRemovedFromWorldHandler(GameClient gameClient)
    {
        _connection.On<Guid>("OnUserRemovedFromWorldHandler", (Guid userId) =>
        {
            Debug.Log($"User {userId} has been removed from the world. Removing user to world...");
            gameClient.RemoveUserFromWorld(userId);
        });
    }

    public class ChatResponse {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public bool IsSenderOnline { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public class Location
    {
        public int X_coord { get; set; }
        public int Y_coord { get; set; }
    }
    public Dictionary<Guid, Location> UserLocations
    {
        get { return userLocations; }
    }

}
}
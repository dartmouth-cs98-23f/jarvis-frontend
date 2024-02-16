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

    public static string SenderId;
    public static string Message;
    public static bool IsOnline;
    private static SignalRClient instance;

    private SignalRClient(string username, string url)
    {
        this.username = username.Length <= 10 ? username.ToLower() : username.ToLower()[..10];
        _connection = new HubConnectionBuilder()
            .WithUrl(url)
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

    public static async Task Initialize(string authToken, string username)
    {
        if (instance == null)
        {
            // string baseURL = "http://localhost:5087/unity";
            string baseURL = "https://simyou.azurewebsites.net/unity";
            string urlWithAuthToken = baseURL + "?auth_token=" + authToken;
            Debug.Log("Initializing SignalRClient with URL:" + urlWithAuthToken);
            instance = new SignalRClient(username, urlWithAuthToken);
            Debug.Log("Post instance assignment" + instance);
            await instance.ConnectAsync();
            if (instance._connection.State == HubConnectionState.Connected)
            {
                Debug.Log("IM ACTUALLY CONNECTED");
            }
            else{
                Debug.Log("I am not actually connected");
            }
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
                X_coordinate = xCoordinate,
                Y_coordinate = yCoordinate
            };

            if (IsConnected()) 
            {
                await _connection.SendAsync("UpdateLocation", location.X_coordinate, location.Y_coordinate);
            } else 
            {
                Debug.Log("In updateLocation not Connected, failed to update");
            }
        }

     public void RegisterUpdateLocationHandler()
        {
            // TODO: Refactor handleError out into a combined handler
            _connection.On<string>("HandleError", (error) =>
            {
                Debug.Log($"Error: {error}");
            });
            _connection.On<Guid, int, int>("UpdateLocation", (userId, xCoord, yCoord) =>
            {
                Debug.Log($"User {userId} moved to X: {xCoord}, Y: {yCoord}");
                
                userLocations[userId] = new Location { X_coordinate = xCoord, Y_coordinate = yCoord };
            });
        }

    // Sends a message to a user through the server.
    public async Task SendChat(Guid receiverId, string message)
    {
        try
        {
            await _connection.SendAsync("SendChat", receiverId, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }

    public void RegisterSendMessageHandler(ChatManager.ChatManager chatManager)
    {
        _connection.On<string, string, string>("ReceiveMessage", (senderId, message, isOnline) =>
        {
            Debug.Log($"Message received from user {senderId}: {message}. User is online: {isOnline}");
            SenderId = senderId;
            Message = message;
            IsOnline = isOnline == "True" ? true : false;
        });
    }

    public class Location
    {
        public int X_coordinate { get; set; }
        public int Y_coordinate { get; set; }
    }
    public Dictionary<Guid, Location> UserLocations
    {
        get { return userLocations; }
    }

}
}
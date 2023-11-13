using Microsoft.AspNetCore.SignalR.Client;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;


namespace Clients {

public class SignalRClient
{
    private readonly string firstName;
    private readonly HubConnection _connection;
    private Dictionary<Guid, Location> userLocations = new Dictionary<Guid, Location>(); // userId: location info about user
    private static SignalRClient instance;

    private SignalRClient(string firstName, string url)
    {
        this.firstName = firstName.Length <= 10 ? firstName.ToLower() : firstName.ToLower()[..10];
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

    public static async Task Initialize(Guid userId, string firstName)
    {
        if (instance == null)
        {
            string baseURL = "http://localhost:5087/unity";
            string urlWithUserId = baseURL + "?userId=" + userId.ToString();
            Debug.Log("Initializing SignalRClient with URL:" + urlWithUserId);
            instance = new SignalRClient(firstName, urlWithUserId);
            await instance.ConnectAsync();
            if (instance._connection.State == HubConnectionState.Connected)
            {
                Debug.Log("IM ACTUALLY CONNECTED");
            }
            else{
                Debug.Log("I am not actually connected :(");
            }
        } 
        // TODO: Delete code below
        // else 
        // {
        //     Debug.Log("Called Initialize but SignalRClient Instance already exist");
        //     if (IsConnected())
        //     {
        //         Debug.Log("IM ACTUALLY CONNECTED and Instance is not null");
        //     }
        //     else{
        //         Debug.Log("I am not actually connected :( and Instance is not null. I'm gonna try reconnecting...");
        //         instance = new SignalRClient("Vico1", url);
        //         await instance.ConnectAsync();
        //         if (instance._connection.State == HubConnectionState.Connected)
        //         {
        //             Debug.Log("I reconnected");
        //         }
        //         else{
        //             Debug.Log("I failed to reconnect");
        //         }
        //     }
        // }
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
        await _connection.SendAsync(funcName, firstName, message);
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

            Debug.Log("UpdateLocation called with location " + location.X_coordinate + " " + location.Y_coordinate);
            if (IsConnected()) 
            {
                await _connection.SendAsync("UpdateLocation", location.X_coordinate, location.Y_coordinate);
                Debug.Log("In updateLocation isConnected, updating location");
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

    public void RegisterSendMessageHandler()
    {
        _connection.On<string, string>("SendMessage", (senderId, message) =>
        {
            Console.WriteLine($"Message received from user {senderId}: {message}");
            // Handle the received message, for example, display it in your game UI
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
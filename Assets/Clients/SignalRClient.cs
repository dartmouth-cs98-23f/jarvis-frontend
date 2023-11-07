using Microsoft.AspNetCore.SignalR.Client;

namespace MockSignalRClient.ClientLibrary;

public class SignalRClient
{
    private readonly string userName;
    private readonly HubConnection _connection;
    private Dictionary<Guid, Location> userLocations = new Dictionary<Guid, Location>(); // userId: location info about user

    /// <summary>
    /// Creates a new instance of a SignalR client.
    /// </summary>
    /// <param name="url">The URL of the server to connect to.</param>
    public SignalRClient(string userName, string url)
    {
        this.userName = userName.Length
                    <= 10 ? userName.ToLower() : userName.ToLower()[..10];
        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();
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
        await _connection.SendAsync(funcName, userName, message);
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

            await _connection.SendAsync("UpdateLocation", location);
        }

     public void RegisterUpdateLocationHandler()
        {
            _connection.On<string, Location>("UpdateLocation", (userId, location) =>
            {
                Console.WriteLine($"User {userId} moved to X: {location.X_coordinate}, Y: {location.Y_coordinate}");
                
                userLocations[userId] = location;
            });
        }

    // Sends a message to a user through the server.
    public async Task SendMessage(Guid receiverId, string message)
    {
        try
        {
            await _connection.SendAsync("SendMessage", receiverId, message);
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


}
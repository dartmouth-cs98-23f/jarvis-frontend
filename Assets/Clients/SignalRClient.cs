// using Microsoft.AspNetCore.SignalR.Client;

// namespace MockSignalRClient.ClientLibrary;

// public class SignalRClient
// {
//     private readonly string userName;
//     private readonly HubConnection _connection;

//     /// <summary>
//     /// Creates a new instance of a SignalR client.
//     /// </summary>
//     /// <param name="url">The URL of the server to connect to.</param>
//     public SignalRClient(string userName, string url)
//     {
//         this.userName = userName.Length
//                     <= 10 ? userName.ToLower() : userName.ToLower()[..10];
//         _connection = new HubConnectionBuilder()
//             .WithUrl(url)
//             .Build();
//     }

//     /// <summary>
//     /// Connects the client to the server.
//     /// </summary>
//     /// <returns></returns>
//     public async Task ConnectAsync()
//     {
//         await _connection.StartAsync();
//     }

//     /// <summary>
//     /// Disconnects the client from the server.
//     /// </summary>
//     /// <returns></returns>
//     public async Task DisconnectAsync()
//     {
//         await _connection.StopAsync();
//     }

//     /// <summary>
//     /// Broadcasts a message to all connected clients.
//     /// </summary>
//     /// <param name="message"></param>
//     /// <returns></returns>
//     public async Task BroadcastMessage(string funcName, string message)
//     {
//         await _connection.SendAsync(funcName, userName, message);
//     }

//     /// <summary>
//     /// Registers a handler for incoming messages.
//     /// </summary>
//     public void RegisterReceiveMessageHandler()
//     {
//         _connection.On<string, string>("ReceiveMessage", (user, message) =>
//         {
//             Console.WriteLine($"({user}): {message}");
//         });
//     }


// }
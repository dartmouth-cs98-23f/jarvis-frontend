// using MockSignalRClient.ClientLibrary;

// namespace MockSignalRClient.ConsoleClient;

// public class Program
// {
//     public static async Task Main(string[] args)
//     {
//         // get the server url from the command line args
//         var userName = args[0];
//         var serverUrl = args[1];

//         // initialize a new client, register handler for incoming messages, and connect to the server
//         var client = new SignalRClient(userName, serverUrl);
//         client.RegisterReceiveMessageHandler();
//         await client.ConnectAsync();

//         // prompt the user to type a message
//         Console.WriteLine("Type a message and press enter to send it to the server. Type 'exit' to quit.");

//         // loop until the user types "exit"
//         while (true)
//         {
//             var message = Console.ReadLine();
//             if (message == "exit")
//             {
//                 break;
//             }

//             if (!string.IsNullOrEmpty(message))
//             {
//                 await client.BroadcastMessage(message);
//             }
//         }

//         // disconnect
//         await client.DisconnectAsync();
//     }
// }
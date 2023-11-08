using UnityEngine;
using Clients;


public class GameClient : MonoBehaviour
{

    void Start()
    {
        // Initialize SignalR connection
        SignalRClient signalRClient = SignalRClient.Instance;

        // Connect to the SignalR server
        signalRClient.ConnectAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to connect to SignalR server.");
            }
            else
            {
                // Register UpdateLocation handler to listen for constant updates
                signalRClient.RegisterUpdateLocationHandler();
            }
        });
    }
}

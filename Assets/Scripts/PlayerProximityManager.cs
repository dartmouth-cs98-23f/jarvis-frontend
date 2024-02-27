using System;
using UnityEngine;
using System.Collections.Generic;
using Clients;

public class PlayerProximityManager : MonoBehaviour
{
    private SignalRClient signalRClient = SignalRClient.Instance;
    private HTTPClient httpClient = HTTPClient.Instance;
    private float proximityThreshold = 10f;

    void Start(){

    }
    void Update(){
        // CheckProximity(httpClient.MyId);
    }

    private async void CheckProximity(Guid currentPlayerId)
    {
        Dictionary<Guid, SignalRClient.Location> userLocations = signalRClient.UserLocations;

        SignalRClient.Location currentPlayerPosition = userLocations[currentPlayerId];
        Debug.Log("Running Check Proximity");

        foreach (var playerId in userLocations.Keys)
        {
            if (playerId != currentPlayerId)
            {
                SignalRClient.Location otherPlayerPosition = userLocations[playerId];
                float distance = Mathf.Sqrt(Mathf.Pow(otherPlayerPosition.X_coord - currentPlayerPosition.X_coord, 2) +
                            Mathf.Pow(otherPlayerPosition.Y_coord - currentPlayerPosition.Y_coord, 2));

                if (distance <= proximityThreshold)
                {
                    // Players are in proximity, perform actions (e.g., fetch user data, chat history)
                    HTTPClient.UserData userInfo = await httpClient.GetUser(playerId);
                    List<HTTPClient.ChatMessage> chatHistory = await httpClient.GetChatHistory(currentPlayerId, playerId);
                    Debug.Log("Got userInfo: " + userInfo + "and chatHistory " + chatHistory);

                    break;
                }
            }
        }
    }

}

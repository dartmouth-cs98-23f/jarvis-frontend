using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

public class PlayersListManager : MonoBehaviour
{
    public TextMeshProUGUI playerCountText;
    public GameObject content;
    public GameObject playerInfoPrefab;
    private HTTPClient httpClient = HTTPClient.Instance;
    public SpriteLoader spriteLoader;
    public SideMenu sideMenuManager;
    public GameObject kickPanel;
    public GameObject playersListPanel;
    public GameObject playersListManager;
    public TextMeshProUGUI kickUsername;
    public Guid playerID;
    public async void localDisplayPlayersList()
    {
        string playersJson = @"
        [
            {
                ""id"": ""11111111-1111-1111-1111-111111111111"",
                ""username"": ""Player1"",
                ""location"": {
                    ""x_coord"": 10,
                    ""y_coord"": 20
                },
                ""isOnline"": true,
                ""sprite_URL"": ""https://picsum.photos/200"",
                ""sprite_headshot_URL"": ""https://picsum.photos/200""
            },
            {
                ""id"": ""22222222-2222-2222-2222-222222222222"",
                ""username"": ""Player2"",
                ""location"": {
                    ""x_coord"": 15,
                    ""y_coord"": 25
                },
                ""isOnline"": false,
                ""sprite_URL"": ""https://picsum.photos/200"",
                ""sprite_headshot_URL"": ""https://picsum.photos/200""
            },
            {
                ""id"": ""33333333-3333-3333-3333-333333333333"",
                ""username"": ""Player3"",
                ""location"": {
                    ""x_coord"": 30,
                    ""y_coord"": 40
                },
                ""isOnline"": true,
                ""sprite_URL"": ""https://picsum.photos/200"",
                ""sprite_headshot_URL"": ""https://picsum.photos/200""
            },
            {
                ""id"": ""44444444-4444-4444-4444-444444444444"",
                ""username"": ""Player4"",
                ""location"": {
                    ""x_coord"": 35,
                    ""y_coord"": 45
                },
                ""isOnline"": false,
                ""sprite_URL"": ""https://picsum.photos/200"",
                ""sprite_headshot_URL"": ""https://picsum.photos/200""
            },
            {
                ""id"": ""55555555-5555-5555-5555-555555555555"",
                ""username"": ""Player5"",
                ""location"": {
                    ""x_coord"": 50,
                    ""y_coord"": 60
                },
                ""isOnline"": true,
                ""sprite_URL"": ""https://picsum.photos/200"",
                ""sprite_headshot_URL"": ""https://picsum.photos/200""
            },
            {
                ""id"": ""66666666-6666-6666-6666-666666666666"",
                ""username"": ""Player6"",
                ""location"": {
                    ""x_coord"": 55,
                    ""y_coord"": 65
                },
                ""isOnline"": false,
                ""sprite_URL"": ""https://picsum.photos/200"",
                ""sprite_headshot_URL"": ""https://picsum.photos/200""
            }
        ]";

    List<HTTPClient.UserData> dummyPlayerList = JsonConvert.DeserializeObject<List<HTTPClient.UserData>>(playersJson);

    for (int i = 0; i < dummyPlayerList.Count; i++)
    {
        HTTPClient.UserData playerInfo = dummyPlayerList[i];
        GameObject playersListGO = Instantiate(playerInfoPrefab, content.transform);
        playersListGO.tag = "PlayerInfoPrefab";

        PlayerInfoComponent playerInfoComponent = playersListGO.GetComponent<PlayerInfoComponent>();
        TextMeshProUGUI username = playerInfoComponent.usernameTMP;
        GameObject onlineIndicator = playerInfoComponent.onlineIndicator;

        playerInfoComponent.kickPanel = kickPanel;
        playerInfoComponent.playersListPanel = playersListPanel;
        playerInfoComponent.playersListManager = playersListManager.GetComponent<PlayersListManager>();
        playerInfoComponent.playerId = playerInfo.id;

        if (playerInfo.id == new Guid("22222222-2222-2222-2222-222222222222")){
                playerInfoComponent.IsOwner();
            }
        
        playerInfoComponent.spriteHeadshotPrefab.GetComponent<BodyPartsManager>().SetSprite(new List<int> {1, 0, 1, 1});

        // Set player details dynamically
        playerInfoComponent.SetPlayerDetails(playerInfo.username);

        // Change color based on online/offline status
        if (playerInfo.isOnline)
        {
            Color color = onlineIndicator.GetComponent<Image>().color;
            color = new Color(110 / 255.0f, 140 / 255.0f, 51 / 255.0f);
            color.a = 1;
            onlineIndicator.GetComponent<Image>().color = color;
            Outline outline = onlineIndicator.GetComponent<Outline>();
            Color outlineColor = outline.effectColor;
            outlineColor.a = 0.5f;
            outline.effectColor = outlineColor;
            onlineIndicator.GetComponent<Outline>().enabled = false;

        }
        else
        {
            Color color = onlineIndicator.GetComponent<Image>().color;
            color = new Color(0f, 0f, 0f, 0f);
            onlineIndicator.GetComponent<Image>().color = color;
            Outline outline = onlineIndicator.GetComponent<Outline>();
            Color outlineColor = outline.effectColor;
            outlineColor = new Color(115/255.0f, 123/255.0f, 125/255.0f);
            outlineColor.a = 0.5f;
            outline.effectColor = outlineColor;
            onlineIndicator.GetComponent<Outline>().enabled = true;
        }
    }
    sideMenuManager.TogglePlayersListPanel();
}

    public async void DisplayPlayersList(){
        List<HTTPClient.UserData> playerList = await httpClient.GetWorldUsers(httpClient.WorldId);

        if (playerList == null)
        {
            Debug.Log("Player List is null");
        }

        int playerCount = playerList.Count;

        if (playerCountText != null)
        {
            playerCountText.text = "Player Count: " + playerCount;
        }
        else
        {
            Debug.Log("Player Count Text component not assigned.");
        }

        foreach (HTTPClient.UserData playerInfo in playerList)
        {
            GameObject playersListGO = Instantiate(playerInfoPrefab, content.transform);
            playersListGO.tag = "PlayerInfoPrefab";

            // Access child components directly
            PlayerInfoComponent playerInfoComponent = playersListGO.GetComponent<PlayerInfoComponent>();
            TextMeshProUGUI username = playerInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            GameObject onlineIndicator = playerInfoComponent.onlineIndicator; // Access the child GameObject

            playerInfoComponent.kickPanel = kickPanel;
            playerInfoComponent.playersListPanel = playersListPanel;
            playerInfoComponent.playersListManager = playersListManager.GetComponent<PlayersListManager>();
            playerInfoComponent.playerId = playerInfo.id;
            playerInfoComponent.spriteHeadshotPrefab.GetComponent<BodyPartsManager>().SetSprite(playerInfo.spriteAnimations);

            HTTPClient.IdData creator = await httpClient.GetWorldCreator();
            if (playerInfo.id == creator.id){
                playerInfoComponent.IsOwner();
            }
            else if (httpClient.MyId == creator.id){
                playerInfoComponent.ShowKick();
            }

            // Set player details dynamically
            playerInfoComponent.SetPlayerDetails(playerInfo.username);

            // Change color based on online/offline status
            if (playerInfo.isOnline)
            {
                onlineIndicator.GetComponent<Image>().color = new Color(110 / 255.0f, 140 / 255.0f, 51 / 255.0f);
                onlineIndicator.GetComponent<Outline>().enabled = false;
            }
            else
            {
                onlineIndicator.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                onlineIndicator.GetComponent<Outline>().enabled = true;
                onlineIndicator.GetComponent<Outline>().effectColor = new Color(115/255.0f, 123/255.0f, 125/255.0f);
            }
    }
    sideMenuManager.TogglePlayersListPanel();
    }

    // Deletes instantiations of the prefab that shows up on the players list when the panel is closed out
    public void ClosePlayerListPanel()
    {
        // Iterate through each child of the PlayerListPanel
        foreach (Transform child in content.transform)
        {
            // Check if the child has the specified tag
            if (child.gameObject.tag == "PlayerInfoPrefab")
            {
                // Destroy the prefab instances
                Destroy(child.gameObject);
            }
        }
    }

    // For local testing
    // Should get player count from backend and display
    public void localPlayerCount()
    {
        int count = 30;
        if (playerCountText != null)
        {
            playerCountText.text = "Player Count: " + count;
        }
        else
        {
            Debug.Log("Player Count Text component not assigned.");
        }
    }

    public void OnCancelPressed(){
        kickPanel.SetActive(false);
        playersListPanel.SetActive(true);
    }

    public async void OnKickPressed(){
        kickPanel.SetActive(false);
        playersListPanel.SetActive(true);
        
        // Call the method to remove the user from the world
        bool response = await httpClient.RemoveUserFromWorld(playerID, httpClient.MyId);

        // Handle the response based on the boolean value
        if (response)
        {
            Debug.Log("User removed from the world successfully.");
        }
        else
        {
            Debug.Log("Failed to remove user from the world.");
        }
    }
    public void SetKickUsername(TextMeshProUGUI name){
        kickUsername.text = "Kick " + name.text + " from World?";
    }
    
    public void SetPlayerID(Guid playerId){
        playerID = playerId;
    }
}

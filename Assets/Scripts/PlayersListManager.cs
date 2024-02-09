using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class PlayersListManager : MonoBehaviour
{
    public TextMeshProUGUI playerCountText;
    public GameObject content;

    [System.Serializable]
    public class PlayerInfo // TODO: Delete when backend testing works
    {
        public bool isOnline;
        public Sprite spriteHead;
        public string username;
    }

    public List<PlayerInfo> playerList;
    public GameObject playerInfoPrefab;
    private HTTPClient httpClient = HTTPClient.Instance;
    public SpriteLoader spriteLoader;
    public SideMenu sideMenuManager;
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
                ""isCreator"": false,
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
                ""isCreator"": false,
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
                ""isCreator"": true,
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
                ""isCreator"": false,
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
                ""isCreator"": false,
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
                ""isCreator"": true,
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
        Image spriteHead = playerInfoComponent.displayUserImage;
        TextMeshProUGUI username = playerInfoComponent.usernameTMP;
        GameObject onlineIndicator = playerInfoComponent.onlineIndicator;

        spriteLoader.LoadSprite(playerInfo.sprite_headshot_URL, (sprite) => {
            // Set player details dynamically
            playerInfoComponent.SetPlayerDetails(sprite, playerInfo.username);

            // Change color based on online/offline status
            if (playerInfo.isOnline)
            {
                onlineIndicator.GetComponent<Image>().color = Color.green;
                onlineIndicator.GetComponent<Outline>().enabled = false;
            }
            else
            {
                onlineIndicator.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                onlineIndicator.GetComponent<Outline>().enabled = true;
                onlineIndicator.GetComponent<Outline>().effectColor = Color.gray;
            }
        });
    }
    await Task.Delay(500);
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
            Image spriteHead = playerInfoComponent.displayUserImage; // Access the child Image component
            TextMeshProUGUI username = playerInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            GameObject onlineIndicator = playerInfoComponent.onlineIndicator; // Access the child GameObject

            spriteLoader = GameObject.FindObjectOfType<SpriteLoader>();

            // Call the LoadSprite method with the desired URL
            spriteLoader.LoadSprite(playerInfo.sprite_headshot_URL, (sprite) => {
            // Set player details dynamically
            playerInfoComponent.SetPlayerDetails(sprite, playerInfo.username);

            // Change color based on online/offline status
            if (playerInfo.isOnline)
            {
                onlineIndicator.GetComponent<Image>().color = Color.green;
                onlineIndicator.GetComponent<Outline>().enabled = false;
            }
            else
            {
                onlineIndicator.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                onlineIndicator.GetComponent<Outline>().enabled = true;
                onlineIndicator.GetComponent<Outline>().effectColor = Color.gray;
            }
        });
    }
    await Task.Delay(500);
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
}

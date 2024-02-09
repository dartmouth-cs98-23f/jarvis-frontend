using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;

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
    private SpriteLoader spriteLoader;
    public SideMenu sideMenuManager;
    public async void localDisplayPlayersList()
    {
        List<PlayerInfo> dummyPlayerList = new List<PlayerInfo>();

        // Dummy player 1 (Online)
        PlayerInfo player1 = new PlayerInfo();
        player1.isOnline = true;
        player1.spriteHead = Resources.Load<Sprite>("Shapes/master_yoda_head");
        player1.username = "PlayerOne";
        dummyPlayerList.Add(player1);

        // Dummy player 2 (Offline)
        PlayerInfo player2 = new PlayerInfo();
        player2.isOnline = false;
        player2.spriteHead = Resources.Load<Sprite>("Shapes/george_washington_head");
        player2.username = "PlayerTwo";
        dummyPlayerList.Add(player2);

        // Dummy player 3 (Online)
        PlayerInfo player3 = new PlayerInfo();
        player3.isOnline = true;
        player3.spriteHead = Resources.Load<Sprite>("Shapes/user_head");
        player3.username = "PlayerThree";
        dummyPlayerList.Add(player3);

        // Dummy player 4 (Online)
        PlayerInfo player4 = new PlayerInfo();
        player4.isOnline = true;
        player4.spriteHead = Resources.Load<Sprite>("Shapes/master_yoda_head");
        player4.username = "PlayerOne";
        dummyPlayerList.Add(player4);

        // Dummy player 5 (Offline)
        PlayerInfo player5 = new PlayerInfo();
        player5.isOnline = false;
        player5.spriteHead = Resources.Load<Sprite>("Shapes/george_washington_head");
        player5.username = "PlayerTwo";
        dummyPlayerList.Add(player5);

        // Dummy player 6 (Online)
        PlayerInfo player6 = new PlayerInfo();
        player6.isOnline = true;
        player6.spriteHead = Resources.Load<Sprite>("Shapes/user_head");
        player6.username = "PlayerThree";
        dummyPlayerList.Add(player6);
        

    for (int i = 0; i < dummyPlayerList.Count; i++)
    {
        PlayerInfo playerInfo = dummyPlayerList[i];
        GameObject playersListGO = Instantiate(playerInfoPrefab, content.transform);
        playersListGO.tag = "PlayerInfoPrefab";

        PlayerInfoComponent playerInfoComponent = playersListGO.GetComponent<PlayerInfoComponent>();
        Image spriteHead = playerInfoComponent.displayUserImage;
        TextMeshProUGUI username = playerInfoComponent.usernameTMP;
        GameObject onlineIndicator = playerInfoComponent.onlineIndicator;

        spriteLoader = GameObject.FindObjectOfType<SpriteLoader>();

        spriteLoader.LoadSprite("https://picsum.photos/200", (sprite) => {
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

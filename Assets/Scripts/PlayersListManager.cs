using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayersListManager : MonoBehaviour
{
    public TextMeshProUGUI playerCountText;
    public GameObject content;

    [System.Serializable]
    public class PlayerInfo
    {
        public bool isOnline;
        public Sprite spriteHead;
        public string username;
    }

    public List<PlayerInfo> playerList;
    public GameObject playerInfoPrefab;

    public void localDisplayPlayersList()
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
        

        for (int i = 0; i < dummyPlayerList.Count; i++)
        {
            PlayerInfo playerInfo = dummyPlayerList[i];
            GameObject playersListGO = Instantiate(playerInfoPrefab, content.transform);
            playersListGO.tag = "PlayerInfoPrefab";

            // Access child components directly
            PlayerInfoComponent playerInfoComponent = playersListGO.GetComponent<PlayerInfoComponent>();
            Image spriteHead = playerInfoComponent.displayUserImage; // Access the child Image component
            TextMeshProUGUI username = playerInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            GameObject onlineIndicator = playerInfoComponent.onlineIndicator; // Access the child GameObject

            // Set player details dynamically
            playerInfoComponent.SetPlayerDetails(playerInfo.isOnline, playerInfo.spriteHead, playerInfo.username);

            // Change color based on online/offline status
            if (onlineIndicator.activeSelf)
            {
                onlineIndicator.GetComponent<Image>().color = Color.green; // Online (filled)
            }
            else
            {
                onlineIndicator.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f); // Offline (gray)
            }

            // Adjust position for each player
            float yOffset = i * 150f; // Adjust this value as needed
            playersListGO.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -yOffset);
        }
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

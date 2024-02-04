using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HatchedListManager : MonoBehaviour
{
    public GameObject content;

    [System.Serializable]
    public class HatchedInfo
    {
        public Sprite spriteHead;
        public string username;
        public string id;
    }

    public List<HatchedInfo> hatchedList;
    public GameObject hatchedInfoPrefab;
    public GameObject agentInfoPanel;

    public void localDisplayHatchedList()
    {
        List<HatchedInfo> dummyHatchedList = new List<HatchedInfo>();

        // Dummy player 1 (Online)
        HatchedInfo player1 = new HatchedInfo();
        player1.spriteHead = Resources.Load<Sprite>("Shapes/master_yoda_head");
        player1.username = "PlayerOne";
        player1.id = "1";
        dummyHatchedList.Add(player1);

        // Dummy player 2 (Offline)
        HatchedInfo player2 = new HatchedInfo();
        player2.spriteHead = Resources.Load<Sprite>("Shapes/george_washington_head");
        player2.username = "PlayerTwo";
        player2.id = "2";
        dummyHatchedList.Add(player2);

        // Dummy player 3 (Online)
        HatchedInfo player3 = new HatchedInfo();
        player3.spriteHead = Resources.Load<Sprite>("Shapes/user_head");
        player3.username = "PlayerThree";
        player3.id = "3";
        dummyHatchedList.Add(player3);

        // Dummy player 4 (Online)
        HatchedInfo player4 = new HatchedInfo();
        player4.spriteHead = Resources.Load<Sprite>("Shapes/master_yoda_head");
        player4.username = "PlayerFour";
        player4.id = "4";
        dummyHatchedList.Add(player4);

        // Dummy player 5 (Offline)
        HatchedInfo player5 = new HatchedInfo();
        player5.spriteHead = Resources.Load<Sprite>("Shapes/george_washington_head");
        player5.username = "PlayerFive";
        player5.id = "5";
        dummyHatchedList.Add(player5);

        // Dummy player 6 (Online)
        HatchedInfo player6 = new HatchedInfo();
        player6.spriteHead = Resources.Load<Sprite>("Shapes/user_head");
        player6.username = "PlayerSix";
        player6.id = "6";
        dummyHatchedList.Add(player6);
        

        for (int i = 0; i < dummyHatchedList.Count; i++)
        {
            HatchedInfo hatchedInfo = dummyHatchedList[i];
            GameObject hatchedListGO = Instantiate(hatchedInfoPrefab, content.transform);
            hatchedListGO.tag = "HatchedInfoPrefab";

            // Access child components directly
            HatchedInfoComponent hatchedInfoComponent = hatchedListGO.GetComponent<HatchedInfoComponent>();
            Image spriteHead = hatchedInfoComponent.displayUserImage; // Access the child Image component
            TextMeshProUGUI username = hatchedInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            hatchedInfoComponent.agentInfoPanel = agentInfoPanel;
            hatchedInfoComponent.hatchedId = hatchedInfo.id;

            // Set player details dynamically
            hatchedInfoComponent.SetPlayerDetails(hatchedInfo.spriteHead, hatchedInfo.username);


            // Adjust position for each player
            float yOffset = i * 150f; // Adjust this value as needed
            hatchedListGO.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -yOffset);
        }
    }

    // Deletes instantiations of the prefab that shows up on the hatched list when the panel is closed out
    public void CloseHatchedListPanel()
    {
        // Iterate through each child of the HatchedListPanel
        foreach (Transform child in content.transform)
        {
            // Check if the child has the specified tag
            if (child.gameObject.tag == "HatchedInfoPrefab")
            {
                // Destroy the prefab instances
                Destroy(child.gameObject);
            }
        }
    }
}

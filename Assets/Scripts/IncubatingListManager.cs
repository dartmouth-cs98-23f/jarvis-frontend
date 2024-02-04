using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IncubatingListManager : MonoBehaviour
{
    public GameObject content;

    [System.Serializable]
    public class IncubatingInfo
    {
        public Sprite spriteHead;
        public string username;
        public float totalDuration;
        public float timeRemaining;
    }

    public List<IncubatingInfo> incubatingList;
    public GameObject incubatingInfoPrefab;
    public GameObject trainAgentPanel;

    public void localDisplayIncubatingList()
    {
        List<IncubatingInfo> dummyIncubatingList = new List<IncubatingInfo>();

        // Dummy player 1
        IncubatingInfo player1 = new IncubatingInfo();
        player1.spriteHead = Resources.Load<Sprite>("Shapes/master_yoda_head");
        player1.username = "PlayerOne";
        player1.totalDuration = 10;
        player1.timeRemaining = 2;
        dummyIncubatingList.Add(player1);

        // Dummy player 2
        IncubatingInfo player2 = new IncubatingInfo();
        player2.spriteHead = Resources.Load<Sprite>("Shapes/george_washington_head");
        player2.username = "PlayerTwo";
        player2.totalDuration = 4;
        player2.timeRemaining = 0.5F;
        dummyIncubatingList.Add(player2);

        // Dummy player 3
        IncubatingInfo player3 = new IncubatingInfo();
        player3.spriteHead = Resources.Load<Sprite>("Shapes/user_head");
        player3.username = "PlayerThree";
        player3.totalDuration = 5;
        player3.timeRemaining = 4;
        dummyIncubatingList.Add(player3);

        // Dummy player 4
        IncubatingInfo player4 = new IncubatingInfo();
        player4.spriteHead = Resources.Load<Sprite>("Shapes/master_yoda_head");
        player4.username = "PlayerOne";
        player4.totalDuration = 6;
        player4.timeRemaining = 5.5F;
        dummyIncubatingList.Add(player4);

        // Dummy player 5
        IncubatingInfo player5 = new IncubatingInfo();
        player5.spriteHead = Resources.Load<Sprite>("Shapes/george_washington_head");
        player5.username = "PlayerTwo";
        player5.totalDuration = 2;
        player5.timeRemaining = 1;
        dummyIncubatingList.Add(player5);

        // Dummy player 6
        IncubatingInfo player6 = new IncubatingInfo();
        player6.spriteHead = Resources.Load<Sprite>("Shapes/user_head");
        player6.username = "PlayerThree";
        player6.totalDuration = 6;
        player6.timeRemaining = 2;
        dummyIncubatingList.Add(player6);
        

        for (int i = 0; i < dummyIncubatingList.Count; i++)
        {
            IncubatingInfo incubatingInfo = dummyIncubatingList[i];
            GameObject incubatingListGO = Instantiate(incubatingInfoPrefab, content.transform);
            incubatingListGO.tag = "IncubatingInfoPrefab";

            // Access child components directly
            IncubatingInfoComponent incubatingInfoComponent = incubatingListGO.GetComponent<IncubatingInfoComponent>();
            Image spriteHead = incubatingInfoComponent.displayUserImage; // Access the child Image component
            TextMeshProUGUI username = incubatingInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            incubatingInfoComponent.SetMaxValue(incubatingInfo.totalDuration);
            incubatingInfoComponent.SetProgress(incubatingInfo.totalDuration - incubatingInfo.timeRemaining);
            incubatingInfoComponent.trainAgentPanel = trainAgentPanel;


            // Set player details dynamically
            incubatingInfoComponent.SetPlayerDetails(incubatingInfo.spriteHead, incubatingInfo.username);


            // Adjust position for each player
            float yOffset = i * 150f; // Adjust this value as needed
            incubatingListGO.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -yOffset);
        }
    }

    // Deletes instantiations of the prefab that shows up on the incubating list when the panel is closed out
    public void CloseIncubatingListPanel()
    {
        // Iterate through each child of the IncubatingListPanel
        foreach (Transform child in content.transform)
        {
            // Check if the child has the specified tag
            if (child.gameObject.tag == "IncubatingInfoPrefab")
            {
                // Destroy the prefab instances
                Destroy(child.gameObject);
            }
        }
    }
}

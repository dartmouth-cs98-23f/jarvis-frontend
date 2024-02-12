using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

public class HatchedListManager : MonoBehaviour
{
    public GameObject content;
    public GameObject hatchedInfoPrefab;
    public GameObject agentInfoPanel;
    private HTTPClient httpClient = HTTPClient.Instance;
    public SpriteLoader spriteLoader;
    public SideMenu sideMenuManager;

    public async void localDisplayHatchedList()
    {
        string hatchedIdsJson = @"
        [
            {
                ""id"": ""11111111-1111-1111-1111-111111111111"",
                ""hatchTime"": ""2024-01-01T00:01:00Z""
            },
            {
                ""id"": ""22222222-2222-2222-2222-222222222222"",
                ""hatchTime"": ""2024-01-01T00:01:00Z""
            },
            {
                ""id"": ""33333333-3333-3333-3333-333333333333"",
                ""hatchTime"": ""2024-01-01T00:01:00Z""
            },
            {
                ""id"": ""44444444-4444-4444-4444-444444444444"",
                ""hatchTime"": ""2024-01-01T00:01:00Z""
            },
            {
                ""id"": ""55555555-5555-5555-5555-555555555555"",
                ""hatchTime"": ""2024-01-01T00:01:00Z""
            },
            {
                ""id"": ""66666666-6666-6666-6666-666666666666"",
                ""hatchTime"": ""2024-01-01T00:01:00Z""
            }
        ]";

        string agent1 = @"
        {
            ""id"": ""11111111-1111-1111-1111-111111111111"",
            ""username"": ""John"",
            ""description"": ""Description 1"",
            ""summary"": ""Summary 1"",
            ""location"": {
                ""x_coord"": 10,
                ""y_coord"": 20
            },
            ""creatorId"": ""22222222-2222-2222-2222-222222222222"",
            ""isHatched"": false,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2023-11-04T22:54:19.911Z"",
            ""hatchTime"": ""2023-12-04T22:54:19.911Z""
        }
        ";

        string agent2 = @"
        {
            ""id"": ""33333333-3333-3333-3333-333333333333"",
            ""username"": ""Alice"",
            ""description"": ""Description 2"",
            ""summary"": ""Summary 2"",
            ""location"": {
                ""x_coord"": 30,
                ""y_coord"": 40
            },
            ""creatorId"": ""44444444-4444-4444-4444-444444444444"",
            ""isHatched"": true,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2023-11-05T22:54:19.911Z"",
            ""hatchTime"": ""2023-12-05T22:54:19.911Z""
        }
        ";

        string agent3 = @"
        {
            ""id"": ""55555555-5555-5555-5555-555555555555"",
            ""username"": ""Bob"",
            ""description"": ""Description 3"",
            ""summary"": ""Summary 3"",
            ""location"": {
                ""x_coord"": 50,
                ""y_coord"": 60
            },
            ""creatorId"": ""66666666-6666-6666-6666-666666666666"",
            ""isHatched"": false,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2023-11-06T22:54:19.911Z"",
            ""hatchTime"": ""2023-12-06T22:54:19.911Z""
        }
        ";

        string agent4 = @"
        {
            ""id"": ""77777777-7777-7777-7777-777777777777"",
            ""username"": ""Sarah"",
            ""description"": ""Description 4"",
            ""summary"": ""Summary 4"",
            ""location"": {
                ""x_coord"": 70,
                ""y_coord"": 80
            },
            ""creatorId"": ""88888888-8888-8888-8888-888888888888"",
            ""isHatched"": true,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2023-11-07T22:54:19.911Z"",
            ""hatchTime"": ""2023-12-07T22:54:19.911Z""
        }
        ";

        string agent5 = @"
        {
            ""id"": ""99999999-9999-9999-9999-999999999999"",
            ""username"": ""Emily"",
            ""description"": ""Description 5"",
            ""summary"": ""Summary 5"",
            ""location"": {
                ""x_coord"": 90,
                ""y_coord"": 100
            },
            ""creatorId"": ""00000000-0000-0000-0000-000000000000"",
            ""isHatched"": false,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2023-11-08T22:54:19.911Z"",
            ""hatchTime"": ""2023-12-08T22:54:19.911Z""
        }
        ";

        string agent6 = @"
        {
            ""id"": ""10101010-1010-1010-1010-101010101010"",
            ""username"": ""David"",
            ""description"": ""Description 6"",
            ""summary"": ""Summary 6"",
            ""location"": {
                ""x_coord"": 110,
                ""y_coord"": 120
            },
            ""creatorId"": ""11111111-1111-1111-1111-111111111111"",
            ""isHatched"": true,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2023-11-09T22:54:19.911Z"",
            ""hatchTime"": ""2023-12-09T22:54:19.911Z""
        }
        ";

        Dictionary<Guid, string> myDictionary = new Dictionary<Guid, string>();

        myDictionary[new Guid("11111111-1111-1111-1111-111111111111")] = agent1;
        myDictionary[new Guid("22222222-2222-2222-2222-222222222222")] = agent2;
        myDictionary[new Guid("33333333-3333-3333-3333-333333333333")] = agent3;
        myDictionary[new Guid("44444444-4444-4444-4444-444444444444")] = agent4;
        myDictionary[new Guid("55555555-5555-5555-5555-555555555555")] = agent5;
        myDictionary[new Guid("66666666-6666-6666-6666-666666666666")] = agent6;

        List<HTTPClient.HatchedData> dummyHatchedList = JsonConvert.DeserializeObject<List<HTTPClient.HatchedData>>(hatchedIdsJson);

        for (int i = 0; i < dummyHatchedList.Count; i++)
        {
            HTTPClient.HatchedData hatchedId = dummyHatchedList[i];
            string hatchedString = myDictionary[hatchedId.id];
            HTTPClient.AgentData hatchedInfo = JsonConvert.DeserializeObject<HTTPClient.AgentData>(hatchedString);
            GameObject hatchedListGO = Instantiate(hatchedInfoPrefab, content.transform);
            hatchedListGO.tag = "HatchedInfoPrefab";

            // Access child components directly
            HatchedInfoComponent hatchedInfoComponent = hatchedListGO.GetComponent<HatchedInfoComponent>();
            Image spriteHead = hatchedInfoComponent.displayUserImage; // Access the child Image component
            TextMeshProUGUI username = hatchedInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            hatchedInfoComponent.agentInfoPanel = agentInfoPanel;

            // Call the LoadSprite method with the desired URL
            spriteLoader.LoadSprite(hatchedInfo.sprite_headshot_URL, (sprite) => {

            // Set player details dynamically
                hatchedInfoComponent.SetPlayerDetails(sprite, hatchedInfo.username);
            });
        }
        sideMenuManager.ToggleHatchedPanel();
    }

    public async void DisplayHatchedList(){
        List<HTTPClient.HatchedData> hatchedIds = await httpClient.GetHatched(httpClient.WorldId);

        if (hatchedIds == null)
        {
            Debug.Log("Hatched Ids list is null");
        }

        foreach (HTTPClient.HatchedData hatchedData in hatchedIds)
        {
            HTTPClient.AgentData agentData = await httpClient.GetAgent(hatchedData.id);

            if (agentData == null)
            {
                Debug.Log("Agent Data is null");
            }

            GameObject hatchedListGO = Instantiate(hatchedInfoPrefab, content.transform);
            hatchedListGO.tag = "HatchedInfoPrefab";

            // Access child components directly
            HatchedInfoComponent hatchedInfoComponent = hatchedListGO.GetComponent<HatchedInfoComponent>();
            Image spriteHead = hatchedInfoComponent.displayUserImage; // Access the child Image component
            TextMeshProUGUI username = hatchedInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            hatchedInfoComponent.agentInfoPanel = agentInfoPanel;
            hatchedInfoComponent.hatchedId = hatchedData.id;

            // Call the LoadSprite method with the desired URL
            spriteLoader.LoadSprite(agentData.sprite_headshot_URL, (sprite) => {

            // Set player details dynamically
                hatchedInfoComponent.SetPlayerDetails(sprite, agentData.username);
            });
        }
        sideMenuManager.ToggleHatchedPanel();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using Clients;

public class IncubatingListManager : MonoBehaviour
{
    public GameObject content;
    public GameObject incubatingInfoPrefab;
    public GameObject trainAgentPanel;
    public SpriteLoader spriteLoader;
    private HTTPClient httpClient = HTTPClient.Instance;
    public TrainAgentManager trainAgentManager;

    public void localDisplayIncubatingList()
    {
        string incubatingIdsJson = @"
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
            ""createdTime"": ""2024-02-04T22:54:19.911Z"",
            ""hatchTime"": ""2024-02-11T22:54:19.911Z""
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
            ""isHatched"": false,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2024-02-04T22:54:19.911Z"",
            ""hatchTime"": ""2024-02-11T22:54:19.911Z""
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
            ""createdTime"": ""2024-02-04T22:54:19.911Z"",
            ""hatchTime"": ""2024-02-11T22:54:19.911Z""
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
            ""isHatched"": false,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2024-02-04T22:54:19.911Z"",
            ""hatchTime"": ""2024-02-11T22:54:19.911Z""
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
            ""createdTime"": ""2024-02-04T22:54:19.911Z"",
            ""hatchTime"": ""2024-02-11T22:54:19.911Z""
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
            ""isHatched"": false,
            ""sprite_URL"": ""https://picsum.photos/200"",
            ""sprite_headshot_URL"": ""https://picsum.photos/200"",
            ""createdTime"": ""2024-02-04T22:54:19.911Z"",
            ""hatchTime"": ""2024-02-11T22:54:19.911Z""
        }
        ";

        Dictionary<Guid, string> myDictionary = new Dictionary<Guid, string>();

        myDictionary[new Guid("11111111-1111-1111-1111-111111111111")] = agent1;
        myDictionary[new Guid("22222222-2222-2222-2222-222222222222")] = agent2;
        myDictionary[new Guid("33333333-3333-3333-3333-333333333333")] = agent3;
        myDictionary[new Guid("44444444-4444-4444-4444-444444444444")] = agent4;
        myDictionary[new Guid("55555555-5555-5555-5555-555555555555")] = agent5;
        myDictionary[new Guid("66666666-6666-6666-6666-666666666666")] = agent6;

        List<HTTPClient.IncubatingData> dummyIncubatingList = JsonConvert.DeserializeObject<List<HTTPClient.IncubatingData>>(incubatingIdsJson);
        

        for (int i = 0; i < dummyIncubatingList.Count; i++)
        {
            HTTPClient.IncubatingData incubatingId = dummyIncubatingList[i];
            string incubatingString = myDictionary[incubatingId.id];
            HTTPClient.AgentData incubatingInfo = JsonConvert.DeserializeObject<HTTPClient.AgentData>(incubatingString);
            GameObject incubatingListGO = Instantiate(incubatingInfoPrefab, content.transform);
            incubatingListGO.tag = "IncubatingInfoPrefab";

            // Access child components directly
            IncubatingInfoComponent incubatingInfoComponent = incubatingListGO.GetComponent<IncubatingInfoComponent>();
            Image spriteHead = incubatingInfoComponent.displayUserImage; // Access the child Image component
            TextMeshProUGUI username = incubatingInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            incubatingInfoComponent.trainAgentManager = trainAgentManager;
            incubatingInfoComponent.incubatingId = incubatingId.id;
            TimeSpan total = incubatingInfo.hatchTime - incubatingInfo.createdTime;
            TimeSpan remaining = incubatingInfo.hatchTime - DateTime.UtcNow;
            double totalHours = total.TotalHours;
            double remainingHours = remaining.TotalHours;

            incubatingInfoComponent.SetMaxValue((float)totalHours);
            incubatingInfoComponent.SetProgress((float)(totalHours - remainingHours));
            incubatingInfoComponent.trainAgentPanel = trainAgentPanel;


            // Call the LoadSprite method with the desired URL
            spriteLoader.LoadSprite(incubatingInfo.sprite_headshot_URL, (sprite) => {

            // Set player details dynamically
                incubatingInfoComponent.SetPlayerDetails(sprite, incubatingInfo.username);
            });
        }
    }

    public async void DisplayIncubatingList(){
        List<HTTPClient.IncubatingData> incubatingIds = await httpClient.GetIncubating(httpClient.WorldId);

        if (incubatingIds == null)
        {
            Debug.Log("Incubating Ids list is null");
        }

        foreach (HTTPClient.IncubatingData incubatingData in incubatingIds)
        {
            HTTPClient.AgentData agentData = await httpClient.GetAgent(incubatingData.id);

            if (agentData == null)
            {
                Debug.Log("Agent Data is null");
            }

            GameObject incubatingListGO = Instantiate(incubatingInfoPrefab, content.transform);
            incubatingListGO.tag = "IncubatingInfoPrefab";

            // Access child components directly
            IncubatingInfoComponent incubatingInfoComponent = incubatingListGO.GetComponent<IncubatingInfoComponent>();
            Image spriteHead = incubatingInfoComponent.displayUserImage; // Access the child Image component
            TextMeshProUGUI username = incubatingInfoComponent.usernameTMP; // Access the child TextMeshProUGUI component
            incubatingInfoComponent.trainAgentPanel = trainAgentPanel;
            incubatingInfoComponent.trainAgentManager = trainAgentManager;
            incubatingInfoComponent.incubatingId = incubatingData.id;

            TimeSpan total = agentData.hatchTime - agentData.createdTime;
            TimeSpan remaining = agentData.hatchTime - DateTime.UtcNow;
            double totalHours = total.TotalHours;
            double remainingHours = remaining.TotalHours;

            incubatingInfoComponent.SetMaxValue((float)totalHours);
            incubatingInfoComponent.SetProgress((float)(totalHours - remainingHours));

            // Call the LoadSprite method with the desired URL
            spriteLoader.LoadSprite(agentData.sprite_headshot_URL, (sprite) => {

            // Set player details dynamically
                incubatingInfoComponent.SetPlayerDetails(sprite, agentData.username);
            });
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Clients;
using Newtonsoft.Json;

public class AgentInfoManager : MonoBehaviour
{
    public GameObject agentInfoPanel;
    public GameObject agentInfoSprite;
    public GameObject agentInfoName;
    public GameObject agentInfoCreatedBy;
    public GameObject agentInfoCreatedOn;
    public GameObject agentInfoSummary;
    private Guid agentId;
    private HTTPClient httpClient = HTTPClient.Instance;
    public SpriteLoader spriteLoader;

    // Method to set the agent ID and fetch agent data
    public void SetAgentID(Guid id)
    {
        agentId = id;
        FillAgentInfoFields();
        // FetchAgentInfo(); // Fetch agent data based on the ID, uncomment when testing with backend, and comment line above
    }

    // Local testing method
    public void FillAgentInfoFields(){
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

        HTTPClient.AgentData agentOb1 = JsonConvert.DeserializeObject<HTTPClient.AgentData>(agent1);
        HTTPClient.AgentData agentOb2 = JsonConvert.DeserializeObject<HTTPClient.AgentData>(agent2);
        HTTPClient.AgentData agentOb3 = JsonConvert.DeserializeObject<HTTPClient.AgentData>(agent3);
        HTTPClient.AgentData agentOb4 = JsonConvert.DeserializeObject<HTTPClient.AgentData>(agent4);
        HTTPClient.AgentData agentOb5 = JsonConvert.DeserializeObject<HTTPClient.AgentData>(agent5);
        HTTPClient.AgentData agentOb6 = JsonConvert.DeserializeObject<HTTPClient.AgentData>(agent6);

        Dictionary<Guid, HTTPClient.AgentData> myDictionary = new Dictionary<Guid, HTTPClient.AgentData>();

        myDictionary[new Guid("11111111-1111-1111-1111-111111111111")] = agentOb1;
        myDictionary[new Guid("22222222-2222-2222-2222-222222222222")] = agentOb2;
        myDictionary[new Guid("33333333-3333-3333-3333-333333333333")] = agentOb3;
        myDictionary[new Guid("44444444-4444-4444-4444-444444444444")] = agentOb4;
        myDictionary[new Guid("55555555-5555-5555-5555-555555555555")] = agentOb5;
        myDictionary[new Guid("66666666-6666-6666-6666-666666666666")] = agentOb6;

        HTTPClient.AgentData curr = myDictionary[agentId];

        agentInfoName.GetComponent<TextMeshProUGUI>().text = curr.username;
        agentInfoCreatedBy.GetComponent<TextMeshProUGUI>().text = "Created by: " + "Evan Phillips";
        agentInfoCreatedOn.GetComponent<TextMeshProUGUI>().text = "Created on: " + curr.createdTime.ToString("MM/dd/yy");
        agentInfoSummary.GetComponent<TextMeshProUGUI>().text = "Summary:\n" + curr.summary;

        // Call the LoadSprite method with the desired URL
        spriteLoader.LoadSprite(curr.sprite_headshot_URL, (sprite) => {

                agentInfoSprite.GetComponent<Image>().sprite = sprite;
            });
            agentInfoPanel.SetActive(true);
        }
    
    public async void FetchAgentInfo(){
        HTTPClient.AgentData agent = await httpClient.GetAgent(agentId);
        httpClient.UserData creator = await httpClient.GetUser(agent.creatorId);

        agentInfoName.GetComponent<TextMeshProUGUI>().text = agent.username;
        agentInfoCreatedBy.GetComponent<TextMeshProUGUI>().text = "Created by: " + creator.username;
        agentInfoCreatedOn.GetComponent<TextMeshProUGUI>().text = "Created on: " + agent.createdTime.ToString("MM/dd/yy");
        agentInfoSummary.GetComponent<TextMeshProUGUI>().text = "Summary:\n" + agent.summary;

        // Call the LoadSprite method with the desired URL
        spriteLoader.LoadSprite(curr.sprite_headshot_URL, (sprite) => {

                agentInfoSprite.GetComponent<Image>().sprite = sprite;
            });
            agentInfoPanel.SetActive(true);
        }
    }

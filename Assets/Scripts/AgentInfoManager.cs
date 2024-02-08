using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class AgentInfoManager : MonoBehaviour
{
    public GameObject agentInfoPanel;
    public GameObject agentInfoSprite;
    public GameObject agentInfoName;
    public GameObject agentInfoCreatedBy;
    public GameObject agentInfoCreatedOn;
    public GameObject agentInfoSummary;

    [Serializable]
    public class AgentData
    {
        public string id;
        public string username;
        public string description;
        public string summary;
        public LocationData location;
        public string creatorId;
        public bool isHatched;
        public string sprite_URL;
        public string sprite_headshot_URL;
        public DateTime createdTime;
        public DateTime hatchTime;
    }

    [Serializable]
    public class LocationData
    {
        public int x_coord;
        public int y_coord;
    }
    
    public void FillAgentInfoFields(){
        AgentData agent = new AgentData();
        agent.sprite_headshot_URL = "Shapes/master_yoda_head";
        agent.username = "Master Yoda";
        agent.creatorId = "00000000-0000-0000-0000-000000000000"; // Should call get user with this id to display creator name
        agent.createdTime = DateTime.Parse("2023-11-04T22:54:19.911Z");
        agent.summary = "Master Yoda is a jedi beast. Now let's spam some text to test the scroll size. Now let's spam some text to test the scroll size. Now let's spam some text to test the scroll size. Now let's spam some text to test the scroll size. Now let's spam some text to test the scroll size.";

        agentInfoSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>(agent.sprite_headshot_URL);
        agentInfoName.GetComponent<TextMeshProUGUI>().text = agent.username;
        agentInfoCreatedBy.GetComponent<TextMeshProUGUI>().text = "Created by: " + "Evan Phillips";
        agentInfoCreatedOn.GetComponent<TextMeshProUGUI>().text = "Created on: " + agent.createdTime.ToString("MM/dd/yy");
        agentInfoSummary.GetComponent<TextMeshProUGUI>().text = "Summary:\n" + agent.summary;
    }

    private void Update(){
        if (agentInfoPanel.activeSelf){
            FillAgentInfoFields();
        }
    }
}

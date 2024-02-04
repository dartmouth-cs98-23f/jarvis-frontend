using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class TrainAgentManager : MonoBehaviour
{
    public GameObject trainAgentPanel;
    public GameObject trainAgentSprite;
    public GameObject trainAgentName;
    public GameObject trainAgentSlider;
    public GameObject trainAgentTimeRemaining;
    public GameObject trainAgentDesc;

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
    
    public void FillTrainAgentFields(){
        AgentData agent = new AgentData();
        agent.sprite_headshot_URL = "Shapes/master_yoda_head";
        agent.username = "Master Yoda";
        agent.creatorId = "00000000-0000-0000-0000-000000000000"; // Should call get user with this id to display creator name
        agent.createdTime = DateTime.Parse("2024-02-04T02:54:19.911Z");
        agent.hatchTime = DateTime.Parse("2024-02-11T12:54:19.911Z");
        agent.description = "Master Yoda is that mf boy.";

        trainAgentSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>(agent.sprite_headshot_URL);
        trainAgentName.GetComponent<TextMeshProUGUI>().text = agent.username;
        trainAgentDesc.GetComponent<TextMeshProUGUI>().text = agent.description;

        TimeSpan total = agent.hatchTime - agent.createdTime;
        TimeSpan remaining = agent.hatchTime - DateTime.Now;
        double totalHours = total.TotalHours;
        double remainingHours = remaining.TotalHours;
        int hours = (int)remainingHours;
        int minutes = (int)((remainingHours - hours) * 60); // to calculate minutes to display in the tex box

        trainAgentTimeRemaining.GetComponent<TextMeshProUGUI>().text = $"Time Remaining: {hours}h {minutes}m";
        trainAgentSlider.GetComponent<Slider>().maxValue = (float)totalHours;
        trainAgentSlider.GetComponent<Slider>().value = (float)(totalHours - remainingHours);
        
    }

    private void Update(){
        if (trainAgentPanel.activeSelf){
            FillTrainAgentFields();
        }
    }
}

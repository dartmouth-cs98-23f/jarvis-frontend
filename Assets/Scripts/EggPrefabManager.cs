using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Clients;

public class EggPrefabManager : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI time; 
    public DateTime hatchTime;
    public double totalHours;
    private bool isSet = false;
    private bool hasBuiltAgent;
    public TextMeshProUGUI name;
    private HTTPClient httpClient = HTTPClient.Instance;
    public HTTPClient.AgentData agent;

    public void SetEggDetails(DateTime hatch, DateTime create, string username){
        hatchTime = hatch;

        TimeSpan total = hatchTime - create;
        totalHours = total.TotalHours;
        slider.maxValue = (float)totalHours;
        name.text = username;

        isSet = true;
        
    }
    // Update is called once per frame
    async void Start(){
        Guid agentId = gameObject.GetComponent<CharacterComponent>().GetCharacterId();
        agent = await httpClient.GetAgent(agentId);
        hasBuiltAgent = false;
    }
    void Update()
    {
        if (isSet && !hasBuiltAgent){
            TimeSpan remaining = hatchTime - DateTime.UtcNow;
            if (remaining < TimeSpan.Zero)
            {
                Debug.Log("Entered here with hatchTime " + hatchTime);
                GameObject gameClientGO = GameObject.Find("GameClient");
                gameClientGO.GetComponent<GameClient>().BuildAgent(agent);
                gameObject.SetActive(false);
                Destroy(this);
                hasBuiltAgent = true; // Mark that the agent has been built
                }
            double remainingHours = remaining.TotalHours;
            int hours = (int)remainingHours;
            int minutes = (int)((remainingHours - hours) * 60); // to calculate minutes to display in the text box

            time.text = $"{hours}h {minutes}min";
            slider.value = (float)(totalHours - remainingHours);
        }
    }
}

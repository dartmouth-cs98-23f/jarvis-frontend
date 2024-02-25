using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Clients;


public class HatchedInfoComponent : MonoBehaviour
{
    public TextMeshProUGUI usernameTMP;
    public GameObject agentInfoPanel;
    public AgentInfoManager agentInfoManager;
    public Guid hatchedId;
    public GameObject spriteHeadshotPrefab;


    // A method to set the player details
    public void SetPlayerDetails(string username)
    {
        usernameTMP.text = username;
    }

    public void ClickPrefab()
    {
        agentInfoManager.SetAgentID(hatchedId);
    }
}

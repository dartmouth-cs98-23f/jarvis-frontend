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
    public Image displayUserImage;
    public GameObject agentInfoPanel;
    public AgentInfoManager agentInfoManager;
    public Guid hatchedId;


    // A method to set the player details
    public void SetPlayerDetails(Sprite profileImage, string username)
    {
        displayUserImage.sprite = profileImage;
        Color color = displayUserImage.color;
        color.a = 1;
        displayUserImage.color = color;
        usernameTMP.text = username;
    }

    public void ClickPrefab()
    {
        agentInfoManager.SetAgentID(hatchedId);
    }
}

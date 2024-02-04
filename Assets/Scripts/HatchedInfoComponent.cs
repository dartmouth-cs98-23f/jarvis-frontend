using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class HatchedInfoComponent : MonoBehaviour
{
    public TextMeshProUGUI usernameTMP;
    public Image displayUserImage;
    public GameObject agentInfoPanel;
    public string hatchedId;


    // A method to set the player details
    public void SetPlayerDetails(Sprite profileImage, string username)
    {
        displayUserImage.sprite = profileImage;
        usernameTMP.text = username;
    }

    public void ClickPrefab()
    {
        // Toggle the panel visibility
        if (agentInfoPanel != null)
        {
            agentInfoPanel.SetActive(!agentInfoPanel.activeSelf);
        }
        else
        {
            Debug.LogError("Agent Info Panel reference is not set.");
        }
    }
}

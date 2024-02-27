using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerInfoComponent : MonoBehaviour
{
    public GameObject onlineIndicator;
    public TextMeshProUGUI usernameTMP;
    public GameObject owner;
    public GameObject kick;
    public GameObject kickPanel;
    public GameObject playersListPanel;
    public PlayersListManager playersListManager;
    public Guid playerId;
    public GameObject spriteHeadshotPrefab;

    // A method to set the player details
    public void SetPlayerDetails(string username)
    {
        usernameTMP.text = username;
    }

    public void IsOwner(){
        owner.SetActive(true);
        kick.SetActive(true);
    }

    public void OnKickPressed(){
        playersListPanel.SetActive(false);
        playersListManager.SetKickUsername(usernameTMP);
        playersListManager.SetPlayerID(playerId);
        kickPanel.SetActive(true);
    }
}

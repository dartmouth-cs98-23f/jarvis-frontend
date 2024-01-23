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
    public Image displayUserImage;

    // A method to set the player details
    public void SetPlayerDetails(bool isOnline, Sprite profileImage, string username)
    {
        // Set online/offline indicator
        // onlineIndicator.SetActive(isOnline);
        displayUserImage.sprite = profileImage;
        usernameTMP.text = username;
    }
}

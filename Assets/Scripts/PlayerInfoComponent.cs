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
    public void SetPlayerDetails(Sprite profileImage, string username)
    {
        displayUserImage.sprite = profileImage;
        Color color = displayUserImage.color;
        color.a = 1;
        displayUserImage.color = color;
        usernameTMP.text = username;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HatchedInfoComponent : MonoBehaviour
{
    public TextMeshProUGUI usernameTMP;
    public Image displayUserImage;

    // A method to set the player details
    public void SetPlayerDetails(Sprite profileImage, string username)
    {
        displayUserImage.sprite = profileImage;
        usernameTMP.text = username;
    }
}

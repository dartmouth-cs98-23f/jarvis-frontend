using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorldSettingsManager : MonoBehaviour
{
    public TextMeshProUGUI worldNameText;
    public TextMeshProUGUI worldDescText;
    public TextMeshProUGUI inviteCodeText;

    // For local testing
    public void localDisplayWorldName(){
        string name = "Evan's World";

        worldNameText.text = "Name: " + name;
    }

    // For local testing
    public void localDisplayWorldDesc(){
        string desc = "For users who love to partyyyy";

        worldDescText.text = "Description: " + desc;
    }

    // For local testing
    public void localDisplayInviteCode(){
        string code = "3ZD9LX";

        inviteCodeText.text = "Invite Code: " + code;
    }
}

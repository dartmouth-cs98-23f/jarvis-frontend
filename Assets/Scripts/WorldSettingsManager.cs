using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Clients;
using System;

public class WorldSettingsManager : MonoBehaviour
{
    public TextMeshProUGUI worldNameText;
    public TextMeshProUGUI worldDescText;
    public TextMeshProUGUI inviteCodeText;
    public Text namePlaceholder;
    public Text descPlaceholder;
    private HTTPClient httpClient = HTTPClient.Instance;
    public SideMenu sideMenuManager;

    // For local testing
    public void localDisplayWorldName(){
        string name = "Evan's World";

        worldNameText.text = "Name: " + name;
    }

    // For local testing
    public void localDisplayWorldDesc(){
        string desc = "For users who love to partyyyy";

        worldDescText.text = "Description: \n" + desc;
    }

    // For local testing
    public void localDisplayInviteCode(){
        string code = "3ZD9LX";

        inviteCodeText.text = "Invite Code: " + code;
    }

    public void localDisplayNamePlaceholder(){
        string placeholder = "Star Wars World";

        namePlaceholder.text = placeholder;
    }

    public void localDisplayDescPlaceholder(){
        string desc = "This world is for all Star Wars characters. All intruders shall die.";

        descPlaceholder.text = desc;
    }

    public async void OnButtonPressed(){
        HTTPClient.UserWorld worldInfo = await httpClient.GetWorld(httpClient.WorldId);
        Guid creator = worldInfo.creatorId;

        if (httpClient.MyId == creator){
            sideMenuManager.ToggleWorldSettingsPanel();
            worldNameText.text = "Name: " + worldInfo.name;
            worldDescText.text = "Description: \n" + worldInfo.description;
            inviteCodeText.text = "Invite Code: " + worldInfo.worldCode;
        }
        else{
            sideMenuManager.ToggleWorldSettingsPanel();
            worldNameText.text = "Name: " + worldInfo.name;
            worldDescText.text = "Description: \n" + worldInfo.description;
            inviteCodeText.text = "Invite Code: " + worldInfo.worldCode;
        }
    }
}

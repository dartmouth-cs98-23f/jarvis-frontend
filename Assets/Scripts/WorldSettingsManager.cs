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
        // HTTPClient.IdData creator = new HTTPClient.IdData();
        // creator.id = new Guid("a5e05db4-74c6-48ed-a561-b3a2e46397d5");
        if (httpClient.MyId == creator){
            // sideMenuManager.ToggleOwnerWorldSettingsPanel(); // TODO: Uncomment if we ever update the desc if the owner edits it
            sideMenuManager.ToggleWorldSettingsPanel();
            // namePlaceholder.text = worldInfo.name;
            // descPlaceholder.text = worldInfo.description;
            worldNameText.text = "Name: " + worldInfo.name;
            worldDescText.text = "Description: \n" + worldInfo.description;
            inviteCodeText.text = "Invite Code: " + worldInfo.worldCode;
            // localDisplayInviteCode();
            // localDisplayNamePlaceholder();
            // localDisplayDescPlaceholder();
        }
        else{
            sideMenuManager.ToggleWorldSettingsPanel();
            worldNameText.text = "Name: " + worldInfo.name;
            worldDescText.text = "Description: \n" + worldInfo.description;
            inviteCodeText.text = "Invite Code: " + worldInfo.worldCode;
            // localDisplayInviteCode();
            // localDisplayWorldDesc();
            // localDisplayWorldName();
        }
    }
}

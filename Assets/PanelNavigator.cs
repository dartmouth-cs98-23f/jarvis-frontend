using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clients;

public class PanelNavigator : MonoBehaviour
{
    public GameObject[] panels;
    private int currentPanelIndex = 0;

    private void Start()
    {
        if (panels.Length == 0)
        {
            Debug.Log("No panels found");
            return;
        }
        panels[0].SetActive(true);
        for (int i = 1; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    public void ShowNextPanel()
    {
        CheckSignalRConnection();
        if (currentPanelIndex < panels.Length - 1)
        {
            Debug.Log("show next panel");
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            panels[currentPanelIndex].SetActive(true);
        }
    }

    public void ShowPreviousPanel()
    {
        if (currentPanelIndex > 0)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            panels[currentPanelIndex].SetActive(true);
        }
    }

    // TODO: Remove after testing signalRClient
    async void CheckSignalRConnection(){
        if (SignalRClient.IsConnected())
        {
            Debug.Log("SignalRClient is connected in panel");
        }
        else
        {
            Debug.Log("SignalRClient is not connected in panel");
        }
    }
}
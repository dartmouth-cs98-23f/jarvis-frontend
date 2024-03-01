using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clients;
using System;

public class OnboardingPanelsManager : MonoBehaviour
{
    public GameObject LandingPanel;
    public GameObject InfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (HTTPClient.Instance.MyId != null && HTTPClient.Instance.MyId != Guid.Empty) // if user has already signed up
        {
            LandingPanel.SetActive(false);
            InfoPanel.SetActive(true);
        } else {
            LandingPanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

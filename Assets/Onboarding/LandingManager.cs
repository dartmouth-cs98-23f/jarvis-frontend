using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Clients;

public class LandingManager : MonoBehaviour
{
    public GameObject LandingPanel;
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    public async void OnLoginButtonPressed()
    {   
        LandingPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public async void OnSignUpButtonPressed()
    {   
        LandingPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

}

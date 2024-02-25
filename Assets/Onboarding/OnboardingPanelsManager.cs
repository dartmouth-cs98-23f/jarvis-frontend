using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingPanelsManager : MonoBehaviour
{
    public GameObject LandingPanel;
    public GameObject InfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("SkipToIntroQuestions") == "true")
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

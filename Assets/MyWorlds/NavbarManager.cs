using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavbarManager : MonoBehaviour
{
    public GameObject myWorldsPanel;
    public GameObject userIcon;
    public GameObject backIcon;

    public GameObject currentPanel;
    public GameObject userProfilePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPressUserButton()
    {
        myWorldsPanel.SetActive(false);
        userProfilePanel.SetActive(true);
        userIcon.SetActive(false);
        backIcon.SetActive(true);
    }

    public void OnPressBackButton()
    {
        myWorldsPanel.SetActive(true);
        userIcon.SetActive(true);
        backIcon.SetActive(false);
    }


}

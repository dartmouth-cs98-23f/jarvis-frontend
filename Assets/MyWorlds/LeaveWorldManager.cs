using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveWorldManager : MonoBehaviour
{

    public GameObject currentWorldObject;
    public Text leaveWorldText;
    private ImageSwiper worldSwiper;

    public GameObject leaveWorldPanel;
    public GameObject myWorldsPanel;
    private MyWorldsManager myWorldsManager;
    private void OnEnable()
    {
        worldSwiper = currentWorldObject.GetComponent<ImageSwiper>();
        myWorldsManager = myWorldsPanel.GetComponent<MyWorldsManager>();
        leaveWorldText.text = "Are you sure you want to leave " + worldSwiper.GetCurrentWorldName() + "?";
    }

    public void OnPressLeave()
    {
        myWorldsManager.LeaveWorld(worldSwiper.GetCurrentWorldId());
        leaveWorldPanel.SetActive(false);
    }

    public void OnPressCancel()
    {
        leaveWorldPanel.SetActive(false);
    }
}

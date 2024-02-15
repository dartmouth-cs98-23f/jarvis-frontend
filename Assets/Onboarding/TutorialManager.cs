using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public void OnPressSkip()
    {
        tutorialPanel.SetActive(false);
        SceneNavigator.LoadMyWorlds();
    }
}

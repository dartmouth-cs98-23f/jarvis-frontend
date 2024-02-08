using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TrainAgentDescModifier : MonoBehaviour
{
    public GameObject trainAgentContent;
    public GameObject trainAgentDesc;

    // This method will be called when the panel becomes active
    private void Update()
    {
        float preferredHeight = trainAgentDesc.GetComponent<TextMeshProUGUI>().preferredHeight;
        RectTransform contentRectTransform = trainAgentContent.GetComponent<RectTransform>();

        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, preferredHeight);
    }
}

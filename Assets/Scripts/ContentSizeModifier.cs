using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ContentSizeModifier : MonoBehaviour
{
    public GameObject content;
    public GameObject desc;

    // This method will be called when the panel becomes active
    private void Update()
    {
        float preferredHeight = desc.GetComponent<TextMeshProUGUI>().preferredHeight;
        RectTransform contentRectTransform = content.GetComponent<RectTransform>();

        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, preferredHeight);
    }
}

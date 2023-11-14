using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{

    public InputField inputField; // Reference to the InputField
    public Button targetButton; // Reference to the Button

    void Start()
    {
        // Add listener to the InputField to detect changes in the text
        inputField.onValueChanged.AddListener(OnInputFieldChanged);
    }

    private void OnInputFieldChanged(string text)
    {
        // Check if there is text in the InputField
        if (!string.IsNullOrEmpty(text))
        {
            // Change the button color value (V in HSV) when there is text
            SetButtonColorValue(1.0f); // Full value (you can adjust this)
        }
        else
        {
            // Reset the button color value when there is no text
            SetButtonColorValue(0.5f); // Half value (or any other default value)
        }
    }

    private void SetButtonColorValue(float value)
    {
        Color currentColor = targetButton.image.color;
        Color.RGBToHSV(currentColor, out float H, out float S, out float V);
        V = value; // Set the new Value
        targetButton.image.color = Color.HSVToRGB(H, S, V);
    }
}

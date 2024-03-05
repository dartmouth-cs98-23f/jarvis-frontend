using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonClickHandler : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        // Add a listener for the button click event
        button.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        // Disable the button to prevent multiple clicks
        button.interactable = false;

        // Start a coroutine to handle the backend operation
        StartCoroutine(BackendOperation());
    }

    IEnumerator BackendOperation()
    {
        // Simulate backend operation delay
        yield return new WaitForSeconds(3.0f);

        // Once the operation is complete, re-enable the button
        button.interactable = true;
    }
}

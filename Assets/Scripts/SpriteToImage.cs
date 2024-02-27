using UnityEngine;
using UnityEngine.UI;

public class SpriteToImage : MonoBehaviour
{
    // Reference to the Sprite Renderer component
    public SpriteRenderer spriteRenderer;

    // Reference to the Image component
    public Image imageComponent;

    void Update()
    {
        // Check if both components are assigned
        if (spriteRenderer != null && imageComponent != null)
        {
            // Get the sprite from the Sprite Renderer component
            Sprite sprite = spriteRenderer.sprite;

            // Set the sprite as the source image of the Image component
            imageComponent.sprite = sprite;
        }
        else
        {
            Debug.LogError("SpriteRenderer or Image component not assigned!");
        }
    }
}

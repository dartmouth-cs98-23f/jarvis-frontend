using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    // TODO: Generate by sprite
    // public Sprite userSprite;

    // A method to set the chat details
    public void SetPosition(int x, int y, int z)
    {
        transform.position = new Vector3(x, y, z);
    }
}

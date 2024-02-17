using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterComponent : MonoBehaviour
{
    // TODO: Generate by sprite
    // public Sprite userSprite;
    public Guid userId;

    // A method to set the chat details
    public void SetPosition(int x, int y, int z)
    {
        transform.position = new Vector3(x, y, z);
    }

    public void SetUserId(Guid userId)
    {
        this.userId = userId;
    }

    public Guid GetUserId()
    {
        return userId;
    }
}

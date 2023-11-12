using UnityEngine;
using System;

public class NPCCharacter : MonoBehaviour
{
    // User-specific information
    // TODO: Make sure this is the correct user id
    public Guid userId = new Guid("375d96a5-492d-43a0-af8c-6db76ce341d3");

    // Method to get the user ID
    public Guid GetUserId()
    {
        return userId;
    }
}

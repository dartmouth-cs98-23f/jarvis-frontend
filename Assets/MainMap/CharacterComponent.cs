using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterComponent : MonoBehaviour
{
    // TODO: Generate by sprite
    // public Sprite userSprite;
    public Guid characterId;
    public string characterType;

    // A method to set the chat details
    public void SetPosition(int x, int y, int z)
    {
        transform.position = new Vector3(x, y, z);
    }

    public void SetCharacterId(Guid characterId)
    {
        this.characterId = characterId;
    }

    public Guid GetCharacterId()
    {
        return characterId;
    }

    public void SetCharacterType(string characterType)
    {
        if (characterType == "user" || characterType == "agent" || characterType == "egg")
        {
            this.characterType = characterType;
        } else 
        {
            throw new ArgumentException("Invalid character type");
        }
    }

    public string GetCharacterType()
    {
        return characterType;
    }
}

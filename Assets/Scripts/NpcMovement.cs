using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class NpcMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnCollisionEnter2D(Collision2D collision){
        Guid tempId = new Guid("375d96a5-492d-43a0-af8c-6db76ce341d3");
        Debug.Log("Ran into NPC!");
        SceneManager.LoadScene("Chat");
    }
}

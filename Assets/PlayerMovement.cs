using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clients;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Animator animator;
    private float moveSpeed = 5.0f;
    private Guid collidedUserId;

    public GameObject InteractButton;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    private async Task MovePlayerByClick()
    {
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        // Clicked on a UI element, do not move the player
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }
        
        if (touch.phase == TouchPhase.Began)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(touch.position);
            
        }
    }

    // This fixes the player automatically going to 0,0 on start
    if (targetPosition.x == 0f && targetPosition.y == 0f)
    {
        rb.velocity = Vector2.zero;
        return;
    }

    Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

    // Calculate the absolute values of movement in x and y directions
    float absMoveX = Mathf.Abs(direction.x);
    float absMoveY = Mathf.Abs(direction.y);

    // Determine which animation to play based on the movement direction
    if (absMoveY > absMoveX)
    {
        // Play walk forward animation
        animator.SetFloat("MoveX", 0f);
        animator.SetFloat("MoveY", direction.y);
    }
    else
    {
        // Play walk right animation
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", 0f);
    }

    // Check if the distance between the player and the target is greater than the stopping distance
    if (Vector2.Distance(targetPosition, transform.position) > 0.1f)
    {
        // Move the player towards the target position
        rb.velocity = new Vector2(Mathf.Round(direction.x * moveSpeed), Mathf.Round(direction.y * moveSpeed));

        // Get the current player position
        int xCoordinate = Mathf.RoundToInt(transform.position.x);
        int yCoordinate = Mathf.RoundToInt(transform.position.y);

        // Send the updated location to the server
        // TODO: Replace with code below
        // PlayerPrefs.SetString("lastKnownX", xCoordinate.ToString());
        // PlayerPrefs.SetString("lastKnownY", yCoordinate.ToString()); 
        
        await SignalRClient.Instance.UpdateLocation(xCoordinate, yCoordinate);
    }
    else
    {
        // If the player is within the stopping distance, stop its movement and animation
        rb.velocity = Vector2.zero;
        animator.SetFloat("MoveX", 0.001f);
        animator.SetFloat("MoveY", 0.001f);
    }
}

    void OnCollisionEnter2D(Collision2D collision)
{
    Debug.Log($"Collided with GameObject: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

    // Show click to chat button
    InteractButton.SetActive(true);

    // Check if the collision is with an NPC
    // if (collision.gameObject.CompareTag("NPC"))
    // {
    //     NPCCharacter npcCharacter = collision.gameObject.GetComponent<NPCCharacter>();

    //     if (npcCharacter != null)
    //     {
    //         // Access NPC information
    //         collidedUserId = npcCharacter.GetUserId();

    //         Debug.Log($"Collided with NPC (ID: {collidedUserId})");

    //         // Disable the Rigidbody2D to stop the NPCCharacter from moving
    //         Rigidbody2D npcRigidbody = npcCharacter.GetComponent<Rigidbody2D>();
    //         if (npcRigidbody != null)
    //         {
    //             npcRigidbody.bodyType = RigidbodyType2D.Static; // Set to Static to make it immovable
    //         }

    //         // Store the collidedUserId in PlayerPrefs
    //         PlayerPrefs.SetString("CollidedUserId", collidedUserId.ToString());
    //     }
    // }

    if (collision.gameObject.CompareTag("Player"))
    {
        CharacterComponent playerCharacterComponent = collision.gameObject.GetComponent<CharacterComponent>();

        if (playerCharacterComponent != null)
        {
            // Access NPC information
            collidedUserId = playerCharacterComponent.GetUserId();

            Debug.Log($"Collided with Player (ID: {collidedUserId})");

            // Disable the Rigidbody2D to stop the NPCCharacter from moving
            Rigidbody2D playerRigidbody = playerCharacterComponent.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.bodyType = RigidbodyType2D.Static; // Set to Static to make it immovable
            }

            // Store the collidedUserId in PlayerPrefs
            PlayerPrefs.SetString("CollidedUserId", collidedUserId.ToString());
        }
    }



    



}

    void OnCollisionExit2D(Collision2D collision)
    {
        // Hide click to chat button
        InteractButton.SetActive(false);
    }

    // Update is called once per frame
    async void Update()
    {
        await MovePlayerByClick();
    }

}

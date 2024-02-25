using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Clients;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Animator animator;
    private float moveSpeed = 5.0f;
    private Guid collidedCharacterId;

    public GameObject InteractButton;
    public Tilemap tilemap; // Reference to the Tilemap

    // Start is called before the first frame update
    async void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void SetTilemap(Tilemap tilemap)
    {
        Debug.Log("Setting tilemap");
        this.tilemap = tilemap;
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
            // Calculate the next position
            Vector2 nextPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

            // Check if the next position is within the bounds of the tilemap
            if (IsWithinTilemapBounds(nextPosition))
            {
                // Move the player towards the target position
                rb.velocity = new Vector2(Mathf.Round(direction.x * moveSpeed), Mathf.Round(direction.y * moveSpeed));
            }
            else
            {
                // Stop the player if the next position is outside the tilemap bounds
                rb.velocity = Vector2.zero;
            }

            // Get the current player position
            int xCoordinate = Mathf.RoundToInt(transform.position.x);
            int yCoordinate = Mathf.RoundToInt(transform.position.y);

            // TODO: Comment out when backend works
            PlayerPrefs.SetString("coordX", xCoordinate.ToString());
            PlayerPrefs.SetString("coordY", yCoordinate.ToString()); 

            // TODO: Backend api connection. Send the updated location to the server
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

    private bool IsWithinTilemapBounds(Vector2 position)
    {
        // Convert world position to tile position
        Vector3Int cellPosition = tilemap.WorldToCell(position);

        // Check if the cell position is within the bounds of the tilemap
        return tilemap.cellBounds.Contains(cellPosition);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collided with GameObject: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        // Show click to chat button
        InteractButton.SetActive(true);

        if (collision.gameObject.CompareTag("user") || collision.gameObject.CompareTag("agent"))
        {
            CharacterComponent characterComponent = collision.gameObject.GetComponent<CharacterComponent>();

            if (characterComponent != null)
            {
                // Access NPC information
                collidedCharacterId = characterComponent.GetCharacterId();
                string collidedCharacterType = characterComponent.GetCharacterType();

                Debug.Log($"Collided with Character Type: {collidedCharacterType} (ID: {collidedCharacterId})");

                // Store the CollidedCharacterId in PlayerPrefs to access in the Chat scene
                // TODO: Do this without using PlayerPrefs
                PlayerPrefs.SetString("CollidedCharacterId", collidedCharacterId.ToString());
                PlayerPrefs.SetString("CollidedCharacterType", collidedCharacterType);
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


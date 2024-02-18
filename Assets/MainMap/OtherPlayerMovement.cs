using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Clients;


// This script is attached to all other user game objects to handle their movement and animation
// It is used to move other players in the game. If other players move, the current player will 
// receive information about their new position and this script will move the other players accordingly
public class OtherPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 5.0f;
    private Guid userId;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        SignalRClient.Instance.RegisterUpdateLocationHandler(this);
    }

    // This method is called my sign
    public async Task UpdateLocation(Guid userId, int x, int y)
    {
        // if updating another user's position, this script that is attached to a player should not move
        if (this.userId == userId) 
        {
            MovePlayer(x, y);
        }
    }

    void MovePlayer(int x, int y)
    {
        Vector2 targetPosition = new Vector2(x, y);

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

            // Move the player towards the target position
            rb.velocity = new Vector2(Mathf.Round(direction.x * moveSpeed), Mathf.Round(direction.y * moveSpeed));
        }
        else
        {
            // If the player is within the stopping distance, stop its movement and animation
            rb.velocity = Vector2.zero;
            animator.SetFloat("MoveX", 0.001f);
            animator.SetFloat("MoveY", 0.001f);
        }
    }

}

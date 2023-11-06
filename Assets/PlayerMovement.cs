using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Animator animator;
    private float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void MovePlayerByClick()
    {
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(touch.position);
        }
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
    }
    else
    {
        // If the player is within the stopping distance, stop its movement and animation
        rb.velocity = Vector2.zero;
        animator.SetFloat("MoveX", 0.001f);
        animator.SetFloat("MoveY", 0.001f);
    }
}


    // Update is called once per frame
    void Update()
    {
        MovePlayerByClick();
    }
}

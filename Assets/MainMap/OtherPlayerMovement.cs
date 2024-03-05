using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Clients;
using System.Collections.Concurrent;


// This script is attached to all other user game objects to handle their movement and animation
// It is used to move other players in the game. If other players move, the current player will 
// receive information about their new position and this script will move the other players accordingly
public class OtherPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 5f;
    public Vector2 movementDirection = new Vector2(0.0f, 0.0f);
    public Guid userId;
    private Animator animator;
    private GameObject gameClientGO;
    public bool collided = false;
    public Collider2D collidedPlayer;
    private Vector2 targetPosition;
    private ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();


    // // Start is called before the first frame update
    void Start()
    {
        SignalRClient.Instance.RegisterUpdateLocationHandler(this);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Enqueue(Action action)
    {
        _actions.Enqueue(action);
    }
    void Update()
    {
        // Check distance to target position and stop if close enough
        if (Vector2.Distance(transform.position, targetPosition) <= 0.1f || collided)
        {
            StopMovement();
        }
        
        // Process actions
        while (_actions.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }

    public async Task UpdateLocation(Guid userId, int x, int y)
    {
        if (this.userId == userId) 
        {
            Enqueue(() =>
            {
                // Set the new target position and start moving towards it
                targetPosition = new Vector2(x, y);
                MovePlayer();
            });
        }
    }

    void MovePlayer()
    {
        if (targetPosition == Vector2.zero && rb.position == Vector2.zero)
        {
            return;
        }        
        
        movementDirection = (targetPosition - (Vector2)transform.position).normalized;

        // Start moving towards the new target position
        if (!collided)
        {
            
            rb.velocity = movementDirection * moveSpeed;
            UpdateAnimation(movementDirection);
        }
        else
        {
            if (Vector2.Distance(transform.position, collidedPlayer.transform.position) <= 0.75f) {
                // Adjust the position to move away slightly
                transform.position -= new Vector3(1f, 1f, 0f);
            }

            Vector2 positionRelative = transform.InverseTransformPoint(collidedPlayer.transform.position);
            float moveRelative = Vector2.Distance(positionRelative, movementDirection);

            if (moveRelative > 1.75f)
            {
                collided = false;
                rb.velocity = movementDirection * moveSpeed;
                UpdateAnimation(movementDirection);
            }
            else{
                StopMovement();
            }
        }
    }


    void UpdateAnimation(Vector2 direction)
    {
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        animator.SetBool("moving", true);
    }

    void StopMovement()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("moving", false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // collided = true;
        collidedPlayer = collision;
        // Vector3 currentPosition = collidedPlayer.transform.position;
        // Vector2 currentPosition2D = new Vector2(currentPosition.x, currentPosition.y);
        // targetPosition = currentPosition2D;

        collided = true;
        if (collision.transform.tag == "user" || collision.transform.tag == "agent" || collision.transform.tag == "egg")
        {
            Vector2 positionRelative = transform.InverseTransformPoint(collision.transform.position);
            movementDirection = positionRelative;
        }
    }
 
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!(collision.transform.tag == "user") && !(collision.transform.tag == "agent") && !(collision.transform.tag == "egg"))
            collided = false;
    }
 
    void OnTriggerExit2D(Collider2D collision)
    {
        collided = false;
    }
}


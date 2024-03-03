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
    public Guid userId;
    private Animator animator;
    private GameObject gameClientGO;
    public bool collided = false;
    public Collider2D collidedPlayer;
    private Vector2 targetPosition;
    private ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();


    // Start is called before the first frame update
    void Start()
    {
        // TODO: Uncomment when SignalR is ready
        SignalRClient.Instance.RegisterUpdateLocationHandler(this);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameClientGO = GameObject.Find("GameClient");
        TestOtherPlayerMovementController testOtherPlayerMovementController = gameClientGO.GetComponent<TestOtherPlayerMovementController>();
        testOtherPlayerMovementController.otherPlayerMovementScript = this;
    }

    public void Enqueue(Action action)
    {
        _actions.Enqueue(action);
    }
    void Update()
    {
        // Check distance to target position and stop if close enough
        if (Vector2.Distance(transform.position, targetPosition) <= 0.1f)
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
        
        // Calculate direction each time MovePlayer is called
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        
        // Start moving towards the new target position
        if (!collided)
        {
            rb.velocity = direction * moveSpeed;
            UpdateAnimation(direction);
        }
        else
        {
            StopMovement();
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
        collided = true;
        collidedPlayer = collision;
        Vector3 currentPosition = collidedPlayer.transform.position;
        Vector2 currentPosition2D = new Vector2(currentPosition.x, currentPosition.y);
        targetPosition = currentPosition2D;
    }
 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!(collision.transform.tag == "user") && !(collision.transform.tag == "agent"))
            collided = true;
    }
 
    private void OnTriggerExit2D(Collider2D collision)
    {
        collided = false;
    }

}

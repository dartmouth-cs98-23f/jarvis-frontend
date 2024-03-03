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
     public Vector2 direction = Vector2.zero;
    private Guid collidedCharacterId;

    public GameObject InteractButton;
    public GameObject NurtureButton;
    public Tilemap tilemap; // Reference to the Tilemap
    public bool collided = false;
    public Collider2D collidedPlayer;


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
                if (IsWithinTilemapBounds(targetPosition)){
                    await SignalRClient.Instance.UpdateLocation((int) targetPosition.x, (int) targetPosition.y);
                }
                else {
                    Debug.Log("Not going to send this location to the backend, it is out of the tilemap boundary.");
                }
            }
        }

        // This fixes the player automatically going to 0,0 on start
        if (targetPosition.x == 0f && targetPosition.y == 0f)
        {
            animator.SetBool("moving", false);
            rb.velocity = Vector2.zero;
            return;
        }

        // Calculate the direction from the current position to the target position
        direction = (targetPosition - (Vector2)transform.position).normalized;

        // Update the animator parameters
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        // Check if the distance between the player and the target is greater than the stopping distance
        if (Vector2.Distance(targetPosition, transform.position) > 0.1f)
        {

            // Check if the next position is within the bounds of the tilemap
            if (IsWithinTilemapBounds(targetPosition))
            {
                if (!collided){
                // Move the player towards the target position
                animator.SetBool("moving", true);
                rb.velocity = new Vector2(Mathf.Round(direction.x * moveSpeed), Mathf.Round(direction.y * moveSpeed));
                }
                else{
                    Debug.Log($"Executing Object Position: {transform.position}");
                    Debug.Log($"Tackled NPC Position: {collidedPlayer.transform.position}");

                    // Check the local position after the InverseTransformPoint
                    Vector2 positionRelative = transform.InverseTransformPoint(collidedPlayer.transform.position);
                    Debug.Log($"Position Relative: {positionRelative}");

                    float moveRelative = Vector2.Distance(positionRelative, direction);
                    Debug.Log("moveRelative " + moveRelative);

                    if (moveRelative > 1.0f)
                    {
                        rb.velocity = new Vector2(Mathf.Round(direction.x * moveSpeed), Mathf.Round(direction.y * moveSpeed));
                        animator.SetBool("moving", true);
                    }
                    else
                        rb.velocity = Vector2.zero;
                        animator.SetBool("moving", false);
                        }
            }
            else
            {
                // Stop the player if the next position is outside the tilemap bounds
                targetPosition = transform.position;
                animator.SetBool("moving", false);
                rb.velocity = Vector2.zero;
            }
            // Get the current player position
            int xCoordinate = Mathf.RoundToInt(transform.position.x);
            int yCoordinate = Mathf.RoundToInt(transform.position.y);

            // // TODO: Comment out when backend works
            // PlayerPrefs.SetString("coordX", xCoordinate.ToString());
            // PlayerPrefs.SetString("coordY", yCoordinate.ToString()); 

            // TODO: Backend api connection. Send the updated location to the server
            // await SignalRClient.Instance.UpdateLocation(xCoordinate, yCoordinate);
            // await SignalRClient.Instance.UpdateLocation((int) targetPosition.x, (int) targetPosition.y);

        }
        else
        {
            // If the player is within the stopping distance, stop its movement and animation
            animator.SetBool("moving", false);
            rb.velocity = Vector2.zero;
        }
    }

    private bool IsWithinTilemapBounds(Vector2 position)
    {
        // Convert world position to tile position
        Vector3Int cellPosition = tilemap.WorldToCell(position);

        // Check if the cell position is within the bounds of the tilemap
        return tilemap.cellBounds.Contains(cellPosition);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collided with GameObject: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        
        if (collision.gameObject.CompareTag("user") || collision.gameObject.CompareTag("agent"))
        {
            InteractButton.SetActive(true);
            collided = true;
            collidedPlayer = collision;
            
            // Vector3 currentPosition = collidedPlayer.transform.position;
            // Vector2 currentPosition2D = new Vector2(currentPosition.x, currentPosition.y);
            // targetPosition = currentPosition2D;

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
        else if (collision.gameObject.CompareTag("egg")){
            collided = true;
            collidedPlayer = collision;
            
            // Vector3 currentPosition = collidedPlayer.transform.position;
            // Vector2 currentPosition2D = new Vector2(currentPosition.x, currentPosition.y);
            // targetPosition = currentPosition2D;

            CharacterComponent eggComponent = collision.gameObject.GetComponent<CharacterComponent>();

            if (eggComponent != null)
            {
                // Access NPC information
                collidedCharacterId = eggComponent.GetCharacterId();
                string collidedCharacterType = eggComponent.GetCharacterType();

                Debug.Log($"Collided with Character Type: {collidedCharacterType} (ID: {collidedCharacterId})");

                GameObject gameClient = GameObject.Find("GameClient");
                gameClient.GetComponent<GameClient>().eggId = collidedCharacterId;
            }
            NurtureButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Hide click to chat button
        InteractButton.SetActive(false);
        NurtureButton.SetActive(false);
        if (collided)
        {
            collided = false;
            collidedPlayer = null;
        }
    }

    // Update is called once per frame
    async void Update()
    {
        await MovePlayerByClick();
    }
}

//     [Header("General:")]
//     [Space]
//     public bool npcTackled = false;
//     public Collider2D tackledNPC;
 
 
//     [Header("Movement Settings:")]
//     [Space]
//     public float movementBaseSpeed = 5.0f;
//     public Vector2 movementDirection = Vector2.zero;
//     public float movementSpeed = 0.0f;
//     public bool canMove = true;
//     public GameObject InteractButton;
//     public GameObject NurtureButton;
//     public Tilemap tilemap; // Reference to the Tilemap
//     private Animator animator;
 
//     [Header("References:")]
//     [Space]
//     public Rigidbody2D playerRB;
 
//     async void Start()
//     {
//         playerRB = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//     }
 
//     void Update()
//     {
//         if (canMove)
//         {
//             ProcessMovementInputs();
//             Move();      
//         }  
//     }

//     public void SetTilemap(Tilemap tilemap)
//     {
//         Debug.Log("Setting tilemap");
//         this.tilemap = tilemap;
//     }
//     void ProcessMovementInputs()
//     {
//         //reset that we are moving
//         movementSpeed = 0.0f;
 
//         //get the absolut inpuit from arrow keys to decide in which direction to move the player
//         movementDirection.x = Input.GetAxisRaw("Horizontal");
//         movementDirection.y = Input.GetAxisRaw("Vertical");
 
//         //if the movement direction is not equal to the zero vector we will define the movmentspeed and declare that the player is actually moving
//         if (movementDirection != Vector2.zero)
//         {
//             //clamp the movementdirections magnitude between 0 and 1, so nobody cheat with special input devices (xbox controllers), and assign it as the movementspeed
//             movementSpeed = 5 * Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
//             //normalize the movement direction, so we are not unrealisticly moving double as fast when using diagonal movement direction
//             movementDirection.Normalize();
//         }
//     }
 
//     void Move()
//     {
//         //only move the palyer into the direction when he currently not in contact with an NPC
//         if (!npcTackled)
//         {
//             playerRB.velocity = movementDirection * movementSpeed * movementBaseSpeed;
//             await SignalRClient.Instance.UpdateLocation((int) targetPosition.x, (int) targetPosition.y);
//         }
//         else
//         {
//             //get the relative position of the NPC to the player
//             // Print or log the positions for debugging
//             Debug.Log($"Executing Object Position: {transform.position}");
//             Debug.Log($"Tackled NPC Position: {tackledNPC.transform.position}");

//             // Check the local position after the InverseTransformPoint
//             Vector2 positionRelative = transform.InverseTransformPoint(tackledNPC.transform.position);
//             Debug.Log($"Position Relative: {positionRelative}");

//             //if we are stucking at the NPC we need to trick around, so we can leave the NPC's colliding shape again
//             //we do this by checking movementDirection (where the player would go to) and get the distance between the NPC's relative position and the movementDirection
//             float moveRelative = Vector2.Distance(positionRelative, movementDirection);
//             Debug.Log("moveRelative " + moveRelative);
//             //as if the player is moving away from the NPC the moveRelative will get > 1, so we can assign the normal movement flow
//             //if the player would go into the NPC with his movementDirection again, then the moveRelative would be < 1, so we assign vector2.zero velocity to his RB
//             if (moveRelative > 1.0f)
//             {
//                 playerRB.velocity = movementDirection * movementSpeed * movementBaseSpeed;
//             }
//             else
//                 playerRB.velocity = Vector2.zero;
//         }
//     }
 
//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         //only care for collision with NPC
//         //other collisions will be treated by the collider components (static structures, that cant get pushed)
//         if (collision.transform.tag == "user")
//         {
//             npcTackled = true;
//             //save the currently tackled NPC for later uses, e.g. relative position and talking with the NPC
//             tackledNPC = collision;
//         }
//     }
 
//     private void OnCollisionExit2D(Collision2D collision)
//     {
//         if (npcTackled)
//         {
//             npcTackled = false;
//             tackledNPC = null;
//         }
//     }
// }
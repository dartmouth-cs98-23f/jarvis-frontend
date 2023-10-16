using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 targetPosition;

    private float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) 
        {
            Debug.Log("Touch detected");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) 
            {
                targetPosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        if (targetPosition != rb.position)
        {
            rb.velocity = new Vector2(Mathf.Round(direction.x * moveSpeed), Mathf.Round(direction.y * moveSpeed));
        } else 
        {
            rb.velocity = Vector2.zero;
        }
        Debug.Log("Moving player to (" + targetPosition.x + ", " + targetPosition.y + ")");
    }
}

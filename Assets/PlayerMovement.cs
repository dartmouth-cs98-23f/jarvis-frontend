using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    bool canMove = true;
    private float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (Vector2.Distance(targetPosition, transform.position) > 0.1f) // Adjust the tolerance level as needed
        {
            rb.velocity = new Vector2(Mathf.Round(direction.x * moveSpeed), Mathf.Round(direction.y * moveSpeed));
        }
        else 
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void OnHamburgerMenuClick()
    {
        canMove = false;
        Debug.Log("Hamburger Menu Clicked!");
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove){
            MovePlayerByClick();
        }
        
    }
}

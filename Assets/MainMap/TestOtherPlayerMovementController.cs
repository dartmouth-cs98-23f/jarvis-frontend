// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TestOtherPlayerMovementController : MonoBehaviour
// {
//     public OtherPlayerMovement otherPlayerMovementScript;
//     public float updateInterval = 2.0f;

//     private void Start()
//     {
//         // Start the coroutine that simulates incoming location updates
//         Debug.Log("Starting location update simulation");
//         StartCoroutine(SimulateLocationUpdates());
//     }

//     IEnumerator SimulateLocationUpdates()
//     {
//         while (true)
//         {
//             // Simulate a new position update
//             int newX = Random.Range(-200, 200);
//             int newY = Random.Range(-200, 200);

//             // Invoke the update method on OtherPlayerMovement script
//             if (otherPlayerMovementScript != null)
//             {
//                 Debug.Log("Simulating location update: " + newX + ", " + newY);

//                 otherPlayerMovementScript.UpdateLocation(otherPlayerMovementScript.userId, newX, newY);
//             }

//             // Wait for the specified interval before sending the next update
//             yield return new WaitForSeconds(updateInterval);
//         }
//     }
// }

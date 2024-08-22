using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFootMovementController : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float detectionRange = 5f; // Range of the detection cone
    public float detectionAngle = 45f; // Angle of the detection cone

    public float movementSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // Check if the player is inside the detection cone
        if (IsPlayerInDetectionCone())
        {
            // Move towards the player
            MoveTowardsPlayer();
        }
    }

    // Function to check if the player is inside the detection cone
    private bool IsPlayerInDetectionCone()
    {
        // Calculate the direction to the player
        Vector2 directionToPlayer = playerTransform.position - transform.position;

        // Calculate the angle between the forward direction of the detection cone and the direction to the player
        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);

        FaceToPosition(playerTransform.position);

        // Check if the player is within the detection cone angle and within the detection range
        return angleToPlayer < detectionAngle * 0.5f && directionToPlayer.magnitude < detectionRange;
    }

    // Function to move towards the player
    private void MoveTowardsPlayer()
    {
        // Move towards the player's position
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, Time.deltaTime * movementSpeed);
    }

    // Function to face towards a given position
    public void FaceToPosition(Vector2 pos)
    {
        // Get the direction from the current position to the target position
        Vector2 direction = (pos - (Vector2)transform.position).normalized;

        // Calculate the angle between the direction and Vector.right
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the detection cone towards the target direction
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Function to move forward
    public void MoveForward()
    {
        // Move the AI forward (you can implement your own movement logic here)
    }

    // Draw the detection cone using Gizmos
    private void OnDrawGizmosSelected()
    {
        // Set the color of the Gizmo
        Gizmos.color = Color.yellow;

        // Calculate the start and end angles of the detection cone
        float startAngle = -detectionAngle * 0.5f ;
        float endAngle = detectionAngle * 0.5f;

        // Calculate the direction of the detection cone
        Vector3 coneDirection = transform.right;

        // Draw the detection cone
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, startAngle) * coneDirection * detectionRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, endAngle) * coneDirection * detectionRange);
        Gizmos.DrawLine(transform.position + Quaternion.Euler(0, 0, startAngle) * coneDirection * detectionRange, transform.position + Quaternion.Euler(0, 0, endAngle) * coneDirection * detectionRange);
    }
}

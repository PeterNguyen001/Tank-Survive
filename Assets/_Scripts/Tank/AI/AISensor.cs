using System.Collections.Generic;
using UnityEngine;

public class AISensor : TankSubComponent
{
    public float detectionRange = 10f;
    public float coneAngle = 90f; // Angle of detection cone
    public LinkedList<Collider2D> ignoreColliders = new LinkedList<Collider2D>();

    bool targetDetected = false;
    private Transform detectedTarget;

    public float detectObstacleRange = 2f;
    bool obstacleInRange = false;
    Transform detectedObstacle;

    public Transform chassis; // Store the chassis transform

    private void Start()
    {
        ignoreColliders = tankStatus.GetListOfCollider2D();
    }

    public void Detect(Transform chassis)
    {
        this.chassis = chassis; // Assign chassis for Gizmos use
        Vector3 sensorPosition = chassis.position;

        // Calculate the forward direction based on the chassis's rotation
        Vector3 forwardDirection = chassis.right;

        // Detect target (Player) within the cone
        Collider2D[] colliders = Physics2D.OverlapCircleAll(sensorPosition, detectionRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Vector2 directionToTarget = (Vector2)(collider.transform.position - sensorPosition);

                // Calculate the angle between the forward direction and the target
                float angleToTarget = Vector2.Angle(forwardDirection, directionToTarget);

                // Check if the target is within the cone angle
                if (angleToTarget <= coneAngle * 0.5f)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(sensorPosition, directionToTarget.normalized, directionToTarget.magnitude);

                    targetDetected = true;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (ignoreColliders.Contains(hit.collider))
                        {
                            continue; // Ignore specified colliders
                        }

                        if (!hit.collider.CompareTag("Player"))
                        {
                            targetDetected = false; // Obstacle detected in the way of the player
                            detectedTarget = null;
                            break;
                        }
                    }

                    if (targetDetected)
                    {
                        detectedTarget = collider.transform;
                        Debug.Log("Target detected within cone: " + detectedTarget.name);
                    }
                }
            }
        }

        // Detect the closest obstacle within the cone
        obstacleInRange = false; // Reset obstacle detection status
        float closestDistance = Mathf.Infinity; // Initialize to a large value
        foreach (Collider2D collider in colliders)
        {
            if (!collider.CompareTag("Player") && !ignoreColliders.Contains(collider))
            {
                Vector2 directionToObstacle = (Vector2)(collider.transform.position - sensorPosition);

                // Calculate the angle between the forward direction and the obstacle
                float angleToObstacle = Vector2.Angle(forwardDirection, directionToObstacle);

                // Check if the obstacle is within the cone angle
                if (angleToObstacle <= coneAngle * 0.5f && directionToObstacle.magnitude <= detectObstacleRange)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(sensorPosition, directionToObstacle.normalized, directionToObstacle.magnitude);

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (ignoreColliders.Contains(hit.collider))
                        {
                            continue; // Ignore specified colliders
                        }

                        if (!hit.collider.CompareTag("Player"))
                        {
                            float distanceToObstacle = directionToObstacle.magnitude;
                            if (distanceToObstacle < closestDistance)
                            {
                                closestDistance = distanceToObstacle;
                                detectedObstacle = collider.transform;
                                obstacleInRange = true;
                            }
                        }
                    }
                }
            }
        }

        if (obstacleInRange)
        {
            Debug.Log("Closest obstacle detected: " + detectedObstacle.name + " at distance: " + closestDistance);
        }
    }

    private void OnDrawGizmos()
    {
        if (chassis == null) return; // Use chassis transform if available

        Vector3 sensorPosition = chassis.position;

        // Calculate the forward direction based on the chassis's rotation
        Vector3 forwardDirection = chassis.right;

        // Draw the detection cone
        Gizmos.color = Color.yellow;
        Quaternion leftRayRotation = Quaternion.Euler(0, 0, -coneAngle * 0.5f);
        Quaternion rightRayRotation = Quaternion.Euler(0, 0, coneAngle * 0.5f);
        Vector3 leftRayDirection = leftRayRotation * forwardDirection;
        Vector3 rightRayDirection = rightRayRotation * forwardDirection;

        Gizmos.DrawLine(sensorPosition, sensorPosition + leftRayDirection * detectionRange);
        Gizmos.DrawLine(sensorPosition, sensorPosition + rightRayDirection * detectionRange);
        Gizmos.DrawLine(sensorPosition + leftRayDirection * detectionRange, sensorPosition + rightRayDirection * detectionRange);

        // Draw detection range circle for reference
        Gizmos.DrawWireSphere(sensorPosition, detectionRange);

        // Draw obstacle detection range circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sensorPosition, detectObstacleRange);
    }
}

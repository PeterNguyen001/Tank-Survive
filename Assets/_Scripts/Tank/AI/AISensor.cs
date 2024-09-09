using System.Collections.Generic;
using UnityEngine;

public class AISensor : TankSubComponent
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float coneAngle = 90f; // Angle of detection cone
    [SerializeField] private List<string> tagsToFindList = new List<string>();

    private DetectionInfo detectedTargetInfo;
    public Transform chassis; // Store the chassis transform
    private LinkedList<Collider2D> ignoreColliders = new LinkedList<Collider2D>();

    private void Start()
    {
        ignoreColliders = tankStatus.GetListOfCollider2D();
    }

    public DetectionInfo Detect()
    {
        Vector3 sensorPosition = chassis.position;
        Vector3 forwardDirection = chassis.right;

        detectedTargetInfo = default; // Reset detected target info
        float closestDistance = Mathf.Infinity; // Initialize with a very large value

        Collider2D[] colliders = Physics2D.OverlapCircleAll(sensorPosition, detectionRange);
        foreach (Collider2D collider in colliders)
        {
            if (tagsToFindList.Contains(collider.tag)) // Check if the collider's tag is in the list
            {
                Vector2 directionToTarget = (Vector2)(collider.transform.position - sensorPosition);
                float angleToTarget = Vector2.Angle(forwardDirection, directionToTarget);

                // Check if the target is within the cone angle
                if (angleToTarget <= coneAngle * 0.5f)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(sensorPosition, directionToTarget.normalized, directionToTarget.magnitude);
                    bool isObstructed = false;

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (ignoreColliders.Contains(hit.collider))
                            continue; // Ignore specified colliders

                        if (!tagsToFindList.Contains(hit.collider.tag))
                        {
                            isObstructed = true; // Obstruction detected
                            break;
                        }
                    }

                    if (!isObstructed)
                    {
                        float distanceToTarget = directionToTarget.magnitude;
                        if (distanceToTarget < closestDistance) // Update if this target is closer
                        {
                            closestDistance = distanceToTarget;
                            detectedTargetInfo = new DetectionInfo(collider.transform.position, collider.tag);
                        }
                    }
                }
            }
        }

        if (closestDistance < Mathf.Infinity)
        {
            Debug.Log("Closest detected: " + detectedTargetInfo.Tag + " at distance: " + closestDistance);
        }

        return detectedTargetInfo; // Return the closest detected target, or default if none found
    }

    private void OnDrawGizmos()
    {
        if (chassis == null) return;

        Vector3 sensorPosition = chassis.position;
        Vector3 forwardDirection = chassis.right;

        // Draw the detection cone for the main detection range (detectionRange)
        Gizmos.color = Color.yellow;
        Quaternion leftRayRotation = Quaternion.Euler(0, 0, -coneAngle * 0.5f);
        Quaternion rightRayRotation = Quaternion.Euler(0, 0, coneAngle * 0.5f);
        Vector3 leftRayDirection = leftRayRotation * forwardDirection;
        Vector3 rightRayDirection = rightRayRotation * forwardDirection;

        Gizmos.DrawLine(sensorPosition, sensorPosition + leftRayDirection * detectionRange);
        Gizmos.DrawLine(sensorPosition, sensorPosition + rightRayDirection * detectionRange);
        Gizmos.DrawLine(sensorPosition + leftRayDirection * detectionRange, sensorPosition + rightRayDirection * detectionRange);

        // Draw detection range circle for the main detection range
        Gizmos.DrawWireSphere(sensorPosition, detectionRange);
    }
}

public struct DetectionInfo
{
    public Vector3 Position { get; private set; }
    public string Tag { get; private set; }

    public DetectionInfo(Vector3 position, string tag)
    {
        Position = position;
        Tag = tag;
    }
}
public static class DetectionTag
{

}
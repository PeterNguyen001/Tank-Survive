using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AISensor : TankSubComponent
{


    [SerializeField] private float obstacleDetectionRange;
    [SerializeField] private float obstacleDetectionAngle; 
    [SerializeField] private List<string> obstacleTagsToDetectList = new List<string>();

    private DetectionInfo detectedTargetInfo;
    public Transform chassis; // Store the chassis transform
    private LinkedList<Collider2D> ignoreColliders = new LinkedList<Collider2D>();

    private void Start()
    {
        ignoreColliders = tankStatus.GetListOfCollider2D();
    }

    public DetectionInfo Detect(float range, float angle, List<string> tags, Vector3 directionOffset)
    {
        Vector3 sensorPosition = chassis.position;
        Vector3 detectionDirection = (chassis.right + directionOffset).normalized;

        detectedTargetInfo = new DetectionInfo(Vector2.zero, 0, ""); // Reset detected target info
        float closestDistance = Mathf.Infinity; // Initialize with a very large value

        Collider2D[] colliders = Physics2D.OverlapCircleAll(sensorPosition, range);
        foreach (Collider2D collider in colliders)
        {
            if (tags.Contains(collider.tag)) // Check if the collider's tag is in the list
            {
                Vector2 directionToTarget = (Vector2)(collider.transform.position - sensorPosition);
                float angleToTarget = Vector2.Angle(detectionDirection, directionToTarget);

                // Check if the target is within the cone angle
                if (angleToTarget <= angle * 0.5f)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll(sensorPosition, directionToTarget.normalized, directionToTarget.magnitude);
                    bool isObstructed = false;
                    Vector2 hitPosition = Vector2.zero;

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (ignoreColliders.Contains(hit.collider))
                            continue; // Ignore specified colliders

                        if (!tags.Contains(hit.collider.tag))
                        {
                            isObstructed = true; // Obstruction detected
                            break;
                        }

                        // Capture the ray hit position
                        hitPosition = hit.point;
                    }

                    if (!isObstructed)
                    {
                        float distanceToHit = (hitPosition - (Vector2)sensorPosition).magnitude; // Calculate distance to ray hit point
                        if (distanceToHit < closestDistance) // Update if this hit is closer
                        {
                            closestDistance = distanceToHit;
                            detectedTargetInfo = new DetectionInfo(hitPosition, closestDistance, collider.tag);
                        }
                    }
                }
            }
        }

        return detectedTargetInfo; // Return the closest detected target with ray hit position
    }


    public DetectionInfo DetectObstacle()
    {
        if(obstacleTagsToDetectList.Count == 0)
        {
            Debug.LogWarning("ObstacleTags is Empty");
        }
       return Detect(obstacleDetectionRange, obstacleDetectionAngle, obstacleTagsToDetectList, Vector3.zero);
    }
    public (DetectionInfo forward, DetectionInfo left, DetectionInfo right) DetectForwardLeftRightObstacles()
    {
        if (obstacleTagsToDetectList.Count == 0)
        {
            Debug.LogWarning("ObstacleTags is Empty");
        }

        // Detect forward obstacle (no offset)
        DetectionInfo forwardObstacle = Detect(obstacleDetectionRange, obstacleDetectionAngle, obstacleTagsToDetectList, Vector3.zero);

        // Detect left obstacle (rotate chassis direction slightly left)
        Vector3 leftOffset = Quaternion.Euler(0, 0, 45) * Vector3.right;
        DetectionInfo leftObstacle = Detect(obstacleDetectionRange, obstacleDetectionAngle, obstacleTagsToDetectList, leftOffset);

        // Detect right obstacle (rotate chassis direction slightly right)
        Vector3 rightOffset = Quaternion.Euler(0, 0, -45) * Vector3.right;
        DetectionInfo rightObstacle = Detect(obstacleDetectionRange, obstacleDetectionAngle, obstacleTagsToDetectList, rightOffset);

        return (forwardObstacle, leftObstacle, rightObstacle);
    }

    public float GetObstacleDetectionRange()
    {
        return obstacleDetectionRange;
    }


    private void OnDrawGizmos()
    {
        if (chassis == null) return;

        Vector3 sensorPosition = chassis.position;
        Vector3 forwardDirection = chassis.right;

        // Draw the detection cone for the main detection range (obstacleDetectionRange)
        Gizmos.color = Color.yellow;
        Quaternion leftRayRotation = Quaternion.Euler(0, 0, -obstacleDetectionAngle * 0.5f);
        Quaternion rightRayRotation = Quaternion.Euler(0, 0, obstacleDetectionAngle * 0.5f);
        Vector3 leftRayDirection = leftRayRotation * forwardDirection;
        Vector3 rightRayDirection = rightRayRotation * forwardDirection;

        Gizmos.DrawLine(sensorPosition, sensorPosition + leftRayDirection * obstacleDetectionRange);
        Gizmos.DrawLine(sensorPosition, sensorPosition + rightRayDirection * obstacleDetectionRange);
        Gizmos.DrawLine(sensorPosition + leftRayDirection * obstacleDetectionRange, sensorPosition + rightRayDirection * obstacleDetectionRange);

        // Draw detection range circle for the main detection range
        Gizmos.DrawWireSphere(sensorPosition, obstacleDetectionRange);
    }
}

public struct DetectionInfo
{
    public Vector2 position;
    public float distance;
    public string tag;
    public DetectionInfo(Vector3 position, float distance,string tag)
    {
        this.position = position;
        this.distance = distance;
        this.tag = tag;
    }
}
public static class DetectionTag
{

}
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AISensor : TankSubComponent
{


    [SerializeField] private float obstacleDetectionRange;
    [SerializeField] private float frontalObstacleDetectionAngle;
    [SerializeField] private float leftRightObstacleDetectionAngle;
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

    public (DetectionInfo forward, DetectionInfo left, DetectionInfo right) DetectForwardLeftRightObstacles()
    {
        if (obstacleTagsToDetectList.Count == 0)
        {
            Debug.LogWarning("ObstacleTags is Empty");
        }

        // Detect forward obstacle (center cone)
        DetectionInfo forwardObstacle = Detect(obstacleDetectionRange, frontalObstacleDetectionAngle, obstacleTagsToDetectList, Vector3.zero);

        // Left detection starts at the end of the forward cone (split to avoid overlap)
        Quaternion leftMinRotation = Quaternion.Euler(0, 0, frontalObstacleDetectionAngle * 0.5f); // Where forward ends
        Vector3 leftOffset = leftMinRotation * Vector3.right;
        DetectionInfo leftObstacle = Detect(obstacleDetectionRange, leftRightObstacleDetectionAngle, obstacleTagsToDetectList, leftOffset);

        // Right detection starts at the end of the forward cone (split to avoid overlap)
        Quaternion rightMinRotation = Quaternion.Euler(0, 0, -frontalObstacleDetectionAngle * 0.5f); // Where forward ends
        Vector3 rightOffset = rightMinRotation * Vector3.right;
        DetectionInfo rightObstacle = Detect(obstacleDetectionRange, leftRightObstacleDetectionAngle, obstacleTagsToDetectList, rightOffset);

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

    // Define half of the forward, left, and right detection angles
    float halfForwardAngle = frontalObstacleDetectionAngle * 0.5f;
    float halfSideAngle = leftRightObstacleDetectionAngle * 0.5f;

    // Forward Detection Cone (yellow)
    Gizmos.color = Color.yellow;
    Quaternion forwardLeftRotation = Quaternion.Euler(0, 0, -halfForwardAngle);
    Quaternion forwardRightRotation = Quaternion.Euler(0, 0, halfForwardAngle);
    Vector3 forwardLeftDirection = forwardLeftRotation * forwardDirection;
    Vector3 forwardRightDirection = forwardRightRotation * forwardDirection;

    // Draw forward detection cone
    Gizmos.DrawLine(sensorPosition, sensorPosition + forwardLeftDirection * obstacleDetectionRange);
    Gizmos.DrawLine(sensorPosition, sensorPosition + forwardRightDirection * obstacleDetectionRange);
    Gizmos.DrawLine(sensorPosition + forwardLeftDirection * obstacleDetectionRange, sensorPosition + forwardRightDirection * obstacleDetectionRange);

    // Left Detection Cone (green)
    Gizmos.color = Color.green;
    Quaternion leftMinRotation = Quaternion.Euler(0, 0, halfForwardAngle); // Starting from where forward cone ends
    Quaternion leftMaxRotation = Quaternion.Euler(0, 0, halfForwardAngle + leftRightObstacleDetectionAngle);
    Vector3 leftMinDirection = leftMinRotation * forwardDirection;
    Vector3 leftMaxDirection = leftMaxRotation * forwardDirection;

    // Draw left detection cone
    Gizmos.DrawLine(sensorPosition, sensorPosition + leftMinDirection * obstacleDetectionRange);
    Gizmos.DrawLine(sensorPosition, sensorPosition + leftMaxDirection * obstacleDetectionRange);
    Gizmos.DrawLine(sensorPosition + leftMinDirection * obstacleDetectionRange, sensorPosition + leftMaxDirection * obstacleDetectionRange);

    // Right Detection Cone (blue)
    Gizmos.color = Color.blue;
    Quaternion rightMinRotation = Quaternion.Euler(0, 0, -halfForwardAngle); // Starting from where forward cone ends
    Quaternion rightMaxRotation = Quaternion.Euler(0, 0, -(halfForwardAngle + leftRightObstacleDetectionAngle));
    Vector3 rightMinDirection = rightMinRotation * forwardDirection;
    Vector3 rightMaxDirection = rightMaxRotation * forwardDirection;

    // Draw right detection cone
    Gizmos.DrawLine(sensorPosition, sensorPosition + rightMinDirection * obstacleDetectionRange);
    Gizmos.DrawLine(sensorPosition, sensorPosition + rightMaxDirection * obstacleDetectionRange);
    Gizmos.DrawLine(sensorPosition + rightMinDirection * obstacleDetectionRange, sensorPosition + rightMaxDirection * obstacleDetectionRange);

    // Draw the detection range as a wire sphere
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(sensorPosition, obstacleDetectionRange);

    // Visualize the detected obstacles
    var (forwardObstacle, leftObstacle, rightObstacle) = DetectForwardLeftRightObstacles();

    // Forward obstacle detection (yellow)
    Gizmos.color = Color.yellow;
    if (forwardObstacle.distance > 0)
    {
        Gizmos.DrawLine(sensorPosition, forwardObstacle.position);
        Gizmos.DrawWireSphere(forwardObstacle.position, 0.2f);
    }

    // Left obstacle detection (green)
    Gizmos.color = Color.green;
    if (leftObstacle.distance > 0)
    {
        Gizmos.DrawLine(sensorPosition, leftObstacle.position);
        Gizmos.DrawWireSphere(leftObstacle.position, 0.2f);
    }

    // Right obstacle detection (blue)
    Gizmos.color = Color.blue;
    if (rightObstacle.distance > 0)
    {
        Gizmos.DrawLine(sensorPosition, rightObstacle.position);
        Gizmos.DrawWireSphere(rightObstacle.position, 0.2f);
    }
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
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

    public DetectionInfo Detect(float range, float angle, List<string> tags, float angleOffset)
    {
        Vector3 sensorPosition = chassis.position;

        // Rotate the forward direction (chassis.right) by angleOffset degrees
        Vector3 detectionDirection = Quaternion.Euler(0, 0, angleOffset) * chassis.right;

        detectedTargetInfo = new DetectionInfo(Vector2.zero, 0, ""); // Reset detected target info
        float closestDistance = Mathf.Infinity; // Initialize with a very large value

        Collider2D[] colliders = Physics2D.OverlapCircleAll(sensorPosition, range);
        foreach (Collider2D collider in colliders)
        {
            if (tags.Contains(collider.tag)) // Check if the collider's tag is in the list
            {
                // Find the closest point of contact on the collider
                Vector2 contactPoint = collider.ClosestPoint(sensorPosition);
                Vector2 directionToContact = contactPoint - (Vector2)sensorPosition;
                float angleToContact = Vector2.Angle(detectionDirection, directionToContact);

                // Check if the target is within the cone angle
                if (angleToContact <= angle * 0.5f)
                {
                    // Raycast to the contact point to check for obstructions
                    RaycastHit2D[] hits = Physics2D.RaycastAll(sensorPosition, directionToContact.normalized, directionToContact.magnitude);
                    bool isObstructed = false;

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (ignoreColliders.Contains(hit.collider))
                            continue; // Ignore specified colliders

                        if (!tags.Contains(hit.collider.tag))
                        {
                            isObstructed = true; // Obstruction detected
                            break;
                        }
                    }

                    if (!isObstructed)
                    {
                        float distanceToContact = directionToContact.magnitude;
                        if (distanceToContact < closestDistance) // Update if this contact is closer
                        {
                            closestDistance = distanceToContact;
                            detectedTargetInfo = new DetectionInfo(contactPoint, closestDistance, collider.tag);
                        }
                    }
                }
                else
                {
                    // If ClosestPoint is outside the angle, cast a ray to the edge of the cone
                    Vector3 edgeDirection = Quaternion.Euler(0, 0, angle * 0.5f * Mathf.Sign(angleOffset)) * detectionDirection;
                    RaycastHit2D[] edgeHit = Physics2D.RaycastAll(sensorPosition, edgeDirection.normalized, range);
                    Debug.DrawRay(sensorPosition, edgeDirection.normalized, Color.black);
                    //Debug.Log(edgeHit.collider.name);
                    foreach (RaycastHit2D hit in edgeHit)
                    {
                        if (ignoreColliders.Contains(hit.collider))
                            continue; // Ignore specified colliders                    
                        if (hit.collider != null && tags.Contains(hit.collider.tag))
                        {
                            //Debug.Log(hit.collider.name);
                            float distanceToEdgeHit = hit.distance;
                            if (distanceToEdgeHit < closestDistance)
                            {
                                closestDistance = distanceToEdgeHit;
                                detectedTargetInfo = new DetectionInfo(hit.point, closestDistance, hit.collider.tag);
                            }
                        }
                    }
                }
            }
        }

        return detectedTargetInfo; // Return the closest detected target with the contact point
    }




    public (DetectionInfo forward, DetectionInfo left, DetectionInfo right) DetectForwardLeftRightObstacles()
    {
        if (obstacleTagsToDetectList.Count == 0)
        {
            Debug.LogWarning("ObstacleTags is Empty");
        }

        // Detect forward obstacle (angleOffset = 0)
        DetectionInfo forwardObstacle = Detect(obstacleDetectionRange, frontalObstacleDetectionAngle, obstacleTagsToDetectList, 0);

        // Detect left obstacle (angleOffset is positive to rotate left)
        DetectionInfo leftObstacle = Detect(obstacleDetectionRange, leftRightObstacleDetectionAngle, obstacleTagsToDetectList, frontalObstacleDetectionAngle * 0.5f + leftRightObstacleDetectionAngle * 0.5f);

        // Detect right obstacle (angleOffset is negative to rotate right)
        DetectionInfo rightObstacle = Detect(obstacleDetectionRange, leftRightObstacleDetectionAngle, obstacleTagsToDetectList, -(frontalObstacleDetectionAngle * 0.5f + leftRightObstacleDetectionAngle * 0.5f));

        // Log detected obstacles in the correct cones
        //if (forwardObstacle.position != Vector2.zero)
        //{
        //    Debug.Log("Detect Obstacle Infront");
        //}
        //if (leftObstacle.position != Vector2.zero)
        //{
        //    Debug.Log("Detect Obstacle Left");
        //}
        //if (rightObstacle.position != Vector2.zero)
        //{
        //    Debug.Log("Detect Obstacle Right");
        //}

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
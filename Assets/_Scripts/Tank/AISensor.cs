using System.Collections.Generic;
using UnityEngine;

public class AISensor : TankSubComponent
{
    public float detectionRange = 10f;
    public float coneAngle = 90f; // Angle of detection cone
    public LinkedList<Collider2D> ignoreColliders = new LinkedList<Collider2D>();

    private Transform detectedTarget;

    private void Start()
    {
        ignoreColliders = tankStatus.GetListOfCollider2D();
    }

    public Transform DetectTarget(Transform chassis)
    {
        Vector3 sensorPosition = chassis.position;

        // Calculate the cone direction based on the turret's rotation
        Vector3 coneDirection = chassis.right;

        List<Vector2> coneVertices = BuildDetectionCone(sensorPosition, coneDirection, coneAngle);
        foreach (Vector2 vertex in coneVertices)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(vertex, detectionRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Vector2 directionToTarget = collider.transform.position - sensorPosition;
                    RaycastHit2D[] hits = Physics2D.RaycastAll(sensorPosition, directionToTarget.normalized, directionToTarget.magnitude);

                    bool targetDetected = true;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (ignoreColliders.Contains(hit.collider))
                        {
                            continue; // Ignore specified colliders
                        }

                        if (!hit.collider.CompareTag("Player"))
                        {
                            targetDetected = false; // Obstacle detected
                            break;
                        }
                    }

                    if (targetDetected)
                    {
                        detectedTarget = collider.transform;
                        return detectedTarget;
                    }
                }
            }
        }

        return null;
    }

    private List<Vector2> BuildDetectionCone(Vector3 position, Vector3 coneDirection, float coneAngle)
    {
        List<Vector2> coneVertices = new List<Vector2>();
        coneVertices.Add(position);
        for (int i = -1; i <= 1; i += 2)
        {
            float angle = coneAngle * 0.5f * i;
            Vector3 vertexDirection = Quaternion.Euler(0, 0, angle) * coneDirection;
            Vector2 vertex = position + vertexDirection * detectionRange;
            coneVertices.Add(vertex);
        }
        return coneVertices;
    }

    private void OnDrawGizmos()
    {
        if (transform == null) return;

        Vector3 sensorPosition = transform.position;

        // Calculate the cone direction based on the turret's rotation
        Vector3 coneDirection = transform.right;

        List<Vector2> coneVertices = BuildDetectionCone(sensorPosition, coneDirection, coneAngle);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(sensorPosition, coneVertices[1]);
        Gizmos.DrawLine(sensorPosition, coneVertices[2]);
        Gizmos.DrawLine(coneVertices[1], coneVertices[2]);

        // Optionally draw the detection range circles at the vertices
        foreach (Vector2 vertex in coneVertices)
        {
            Gizmos.DrawWireSphere(vertex, detectionRange);
        }
    }
}

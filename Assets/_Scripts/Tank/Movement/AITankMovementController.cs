using System.Collections.Generic;
using UnityEngine;

public class AITankMovementController : MovementController
{
    public float detectionRange = 10f;
    public float stoppingDistance = 2f; // Distance at which the AI stops moving towards the player

    private LinkedList<Collider2D> ignoreColliders = new LinkedList<Collider2D>();

    private Transform playerTank;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (DetectPlayerTank())
        {
            MoveTowardsPosition(playerTank.position);
        }
        AdjustDragBasedOnMovement();
    }

    private bool DetectPlayerTank()
    {
        Vector3 tankPosition = chassisRB.transform.position;
        float coneAngle = 90f; // Example cone angle, adjust as needed

        // Calculate the cone direction based on the turret's rotation
        Vector3 coneDirection = chassisRB.transform.right;

        List<Vector2> coneVertices = BuildDetectionCone(tankPosition, coneDirection, coneAngle);
        foreach (Vector2 vertex in coneVertices)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(vertex, detectionRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Vector2 directionToPlayer = collider.transform.position - tankPosition;
                    RaycastHit2D[] hits = Physics2D.RaycastAll(tankPosition, directionToPlayer.normalized, directionToPlayer.magnitude);

                    bool playerDetected = true;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (ignoreColliders.Contains(hit.collider))
                        {
                            continue; // Ignore specified colliders
                        }

                        if (!hit.collider.CompareTag("Player"))
                        {
                            playerDetected = false; // Obstacle detected
                            break;
                        }
                    }

                    if (playerDetected)
                    {
                        playerTank = collider.transform;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void MoveTowardsPosition(Vector2 targetPosition)
    {
        Vector2 TankPosition = new Vector2(chassisRB.transform.position.x, chassisRB.transform.position.y);
        Vector2 directionToTarget = targetPosition - TankPosition;
        float distanceToTarget = directionToTarget.magnitude;
        float angleToTarget = Vector2.SignedAngle(chassisRB.transform.right, directionToTarget);

        if (distanceToTarget > stoppingDistance)
        {
            if (Mathf.Abs(angleToTarget) > 10f)  // Adjust the threshold angle as needed
            {
                if (angleToTarget > 0)
                {
                    Movement.RotateTankLeft();
                }
                else
                {
                    Movement.RotateTankRight();
                }
            }
            else
            {
                // Once the tank is approximately facing the target, move forward
                Movement.MoveTankForward();
            }
        }
        else
        {
            // Stop moving if within stopping distance
            Movement.SetCanMove(false);
        }
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

    public override void Init()
    {
        base.Init();
        ignoreColliders = tankStatus.GetListOfCollider2D();
    }

    private void OnDrawGizmos()
    {
        if (chassisRB == null) return;

        Vector3 tankPosition = chassisRB.transform.position;
        float coneAngle = 90f; // Example cone angle, adjust as needed

        // Calculate the cone direction based on the turret's rotation
        Vector3 coneDirection = chassisRB.transform.right;

        List<Vector2> coneVertices = BuildDetectionCone(tankPosition, coneDirection, coneAngle);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(tankPosition, coneVertices[1]);
        Gizmos.DrawLine(tankPosition, coneVertices[2]);
        Gizmos.DrawLine(coneVertices[1], coneVertices[2]);

        // Optionally draw the detection range circles at the vertices
        foreach (Vector2 vertex in coneVertices)
        {
            Gizmos.DrawWireSphere(vertex, detectionRange);
        }
    }
}

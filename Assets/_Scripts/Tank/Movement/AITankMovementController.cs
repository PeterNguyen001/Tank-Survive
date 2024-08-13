using System.Collections.Generic;
using UnityEngine;

public class AITankMovementController : MovementController
{
    public float detectionRange = 10f;
    public float stoppingDistance = 2f; // Distance at which the AI stops moving towards the player



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
            MoveTowardsPlayerTank();
        }
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
                Debug.Log(collider.name);
                if (collider.CompareTag("Player"))
                {
                    playerTank = collider.transform;
                    return true;
                }
            }
        }

        return false;
    }

    private void MoveTowardsPlayerTank()
    {
        Vector3 directionToPlayer = playerTank.position - chassisRB.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector2.SignedAngle(chassisRB.transform.right, directionToPlayer);

        if (distanceToPlayer > stoppingDistance)
        {
            if (Mathf.Abs(angleToPlayer) > 10f)  // Adjust the threshold angle as needed
            {
                if (angleToPlayer > 0)
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
                // Once the tank is approximately facing the player, move forward
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

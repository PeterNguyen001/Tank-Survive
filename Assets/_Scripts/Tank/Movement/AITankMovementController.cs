using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITankMovementController : TankSubComponent
{
    private Movement AIMovement;

    public float horsepower = 50;

    private Rigidbody2D chassisRB;
    private Rigidbody2D leftTrackRB;
    private Rigidbody2D rightTrackRB;

    LinkedList<Tracks> trackList = new LinkedList<Tracks>();

    private Vector2 moveDirection;
    private Transform playerTank;

    public float detectionRange = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (DetectPlayerTank())
        {
  
            RotateTowardsPlayerTank();
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

    private void RotateTowardsPlayerTank()
    {
        Vector3 directionToPlayer = playerTank.position - chassisRB.transform.position;
        float angleToPlayer = Vector2.SignedAngle(chassisRB.transform.right, directionToPlayer);

        if (Mathf.Abs(angleToPlayer) > 10f)  // Adjust the threshold angle as needed
        {
            if (angleToPlayer > 0)
            {
                AIMovement.RotateTankLeft();
            }
            else
            {
                AIMovement.RotateTankRight();
            }
        }
        else
        {
            // Once the tank is approximately facing the player, stop rotating
            AIMovement.SetCanMove(false);
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
        chassisRB = Tools.FindComponentRecursively<Chassis>(transform).GetComponent<Rigidbody2D>();

        Tools.FindComponentsRecursively(transform, trackList);

        foreach (Tracks track in trackList)
        {
            if (track.name == "Left Track")
                leftTrackRB = track.gameObject.GetComponent<Rigidbody2D>();
            else if (track.name == "Right Track")
                rightTrackRB = track.gameObject.GetComponent<Rigidbody2D>();
        }
        AIMovement = new Movement(chassisRB, leftTrackRB, rightTrackRB, horsepower);
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

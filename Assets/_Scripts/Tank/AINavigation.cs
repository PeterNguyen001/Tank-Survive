using System.Collections.Generic;
using UnityEngine;

public class AINavigation : MovementController
{
    public float detectionRange = 10f;
    public float stoppingDistance = 2f; // Distance at which the AI stops moving towards each location
    public Queue<Vector2> movementLocations = new Queue<Vector2>(); // Queue of locations to move to

    private AISensor sensor;
    private Transform playerTank;
    private Vector2 currentTarget; // Current target location

    // Start is called before the first frame update
    void Start()
    {
        sensor = GetComponent<AISensor>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If there are no more locations, add a random one
        if (movementLocations.Count == 0)
        {
            AddRandomLocationNearAI(20);
        }

        // Move to the current target location
        MoveToCurrentLocation();

        AdjustDragBasedOnMovement();
    }

    private void MoveToCurrentLocation()
    {
        if (movementLocations.Count == 0)
        {
            return; // No more locations to move to
        }

        Vector2 TankPosition = new Vector2(chassisRB.transform.position.x, chassisRB.transform.position.y);
        Vector2 directionToTarget = currentTarget - TankPosition;
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
            // Reached the current target, dequeue the next one
            if (movementLocations.Count > 0)
            {
                currentTarget = movementLocations.Dequeue();
            }
        }
    }

    public void AddMovementLocation(Vector2 location)
    {
        movementLocations.Enqueue(location);

        // If the AI has no current target, immediately set the new target
        if (movementLocations.Count == 1)
        {
            currentTarget = location;
        }
    }

    public void ClearMovementLocations()
    {
        movementLocations.Clear();
    }

    public void AddRandomLocationNearAI(float radius)
    {
        Vector2 currentPosition = chassisRB.transform.position;
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // Get a random direction
        float randomDistance = Random.Range(0, radius); // Get a random distance within the specified radius

        Vector2 randomLocation = currentPosition + randomDirection * randomDistance;
        AddMovementLocation(randomLocation);

        Debug.Log($"Added random location: {randomLocation}");
    }

    public override void Init()
    {
        base.Init();
    }
}

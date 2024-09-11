using System.Collections.Generic;
using UnityEngine;

public class AINavigation : MovementController
{
    public float stoppingDistance = 0f; // Distance at which the AI stops moving towards each location
    public float stoppingAngle = 0f;
    public Queue<Vector2> movementLocations = new Queue<Vector2>(); // Queue of locations to move to

    private AISensor sensor;
    private Vector2 currentTarget; // Current target location

    // Reference to the grid-based pathfinding system
    public CustomNavMesh2D pathfindingSystem;

    // Steering parameters
    public float obstacleAvoidanceStrength = 1f; // Strength of obstacle avoidance steering

    // Start is called before the first frame update
    void Start()
    {
        sensor = GetComponent<AISensor>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (movementLocations.Count == 0)
        {
            AddRandomLocationNearAI(25);
        }

        MoveToCurrentLocation();

        AdjustDragBasedOnMovement();
    }

    private bool isAvoidingObstacle = false; // Track whether the tank is avoiding an obstacle

    private void MoveToCurrentLocation()
    {
        if (movementLocations.Count == 0)
        {
            return; // No more locations to move to
        }

        Vector2 tankPosition = new Vector2(chassisRB.transform.position.x, chassisRB.transform.position.y);
        Vector2 directionToTarget = currentTarget - tankPosition;
        float distanceToTargetLocation = directionToTarget.magnitude;
        float angleToTarget = Vector2.SignedAngle(chassisRB.transform.right, directionToTarget);

        // Get obstacle detection info
        DetectionInfo obstacleInfo = sensor.DetectObstacle();
        float obstacleDistance = obstacleInfo.distance;
        float obstacleDetectionRange = sensor.GetObstacleDetectionRange();
        Vector2 directionToObstacle = obstacleInfo.position - tankPosition;
        float angleToObstacle = Vector2.SignedAngle(chassisRB.transform.right, directionToObstacle);

        // Check if an obstacle is detected and if it's closer than the target
        if (obstacleInfo.position != Vector2.zero && obstacleDistance < obstacleDetectionRange)
        {
            // Enter obstacle avoidance mode
            isAvoidingObstacle = true;

            // Obstacle detected: Handle avoidance
            if (obstacleDistance < obstacleDetectionRange * 0.5f)
            {
                // If the obstacle is very close, move backward to avoid collision
                Debug.Log("Obstacle too close, moving backward");
                Movement.MoveTankBackward();
            }
            else if (obstacleDistance < obstacleDetectionRange * 0.70f && Mathf.Abs(angleToObstacle) <= 50)
            {
                // Rotate in place to avoid the obstacle
                Debug.Log(angleToObstacle);
                if (angleToObstacle > 0)
                {
                    Debug.Log("Obstacle detected, rotating right to avoid");
                    Movement.RotateTankRight();
                }
                else
                {
                    Debug.Log("Obstacle detected, rotating left to avoid");
                    Movement.RotateTankLeft();
                }
            }
            else
            {
                // Move forward-right or forward-left based on the obstacle position
                Vector2 avoidanceDirection = (tankPosition - obstacleInfo.position).normalized;
                if (avoidanceDirection.x > 0)
                {
                    Debug.Log("Avoiding obstacle by steering right");
                    Movement.MoveTankForwardRight();
                }
                else
                {
                    Debug.Log("Avoiding obstacle by steering left");
                    Movement.MoveTankForwardLeft();
                }
            }
        }
        else
        {
            // Exit obstacle avoidance mode
            isAvoidingObstacle = false;

            // No obstacle detected: proceed to the target
            if (distanceToTargetLocation > stoppingDistance)
            {
                if (!isAvoidingObstacle) // Only rotate to target if not avoiding an obstacle
                {
                    if (Mathf.Abs(angleToTarget) > stoppingAngle)
                    {
                        if (angleToTarget > 5)
                        {
                            Debug.Log("Rotate left to Target");
                            Movement.RotateTankLeft();
                        }
                        else if (angleToTarget < -5)
                        {
                            Debug.Log("Rotate right to Target");
                            Movement.RotateTankRight();
                        }
                        else if (angleToTarget > 0 && angleToTarget <= 5)
                        {
                            Debug.Log("Move forward left to Target");
                            Movement.MoveTankForwardLeft();
                        }
                        else if (angleToTarget < 0 && angleToTarget >= -5)
                        {
                            Debug.Log("Move forward right to Target");
                            Movement.MoveTankForwardRight();
                        }
                    }
                    else
                    {
                        Debug.Log("Moving forward to target");
                        Movement.MoveTankForward();
                    }
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

        // Use the pathfinding system to generate a path from the tank's position to the random location
        LinkedList<GridNode> path = pathfindingSystem.FindPath(currentPosition, randomLocation);

        if (path != null)
        {
            foreach (GridNode waypoint in path)
            {
                AddMovementLocation(waypoint.position);
            }
        }
    }

    public override void Init()
    {
        base.Init();
    }
}

//private void MoveToCurrentLocation()
//{
//    if (movementLocations.Count == 0)
//    {
//        return; // No more locations to move to
//    }

//    Vector2 tankPosition = new Vector2(chassisRB.transform.position.x, chassisRB.transform.position.y);
//    Vector2 directionToTarget = currentTarget - tankPosition;
//    float distanceToTargetLocation = directionToTarget.magnitude;
//    float angleToTarget = Vector2.SignedAngle(chassisRB.transform.right, directionToTarget);

//    if (distanceToTargetLocation > stoppingDistance)
//    {
//        Vector2 avoidanceDirection = GetObstacleAvoidanceDirectionFromSensor();
//        if (Mathf.Abs(angleToTarget) > stoppingAngle)
//        {
//            if (angleToTarget > 5)
//            {
//                Debug.Log("left");
//                Movement.RotateTankLeft();
//            }
//            else if (angleToTarget < -5)
//            {
//                Debug.Log("right");
//                Movement.RotateTankRight();
//            }
//            else if (angleToTarget > 0 && angleToTarget <= 5)
//            {
//                if (avoidanceDirection != Vector2.zero)
//                {
//                    Debug.Log("FL");
//                    Movement.MoveTankForwardLeft();
//                }
//                else if (avoidanceDirection.x > 0)
//                {
//                    Debug.Log("FR");
//                    Movement.MoveTankForwardRight();
//                }
//            }
//            else if (angleToTarget < 0 && angleToTarget >= -5)
//            {
//                if (avoidanceDirection != Vector2.zero)
//                {
//                    Debug.Log("FRR");
//                    Movement.MoveTankForwardRight();
//                }
//                else if (avoidanceDirection.x < 0)
//                {
//                    Debug.Log("FLL");
//                    Movement.MoveTankForwardLeft();
//                }
//            }
//        }
//        else
//        {
//            Debug.Log("forward");
//            Movement.MoveTankForward();
//        }
//    }
//    else
//    {
//        // Reached the current target, dequeue the next one
//        if (movementLocations.Count > 0)
//        {
//            currentTarget = movementLocations.Dequeue();
//        }
//    }
//}
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

    private void MoveToCurrentLocation()
    {
        if (movementLocations.Count == 0)
        {
            return; // No more locations to move to
        }

        Vector2 tankPosition = new Vector2(chassisRB.transform.position.x, chassisRB.transform.position.y);
        Vector2 directionToTarget = currentTarget - tankPosition;
        float distanceToTarget = directionToTarget.magnitude;
        float angleToTarget = Vector2.SignedAngle(chassisRB.transform.right, directionToTarget);

        // Get obstacle detection info
        DetectionInfo obstacleInfo = sensor.DetectObstacle();
        float obstacleDistance = obstacleInfo.distance;
        float obstacleDetectionRange = sensor.GetObstacleDetectionRange();

        // Check if an obstacle is detected and if it's closer than the target
        if (obstacleInfo.position != Vector2.zero && obstacleDistance < obstacleDetectionRange)
        {
            // Obstacle detected: Handle avoidance
            if (obstacleDistance < obstacleDetectionRange * 0.25f)
            {
                // If the obstacle is very close, move backward to avoid collision
                Debug.Log("Obstacle too close, moving backward");
                Movement.MoveTankBackward();
            }
            else if (obstacleDistance < obstacleDetectionRange * 0.5f)
            {
                // Rotate in place to avoid the obstacle
                Debug.Log("Obstacle detected, rotating to avoid");
                if (Vector2.SignedAngle(chassisRB.transform.right, obstacleInfo.position - tankPosition) > 0)
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
            // No obstacle detected: proceed to the target

            if (distanceToTarget > stoppingDistance)
            {
                Debug.Log("Rotate to Target");
                if (Mathf.Abs(angleToTarget) > stoppingAngle)
                {
                    if (angleToTarget > 5)
                    {
                        Movement.RotateTankLeft();
                    }
                    else if (angleToTarget < -5)
                    {
                        Movement.RotateTankRight();
                    }
                    else if (angleToTarget > 0 && angleToTarget <= 5)
                    {
                        Movement.MoveTankForwardLeft();
                    }
                    else if (angleToTarget < 0 && angleToTarget >= -5)
                    {
                        Movement.MoveTankForwardRight();
                    }
                }
                else
                {
                    Debug.Log("Moving forward to target");
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
    }


    private Vector2 GetObstacleAvoidanceDirectionFromSensor()
    {
        // Check if the sensor has detected an obstacle
        if (sensor.DetectObstacle().position != null)
        {
            Vector2 tankPosition = chassisRB.transform.position;
            Vector2 obstaclePosition = sensor.DetectObstacle().position;

            // Calculate the avoidance direction by steering away from the obstacle
            Vector2 avoidanceDirection = (tankPosition - obstaclePosition).normalized;

            return avoidanceDirection;
        }

        // No obstacles detected, return zero vector (no avoidance needed)
        return Vector2.zero;
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
//    float distanceToTarget = directionToTarget.magnitude;
//    float angleToTarget = Vector2.SignedAngle(chassisRB.transform.right, directionToTarget);

//    if (distanceToTarget > stoppingDistance)
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
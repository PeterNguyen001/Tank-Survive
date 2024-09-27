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

    // Steering and obstacle avoidance parameters
    public float obstacleAvoidanceStrength = 1f; // Strength of obstacle avoidance steering
    public float backwardDuration = 1f; // Time to move backward when obstacle is too close
    public float speedAdjustmentProximity = 0.75f; // Distance at which speed adjustment starts

    private float backwardTimer = 0f; // Timer to track backward movement

    private float forwardSpeed;
    private float  backwardSpeed;
    private float rotationSpeed;

    public bool debugMode =false;

    // Start is called before the first frame update
    void Start()
    {
        sensor = GetComponent<AISensor>();
        forwardSpeed = horsepower;
        backwardSpeed = horsepower / 2;
        rotationSpeed = horsepower / 3;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sensor.DetectForwardLeftRightObstacles();
        if (movementLocations.Count == 0 && !debugMode)
        {
            AddRandomLocationNearAI(25);
        }
        else
            sensor.DetectForwardLeftRightObstacles();

        MoveToCurrentLocation();
        AdjustDragBasedOnMovement();
    }

    private bool isAvoidingObstacle = false; // Track whether the tank is avoiding an obstacle
    private bool isMovingBackward = false; // Track backward movement status


    private void MoveToCurrentLocation()
    {
        if (movementLocations.Count == 0)
        {
            Debug.Log("No movement locations to process.");
            return; // No more locations to move to
        }

        Vector2 tankPosition = new Vector2(chassisRB.transform.position.x, chassisRB.transform.position.y);
        Vector2 directionToTarget = currentTarget - tankPosition;
        float distanceToTargetLocation = directionToTarget.magnitude;
        float angleToTarget = Vector2.SignedAngle(chassisRB.transform.right, directionToTarget);

        // Detect forward, left, and right obstacles
        var (forwardObstacle, leftObstacle, rightObstacle) = sensor.DetectForwardLeftRightObstacles();
        float forwardObstacleDistance = forwardObstacle.distance;
        float leftObstacleDistance = leftObstacle.distance;
        float rightObstacleDistance = rightObstacle.distance;
        float obstacleDetectionRange = sensor.GetObstacleDetectionRange();

        // Calculate weights for each side based on proximity
        float leftWeight = leftObstacleDistance > 0 ? Mathf.Clamp01(obstacleDetectionRange / leftObstacleDistance) : 0;
        float rightWeight = rightObstacleDistance > 0 ? Mathf.Clamp01(obstacleDetectionRange / rightObstacleDistance) : 0;
        float forwardWeight = forwardObstacleDistance > 0 ? Mathf.Clamp01(obstacleDetectionRange / forwardObstacleDistance) : 0;
        float totalWeight = forwardWeight + leftWeight + rightWeight;

        if (isMovingBackward)
        {
            backwardTimer += Time.fixedDeltaTime;
            if (backwardTimer >= backwardDuration)
            {
                isMovingBackward = false;
                backwardTimer = 0f;
            }
            else
            {
                // Adjust backward movement and steering control based on obstacle proximity
                float backwardSteeringDecision = (leftWeight * 0.55f - rightWeight * 0.55f) / totalWeight;

                if (Mathf.Abs(backwardSteeringDecision) > 0.3f)
                {
                    if (backwardSteeringDecision > 0)
                    {
                        Movement.MoveLeftTrackBackWard(backwardSpeed);
                    }
                    else
                    {
                        Movement.MoveRightTrackBackWard(backwardSpeed);
                    }
                }
                else
                {
                    Movement.MoveTankBackward();
                }
                return;
            }
        }

        if (distanceToTargetLocation > stoppingDistance)
        {
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
                else if (angleToTarget >= -5 && angleToTarget <= 5)
                {
                    if (totalWeight > 0)
                    {
                        float steeringDecision = (rightWeight - leftWeight) / totalWeight;

                        // Adjust speed based on obstacle distance
                        float leftSpeed = 1f;
                        float rightSpeed = 1f;

                        if (leftObstacleDistance < obstacleDetectionRange * 0.5f)
                        {
                            leftSpeed = 0.5f; // Slow down left track
                            isMovingBackward = true;
                            backwardTimer = 0f;
                            Movement.MoveTankBackward();
                            return;
                        }

                        if (rightObstacleDistance < obstacleDetectionRange * 0.5f)
                        {
                            rightSpeed = 0.5f; // Slow down right track
                            isMovingBackward = true;
                            backwardTimer = 0f;
                            Movement.MoveTankBackward();
                            return;
                        }

                        if (forwardWeight > 0.3f)
                        {
                            if (steeringDecision > 0)
                            {
                                Movement.MoveLeftTrackForward(leftSpeed);
                            }
                            else
                            {
                                Movement.MoveRightTrackForward(rightSpeed);
                            }
                        }
                        else
                        {
                            if (Mathf.Abs(steeringDecision) > 0.3f)
                            {
                                if (steeringDecision > 0)
                                {
                                    Movement.MoveLeftTrackForward(leftSpeed);
                                }
                                else
                                {
                                    Movement.MoveRightTrackForward(rightSpeed);
                                }
                            }
                            else
                            {
                                Movement.MoveTankForward();
                            }
                        }
                    }
                    else
                    {
                        if (angleToTarget > 0)
                        {
                            Movement.MoveTankForwardLeft();
                        }
                        else if (angleToTarget < 0)
                        {
                            Movement.MoveTankForwardRight();
                        }
                        else
                        {
                            Movement.MoveTankForward();
                        }
                    }
                }
            }
        }
        else
        {
            if (movementLocations.Count > 0)
            {
                currentTarget = movementLocations.Dequeue();
            }
        }
    }



    // Dynamic speed adjustment based on proximity to obstacles
    private void AdjustSpeedBasedOnProximity(float closestObstacleDistance)
    {
        float obstacleDetectionRange = sensor.GetObstacleDetectionRange();

        if (closestObstacleDistance < obstacleDetectionRange * 0.75f)
        {
            Debug.Log("Slowing down significantly due to close proximity to obstacle.");
            Movement.SetMoveSpeed(0.3f); // Slow down significantly when very close to obstacles
        }
        else if (closestObstacleDistance < obstacleDetectionRange * speedAdjustmentProximity)
        {
            Debug.Log("Slowing down moderately due to approaching obstacle.");
            Movement.SetMoveSpeed(0.5f); // Slow down moderately when approaching obstacles
        }
        else
        {
            Debug.Log("Full speed ahead, no nearby obstacles.");
            Movement.SetMoveSpeed(1f); // Full speed when no nearby obstacles
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

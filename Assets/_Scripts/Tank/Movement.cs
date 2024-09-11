using UnityEngine;

public class Movement
{
    
    private float forwardSpeed;
    private float backwardSpeed;
    private float forwardTurnSpeed;
    private float rotationSpeed;

    private Rigidbody2D chassisRB;
    private Rigidbody2D leftTrackRB;
    private Rigidbody2D rightTrackRB;
    public Movement(Rigidbody2D chassis, Rigidbody2D lTrack, Rigidbody2D rTrack, float horsepower)
    {
        chassisRB = chassis;
        leftTrackRB = lTrack;
        rightTrackRB = rTrack;

        leftTrackRB.GetComponent<FixedJoint2D>().connectedBody = chassisRB;
        rightTrackRB.GetComponent<FixedJoint2D>().connectedBody= chassisRB;

        forwardSpeed = horsepower;
        backwardSpeed = horsepower/2;
        forwardTurnSpeed = horsepower / 2;
        rotationSpeed = horsepower/3;

    }

    public void MoveTankForward()
    {
        MoveLeftTrackForward(forwardSpeed);
        MoveRightTrackForward(forwardSpeed);
    }

    public void MoveTankBackward()
    { 
        MoveLeftTrackBackWard(backwardSpeed);
        MoveRightTrackBackWard(backwardSpeed);
    }

    public void MovePlayerTank(Vector2 moveDirection)
    {
        if      (moveDirection == MoveType.MoveForward)       { MoveTankForward(); }
        else if (moveDirection == MoveType.MoveBackward)      { MoveTankBackward(); }
        else if (moveDirection == MoveType.RotateLeft)        { RotateTankLeft(); }
        else if (moveDirection == MoveType.RotateRight)       { RotateTankRight(); }
        else if (moveDirection == MoveType.MoveForwardLeft)   { MoveTankForwardLeft(); }
        else if (moveDirection == MoveType.MoveForwardRight)  { MoveTankForwardRight(); }
        else if (moveDirection == MoveType.MoveBackwardLeft)  { MoveTankBackwardLeft(); }
        else if (moveDirection == MoveType.MoveBackwardRight) { MoveTankBackwardRight(); }

        else if (moveDirection == MoveType.Brake ) { BrakeTank(); }
    }

    public void SetMoveSpeed(float speed)
    {
        forwardSpeed = speed;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void SetCanMove(bool move)
    {
        if (move)
        {
            chassisRB.velocity = Vector2.zero;
            chassisRB.angularVelocity = 0f;
        }
    }

    public void MoveLeftTrackForward(float speed)
    {
        Vector2 moveForce = leftTrackRB.transform.right * speed;

        leftTrackRB.AddForce(moveForce);
    }

    public void MoveRightTrackForward(float speed)
    {
        Vector2 moveForce = rightTrackRB.transform.right * speed;

        rightTrackRB.AddForce(moveForce);
    }

    public void MoveLeftTrackBackWard(float speed)
    {
        Vector2 moveForce = leftTrackRB.transform.right * -speed;

        leftTrackRB.AddForce(moveForce);
    }

    public void MoveRightTrackBackWard(float speed)
    {
        Vector2 moveForce = rightTrackRB.transform.right * -speed;

        rightTrackRB.AddForce(moveForce);
    }
    public void RotateTankLeft()
    {
        MoveLeftTrackBackWard(rotationSpeed);
        MoveRightTrackForward(rotationSpeed);
    }
    public void RotateTankRight() 
    { 
        MoveLeftTrackForward(rotationSpeed);
        MoveRightTrackBackWard(rotationSpeed);
    }

    public void MoveTankForwardLeft()
    {
        MoveLeftTrackForward(rotationSpeed);
        MoveRightTrackForward(forwardTurnSpeed);
    }

    public void MoveTankForwardRight() 
    {
        MoveLeftTrackForward(forwardTurnSpeed);
        MoveRightTrackForward(rotationSpeed);
    }

    public void MoveTankBackwardLeft() 
    {
        MoveLeftTrackBackWard(rotationSpeed);
        MoveRightTrackBackWard(backwardSpeed);
    }
    public void MoveTankBackwardRight() 
    { 
        MoveLeftTrackBackWard(backwardSpeed);
        MoveRightTrackBackWard(rotationSpeed);
    }

    public void BrakeTank()
    {

        if (chassisRB.velocity.magnitude > 0.2f)
        {
            // Calculate the opposite force based on the current velocity of the tracks
            Vector2 oppositeForceLeft = -leftTrackRB.velocity.normalized * backwardSpeed * 2f;
            Vector2 oppositeForceRight = -rightTrackRB.velocity.normalized * backwardSpeed * 2f;

            // Apply the opposite force to slow down the tank
            leftTrackRB.AddForce(oppositeForceLeft, ForceMode2D.Force);
            rightTrackRB.AddForce(oppositeForceRight, ForceMode2D.Force);
        }
    }
}

public static class MoveType
{
    public static Vector2 MoveForward  = new Vector2(0, 1);
    public static Vector2 MoveBackward = new Vector2(0,-1);

    public static Vector2 RotateLeft   = new Vector2(-1,0);
    public static Vector2 RotateRight  = new Vector2(1 ,0);

    public static Vector2 MoveForwardLeft  = new Vector2(-1, 1);
    public static Vector2 MoveForwardRight = new Vector2(1 , 1);

    public static Vector2 MoveBackwardLeft  = new  Vector2(-1, -1);
    public static Vector2 MoveBackwardRight = new  Vector2(1 , -1);

    public static Vector2 Brake = new Vector2(0,0);
}

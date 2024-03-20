using UnityEngine;

public class Movement
{
    private Rigidbody2D playerRigidBody;
    private float forwardSpeed;
    private float backwardSpeed;
    private float rotationSpeed;


    private Rigidbody2D leftTrackRB;
    private Rigidbody2D rightTrackRB;
    public Movement(Rigidbody2D lTrack, Rigidbody2D rTrack, float horsepower)
    {
        //playerRigidBody = rb;
        leftTrackRB = lTrack;
        rightTrackRB = rTrack;

        forwardSpeed = horsepower;
        backwardSpeed = horsepower/2;
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
            playerRigidBody.velocity = Vector2.zero;
            playerRigidBody.angularVelocity = 0f;
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
        MoveRightTrackForward(forwardSpeed);
    }

    public void MoveTankForwardRight() 
    {
        Debug.Log("fr");
        MoveLeftTrackForward(forwardSpeed);
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


}

public class MoveType
{
    public static Vector2 MoveForward  = new Vector2(0, 1);
    public static Vector2 MoveBackward = new Vector2(0,-1);

    public static Vector2 RotateLeft   = new Vector2(-1,0);
    public static Vector2 RotateRight  = new Vector2(1 ,0);

    public static Vector2 MoveForwardLeft  = new Vector2(-1, 1);
    public static Vector2 MoveForwardRight = new Vector2(1 , 1);

    public static Vector2 MoveBackwardLeft = new  Vector2(-1, -1);
    public static Vector2 MoveBackwardRight = new Vector2(1 , -1);
}

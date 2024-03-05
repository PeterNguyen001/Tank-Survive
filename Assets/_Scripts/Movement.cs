using UnityEngine;

public class Movement
{
    private Rigidbody2D playerRigidBody;
    public float forwardSpeed = 50;
    public float backwardSpeed = 25;
    public float rotationSpeed = 180f;


    private Rigidbody2D leftTrackRB;
    private Rigidbody2D rightTrackRB;
    public Movement(Rigidbody2D lTrack, Rigidbody2D rTrack)
    {
        //playerRigidBody = rb;
        leftTrackRB = lTrack;
        rightTrackRB = rTrack;
    }

    public void MoveTankForwardAndBackward(float verticalInput)
    {
        // Calculate the force for forward and backward movement
        //Vector2 moveForce = playerRigidBody.transform.right * verticalInput * moveSpeed;


        // Apply the force
        //playerRigidBody.AddForce(moveForce);
    }

    public void MoveTankForward()
    {
        MoveLeftTrackForward();
        MoveRightTrackForward();
    }

    public void MoveTankBackward() 
    { 
        MoveLeftTrackBackWard();
        MoveRightTrackBackWard();
    }

    public void RotateTank(float horizontalInput)
    {
        //float rotationAmount = -horizontalInput * rotationSpeed * Time.fixedDeltaTime;
        //playerRigidBody.AddTorque(rotationAmount);
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

    public void MoveLeftTrackForward()
    {
        Vector2 moveForce = leftTrackRB.transform.right * forwardSpeed;

        leftTrackRB.AddForce(moveForce);
    }

    public void MoveRightTrackForward()
    {
        Vector2 moveForce = rightTrackRB.transform.right * forwardSpeed;

        rightTrackRB.AddForce(moveForce);
    }

    public void MoveLeftTrackBackWard()
    {
        Vector2 moveForce = leftTrackRB.transform.right * backwardSpeed;

        leftTrackRB.AddForce(moveForce);
    }

    public void MoveRightTrackBackWard()
    {
        Vector2 moveForce = rightTrackRB.transform.right * backwardSpeed;

        rightTrackRB.AddForce(moveForce);
    }
}
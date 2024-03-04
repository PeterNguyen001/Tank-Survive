using UnityEngine;

public class Movement
{
    private Rigidbody2D playerRigidBody;
    public float moveSpeed = 100;
    public float rotationSpeed = 180f;
    public float lateralDrag = 50;


    public Movement(Rigidbody2D rb)
    {
        playerRigidBody = rb;
    }

    public void MoveTankForwardAndBackward(float verticalInput)
    {
        // Calculate the force for forward and backward movement
        Vector2 moveForce = playerRigidBody.transform.right * verticalInput * moveSpeed;


        // Apply the force
        playerRigidBody.AddForce(moveForce);
    }



    public void RotateTank(float horizontalInput)
    {
        float rotationAmount = -horizontalInput * rotationSpeed * Time.fixedDeltaTime;
        playerRigidBody.AddTorque(rotationAmount);
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
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
}
using UnityEngine;

public class Movement
{
    private Rigidbody2D playerRigidBody;
    private float moveSpeed = 20f;
    private float rotationSpeed = 180f;

    public Movement(Rigidbody2D rb)
    {
        playerRigidBody = rb;
    }

    public void MoveTankForwardAndBackward(float verticalInput)
    {
        Vector2 moveForce = playerRigidBody.transform.right * verticalInput * moveSpeed;
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
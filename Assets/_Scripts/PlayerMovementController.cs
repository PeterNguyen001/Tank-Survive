using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private Movement PlayerMovement;
    [SerializeField]
    private Rigidbody2D leftTrack;
    [SerializeField]
    private Rigidbody2D rightTrack;

    private int forwardDirection;
    private int backwardDirection;
    void Start()
    {
        PlayerMovement = new Movement(leftTrack, rightTrack);
    }

    void FixedUpdate()
    {
        //float verticalInput = Input.GetAxis("Vertical");
        //float horizontalInput = Input.GetAxis("Horizontal");

        //PlayerMovement.MoveTankForwardAndBackward(verticalInput);
        //PlayerMovement.RotateTank(horizontalInput);
    }

    public void MoveForward(InputAction.CallbackContext context)
    {
        float moveDirection = context.ReadValue<float>();

        Debug.Log(moveDirection);
    }

}


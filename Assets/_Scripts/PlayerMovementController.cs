using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private Movement PlayerMovement;
    [SerializeField]
    private Rigidbody2D leftTrack;
    [SerializeField]
    private Rigidbody2D rightTrack;

    private float forwardDirection;
    private float backwardDirection;
    private Vector2 moveDirection;
    void Start()
    {
        PlayerMovement = new Movement(leftTrack, rightTrack);
    }

    void FixedUpdate()
    {

        PlayerMovement.MovePlayerTank(moveDirection);
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();

        Debug.Log(moveDirection);
    }



}


using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private Movement PlayerMovement;

    private Rigidbody2D leftTrack;

    private Rigidbody2D rightTrack;

    private float forwardDirection;
    private float backwardDirection;
    private Vector2 moveDirection;
    void Start()
    {
        Initializetracks();
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

    private void Initializetracks()
    {
        leftTrack = transform.Find("Tracks/Left Track").GetComponent<Rigidbody2D>();
        rightTrack = transform.Find("Tracks/Right Track").GetComponent<Rigidbody2D>();
    }

}


using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private Movement PlayerMovement;

    public float horsepower = 50;

    private Rigidbody2D chassis;
    private Rigidbody2D leftTrack;
    private Rigidbody2D rightTrack;

    private Vector2 moveDirection;


    void Start()
    {
        InitializeRigidBody();
        PlayerMovement = new Movement(chassis, leftTrack, rightTrack, horsepower);
    }

    void FixedUpdate()
    {

        PlayerMovement.MovePlayerTank(moveDirection);
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    private void InitializeRigidBody()
    {
        chassis = GetComponent<Rigidbody2D>();
        leftTrack = transform.Find("Tracks/Left Track").GetComponent<Rigidbody2D>();
        rightTrack = transform.Find("Tracks/Right Track").GetComponent<Rigidbody2D>();
    }

 

}


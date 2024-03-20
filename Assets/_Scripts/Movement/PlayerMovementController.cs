using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private Movement PlayerMovement;

    public float horsepower = 50;

    private Rigidbody2D leftTrack;
    private Rigidbody2D rightTrack;

    private Vector2 moveDirection;


    void Start()
    {
        Initializetracks();
        PlayerMovement = new Movement(leftTrack, rightTrack, horsepower);
    }

    void FixedUpdate()
    {

        PlayerMovement.MovePlayerTank(moveDirection);
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    private void Initializetracks()
    {
        leftTrack = transform.Find("Left Track").GetComponent<Rigidbody2D>();
        rightTrack = transform.Find("Right Track").GetComponent<Rigidbody2D>();
    }

    //public float GetSpeedKMH()
    //{
    //    // Calculate the speed in meters per second (m/s)
    //    float speedMS = ;

    //    // Convert speed from meters per second (m/s) to kilometers per hour (km/h)
    //    float speedKMH = speedMS * 3.6f; // 1 m/s = 3.6 km/h

    //    return speedKMH;
    //}

}


using UnityEngine;

public class TankController : MonoBehaviour
{
    private Movement PlayerMovement;

    void Start()
    {
        PlayerMovement = new Movement(GetComponent<Rigidbody2D>());
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        PlayerMovement.MoveTankForwardAndBackward(verticalInput);
        PlayerMovement.RotateTank(horizontalInput);
    }
}

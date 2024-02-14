using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement playerMovement;

    void Start()
    {

        playerMovement = new Movement(GetComponent<Rigidbody2D>());
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        playerMovement.MoveTankForwardAndBackward(verticalInput);
        playerMovement.RotateTank(horizontalInput);
    }
}

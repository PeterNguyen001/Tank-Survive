using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;  // Adjust the speed as needed
    public float rotationSpeed = 180f;  // Adjust the rotation speed as needed

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveTank();
        RotateTank();
    }

    private void MoveTank()
    {
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 moveForce = transform.up * verticalInput * moveSpeed;

        // Apply force to move the tank
        rb.AddForce(moveForce);
    }

    private void RotateTank()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float rotationAmount = -horizontalInput * rotationSpeed * Time.fixedDeltaTime;

        // Apply torque to rotate the tank
        rb.AddTorque(rotationAmount);
    }
}

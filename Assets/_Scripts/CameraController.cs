using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private GameObject playerTank;
    public float followSpeed = 5f;
    public float edgeMoveSpeed = 5f;
    public float edgeMoveThreshold = 50f;

    private Vector2 mousePosition = Vector2.zero;
    private void Start()
    {
        playerTank = GameObject.Find("Player Tank");
        if (playerTank == null)
        {
            Debug.LogError("Player tank reference is not set in the CameraController script.");
            enabled = false;
        }
    }

    private void Update()
    {
        // Follow the player tank
        FollowPlayer();

        MoveTowardsMouse(mousePosition);
        // Move towards mouse position when close to the screen edge
    }

    private void FollowPlayer()
    {
        // Calculate the target position for the camera to follow the player tank
        Vector3 targetPosition = playerTank.transform.position;
        targetPosition.z = transform.position.z; // Maintain the camera's original z position

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    private void MoveTowardsMouse(Vector2 mousePosition)
    {

        // Get screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate movement direction based on mouse position and screen dimensions
        Vector3 moveDirection = Vector3.zero;
        if (mousePosition.x < edgeMoveThreshold)
        {
            moveDirection += Vector3.left;
        }
        else if (mousePosition.x > screenWidth - edgeMoveThreshold)
        {
            moveDirection += Vector3.right;
        }

        if (mousePosition.y < edgeMoveThreshold)
        {
            moveDirection += Vector3.down;
        }
        else if (mousePosition.y > screenHeight - edgeMoveThreshold)
        {
            moveDirection += Vector3.up;
        }

        // Normalize move direction to maintain consistent speed regardless of direction
        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();
            transform.position += moveDirection * edgeMoveSpeed * Time.deltaTime;
        }
    }

    public void MoveMouse(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
}

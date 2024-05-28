using System.Collections.Generic;
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

    LinkedList<Tracks> trackList = new LinkedList<Tracks>();    

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

        Tools.FindComponentsRecursively(transform, trackList);
        
        foreach (Tracks track in trackList) 
        {
            if (track.name == "Left Track")
                leftTrack = track.gameObject.GetComponent<Rigidbody2D>();
            else if(track.name == "Right Track")
                rightTrack = track.gameObject.GetComponent<Rigidbody2D>();
        }

    }

 

}


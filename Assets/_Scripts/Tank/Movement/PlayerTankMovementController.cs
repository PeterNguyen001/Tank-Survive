using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTankMovementController : TankSubComponent
{
    private Movement PlayerMovement;

    public float horsepower = 50;

    private Rigidbody2D chassisRB;
    private Rigidbody2D leftTrackRB;
    private Rigidbody2D rightTrackRB;

    LinkedList<Tracks> trackList = new LinkedList<Tracks>();    

    private Vector2 moveDirection;


    void Start()
    {
        
    }

    void FixedUpdate()
    {

        PlayerMovement.MovePlayerTank(moveDirection);
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public override void Init()
    {
        chassisRB = Tools.FindComponentRecursively<Chassis>(transform).GetComponent<Rigidbody2D>();

        Tools.FindComponentsRecursively(transform, trackList);
        
        foreach (Tracks track in trackList) 
        {
            if (track.name == "Left Track")
                leftTrackRB = track.gameObject.GetComponent<Rigidbody2D>();
            else if(track.name == "Right Track")
                rightTrackRB = track.gameObject.GetComponent<Rigidbody2D>();
        }
        PlayerMovement = new Movement(chassisRB, leftTrackRB, rightTrackRB, horsepower);
    }

 

}


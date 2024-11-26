using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTankMovementController : MovementController
{

    private Vector2 moveDirection;


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
        Movement.MovePlayerTank(moveDirection);
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public override void Init()
    {
        base.Init();
    }

 

}


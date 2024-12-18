using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class MovementController : TankSubComponent
{
    protected Movement Movement;

    public float highDrag = 5f;
    public float lowDrag = 0.0f;

    float maxSpeed;

    public float horsepower = 50;

    private TankEngine tankEngine;

    protected Rigidbody2D chassisRB;
    private Rigidbody2D leftTrackRB;
    private Rigidbody2D rightTrackRB;

    LinkedList<Tracks> trackList = new LinkedList<Tracks>();

    // Start is called before the first frame update


    // Update is called once per frame
    protected void FixedUpdate()
    {
        AdjustDragBasedOnMovement();

        EnforceMaxSpeed();
    }

    public override void Init()
    {


        chassisRB = Tools.FindComponentRecursively<Chassis>(transform).GetComponent<Rigidbody2D>();

        // Ensure drag is set initially
        SetDrag(lowDrag);

        Tools.FindComponentsRecursively(transform, trackList);

        foreach (Tracks track in trackList)
        {
            if (track.name == "Left Track")
                leftTrackRB = track.gameObject.GetComponent<Rigidbody2D>();
            else if (track.name == "Right Track")
                rightTrackRB = track.gameObject.GetComponent<Rigidbody2D>();
        }
        tankEngine = Tools.FindComponentRecursively<TankEngine>(transform);
        horsepower = tankEngine.TankEngineData.horsePower;
        maxSpeed = tankEngine.TankEngineData.maxSpeed;
        Movement = new Movement(chassisRB, leftTrackRB, rightTrackRB, horsepower);
    }

    protected void AdjustDragBasedOnMovement()
    {
        if (IsMovingForwardOrBackward())
        {
            SetDrag(lowDrag);
        }
        else
        {
            SetDrag(highDrag);
        }
    }

    private bool IsMovingForwardOrBackward()
    {
        Vector2 velocity = chassisRB.velocity;
        Vector2 tankRight = chassisRB.transform.right;

        float dotProduct = Vector2.Dot(velocity.normalized, tankRight);

        // Consider the tank to be moving forward or backward if the dot product is close to 1 or -1
        return Mathf.Abs(dotProduct) > 0.99f;
    }

    private void SetDrag(float dragValue)
    {
        if (chassisRB.drag != dragValue)
            chassisRB.drag = dragValue;
    }

    private void EnforceMaxSpeed()
    {
        // Check if velocity exceeds max speed
        if (chassisRB.velocity.magnitude > maxSpeed)
        {
            // Clamp the velocity to max speed
            chassisRB.velocity = chassisRB.velocity.normalized * maxSpeed;
        }
    }
}

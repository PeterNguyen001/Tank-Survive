using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class MovementController : TankSubComponent
{
    protected Movement Movement;

    public float highDrag = 5f;
    public float lowDrag = 0.0f;

    public float horsepower = 50;

    protected Rigidbody2D chassisRB;
    private Rigidbody2D leftTrackRB;
    private Rigidbody2D rightTrackRB;

    LinkedList<Tracks> trackList = new LinkedList<Tracks>();

    // Start is called before the first frame update
    void Start()
    {
        // Ensure drag is set initially
        SetDrag(lowDrag);
    }

    // Update is called once per frame
    void Update()
    {
        //AdjustDragBasedOnMovement();
    }

    public override void Init()
    {
        chassisRB = Tools.FindComponentRecursively<Chassis>(transform).GetComponent<Rigidbody2D>();

        Tools.FindComponentsRecursively(transform, trackList);

        foreach (Tracks track in trackList)
        {
            if (track.name == "Left Track")
                leftTrackRB = track.gameObject.GetComponent<Rigidbody2D>();
            else if (track.name == "Right Track")
                rightTrackRB = track.gameObject.GetComponent<Rigidbody2D>();
        }
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
        Debug.Log(dotProduct);

        // Consider the tank to be moving forward or backward if the dot product is close to 1 or -1
        return Mathf.Abs(dotProduct) > 0.99f;
    }

    private void SetDrag(float dragValue)
    {
        if (chassisRB.drag != dragValue)
            chassisRB.drag = dragValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class MovementController : TankSubComponent
{
    protected Movement Movement;

    public float horsepower = 50;

    protected Rigidbody2D chassisRB;
    private Rigidbody2D leftTrackRB;
    private Rigidbody2D rightTrackRB;

    LinkedList<Tracks> trackList = new LinkedList<Tracks>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

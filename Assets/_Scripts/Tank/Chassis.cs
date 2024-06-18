using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chassis : TankPart
{
    public ChassisData chassisData;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        tankPart = chassisData;
        InitializeCollider();
    }

    public void InitializeCollider()
    {
        PolygonCollider2D meshCollider = GetComponent<PolygonCollider2D>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<PolygonCollider2D>();
        }
    }
}

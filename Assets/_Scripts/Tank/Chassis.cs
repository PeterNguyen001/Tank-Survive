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
    }
}

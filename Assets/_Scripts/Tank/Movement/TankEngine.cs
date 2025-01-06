using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEngine : TankPart
{
    [SerializeField]    
    TankEngineData tankEngineData;

    public TankEngineData TankEngineData { get => tankEngineData; set => tankEngineData = value; }
    // Start is called before the first frame update

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        tankPart = tankEngineData;
    }
}

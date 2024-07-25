using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPart : MonoBehaviour
{
    protected TankPartData tankPart;
    private bool isDisable;

    public bool IsDisable { get => isDisable; set => isDisable = value; }

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        // Initialization logic for TankPart can go here
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TankPartData GetTankPart() { return tankPart; }
}

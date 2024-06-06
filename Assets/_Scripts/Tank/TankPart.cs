using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPart : MonoBehaviour
{
    protected Item tankPart;
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

    public Item GetTankPart() { return tankPart; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITankDesignNode : UIStateNode
{
    TankPartManager tankPartManager;
    public UITankDesignNode(GameObject stateObject) : base(stateObject)
    {
    }

    public override void Activate()
    {
        base.Activate();
        Debug.Log("a");
        tankPartManager = Tools.FindComponentRecursively<TankPartManager>(transform);
        tankPartManager.FillListOfSubComponent();
        foreach (TankSubComponent subComponent in tankPartManager.SubComponentList)
        {
            
            subComponent.enabled = false;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

}

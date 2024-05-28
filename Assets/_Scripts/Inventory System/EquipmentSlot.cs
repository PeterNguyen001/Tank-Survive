using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    // Start is called before the first frame update

    public override int Count
    {
        get { return base.Count; }
        set
        {
            base.Count = value;
            UpdateTankPart(); // Call UpdateTankPart whenever the count changes
        }
    }
    public void UpdateTankPart()
    {
        TankBuilder.BuildTankPart(this);
    }
}

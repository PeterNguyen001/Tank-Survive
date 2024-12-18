using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITankDesignNode : UIStateNode
{
    //TankPartManager tankPartManager;
    public UITankDesignNode(GameObject stateObject) : base(stateObject)
    {
    }

    public override void Deactivate()
    {
        if (UIStateMachine.Instance.CurrentState == this)
        {
            AmmoInventory inventory = Tools.FindComponentRecursively<AmmoInventory>(this.transform);
            List<ItemSlot> slots = inventory.GetListOfAmmoSlot();

            GameManager.Instance.PlayerTank.PartManager.Loader.AddAmmos(slots);
        }
        base.Deactivate();
    }
}

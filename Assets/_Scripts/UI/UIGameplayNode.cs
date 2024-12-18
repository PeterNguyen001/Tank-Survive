using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayNode : UIStateNode
{
    public UIGameplayNode(GameObject stateObject) : base(stateObject) {}

    public override void Activate()
    {
        base.Activate();
        GameManager.Instance.PlayerTank.transform.parent = this.gameObject.transform;
        GameManager.Instance.PlayerTank.enabled = true;
        GameManager.Instance.PlayerTank.GetComponent<TankPartManager>().ActivateTank();
    }

}

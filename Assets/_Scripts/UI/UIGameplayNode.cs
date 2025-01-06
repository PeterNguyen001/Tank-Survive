using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayNode : UIStateNode
{
    [SerializeField]
    private GameObject EnemyContainer;
    
    public UIGameplayNode(GameObject stateObject) : base(stateObject) {}

    public override void Activate()
    {
        LinkedList<TankPartManager> enemyPartManagerList = new LinkedList<TankPartManager>();
        base.Activate();
        GameManager.Instance.PlayerTank.transform.parent = this.transform;
        GameManager.Instance.ActivatePlayer();
        Tools.FindComponentsRecursively(EnemyContainer.transform, enemyPartManagerList, true);
        foreach (TankPartManager enemyTank in enemyPartManagerList)
        {
            enemyTank.ActivateTank();
        }
    }

}

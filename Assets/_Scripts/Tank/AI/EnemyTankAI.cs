using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankAI : EnemyAI
{
    protected TankPartManager tankPartManager;

    DetectionInfo playerDetectionInfo;
    AITankNavigation tankNavigation;
    TurretController turretController;

    protected Vector2 enemyLastKnowPosition;
    public float lastKnowPositionOffset;

    public AITankNavigation TankNavigation { get => tankNavigation; set => tankNavigation = value; }
    public TurretController TurretController { get => turretController; set => turretController = value; }
    public DetectionInfo PlayerDetectionInfo { get => playerDetectionInfo; set => playerDetectionInfo = value; }

    public override void Init()
    {
        tankPartManager = GetComponent<TankPartManager>();
    }
    void Start()
    {
        sensor = tankPartManager.AISensor;
        tankNavigation = tankPartManager.AITankNavigation;
        turretController = tankPartManager.TurretController;
        stateMachine = new AIStateMachine();
        stateMachine.ChangeState(new IdleState(this));  // Start in idle state
    }

    void FixedUpdate()
    {
        playerDetectionInfo = sensor.DetectPlayer(5);
        stateMachine.Update();

        Vector2 enemyNewPosition = playerDetectionInfo.position;
        if (Vector2.Distance(enemyLastKnowPosition, enemyNewPosition) > lastKnowPositionOffset && enemyNewPosition != Vector2.zero)
        {
            // Update the last known position if the new position is far enough
            enemyLastKnowPosition = enemyNewPosition;
        }
        if (playerDetectionInfo.tag == "Player")
        {
           
            SetEnemyLastKnowPosition(playerDetectionInfo.position);
            stateMachine.ChangeState(new TankAttackEnemy(this)); // Chase player if within range
        }
        else
        {
            stateMachine.ChangeState(new TankGoToLastKnowPosition(this)); // Otherwise, patrol
        }
    }




    public void SetEnemyLastKnowPosition(Vector2 newPosition)
    {
            enemyLastKnowPosition = newPosition; 
    }

    public Vector2 GetEnemyLastKnowPosition()
    {
        return enemyLastKnowPosition;
    }


}
public class TankGoToLastKnowPosition : TankState
{
    public TankGoToLastKnowPosition(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter()
    {
        if(tankAI.GetEnemyLastKnowPosition() != Vector2.zero)
        {
            Debug.Log("Tank is going to last seen");
            tankAI.TankNavigation.AddMovementLocation(tankAI.GetEnemyLastKnowPosition());
            tankAI.SetEnemyLastKnowPosition(Vector2.zero);
        }
        // Custom patrol behavior for tanks
    }
    public override void Execute()
    {
        // Custom patrol behavior for tanks
        Debug.Log("Tank is checking");
        tankAI.TankNavigation.MoveToTargetLocation();
        // Add movement or other tank-specific logic here
    }
}

public class TankChaseState : TankState
{
  public TankChaseState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter()
    {
        Debug.Log("Tank started chasing");
        tankAI.TankNavigation.AddMovementLocation(tankAI.PlayerDetectionInfo.position);
    }

    public override void Execute()
    {
        // Custom patrol behavior for tanks
        Debug.Log("Tank is chasing");
        tankAI.TankNavigation.MoveToTargetLocation();
        // Add movement or other tank-specific logic here
    }

    public override void Exit()
    {
        tankAI.TankNavigation.ClearMovementQueue();
    }
}

public class TankAttackEnemy : TankState
{
    public TankAttackEnemy(EnemyAI enemyAI) : base(enemyAI) { }
    
    public override void Execute() 
    {
        tankAI.TankNavigation.RotateToFaceTarget(tankAI.GetEnemyLastKnowPosition());
        tankAI.TurretController.AttackTaget();
    }
}
public class TankPatrol : TankState
{
    public TankPatrol(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter() { }
}


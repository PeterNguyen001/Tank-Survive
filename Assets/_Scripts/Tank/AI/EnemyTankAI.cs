using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankAI : EnemyAI
{
    DetectionInfo playerDetectionInfo;
    private AITankNavigation navigation;
    void Start()
    {
        sensor = GetComponent<AISensor>();
        navigation = GetComponent<AITankNavigation>();
        stateMachine = new AIStateMachine();
        stateMachine.ChangeState(new IdleState(this));  // Start in idle state
    }

    void Update()
    {
        playerDetectionInfo = sensor.DetectPlayer(5);
        stateMachine.Update();

        if (playerDetectionInfo.tag != "")
        {
            //stateMachine.ChangeState(new TankChaseState(this)); // Chase player if within range
        }
        else
        {
            stateMachine.ChangeState(new TankPatrolState(this)); // Otherwise, patrol
        }
    }

    public DetectionInfo GetPlayerDetectionInfo()
    {
        return playerDetectionInfo;
    }

    public void SetTargetLoaction( Vector2 targetLoaction)
    {
        navigation.SetTargetLocation(targetLoaction);
    }

    public void ChaseEnemy()
    {
        if (playerDetectionInfo.tag != "")
        {
            navigation.MoveToTargetLocation();
        }
    }

}
public class TankPatrolState : TankState
{
    public TankPatrolState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Execute()
    {
        // Custom patrol behavior for tanks
        Debug.Log("Tank is patrolling");
        // Add movement or other tank-specific logic here
    }
}

public class TankChaseState : TankState
{
  public TankChaseState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter()
    {
        Debug.Log("Tank started chasing");
        Debug.Log(tankAI.GetPlayerDetectionInfo().position);
        tankAI.SetTargetLoaction(tankAI.GetPlayerDetectionInfo().position);
    }

    public override void Execute()
    {
        // Custom patrol behavior for tanks
        Debug.Log("Tank is chasing");
        tankAI.ChaseEnemy();
        // Add movement or other tank-specific logic here
    }
}


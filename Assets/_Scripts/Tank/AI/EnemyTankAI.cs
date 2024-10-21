using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankAI : EnemyAI
{
    DetectionInfo playerDetectionInfo;
    private AITankNavigation navigation;
    protected Vector2 enemyLastKnowPosition;
    public float lastKnowPositionOffset;
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

        if (playerDetectionInfo.tag == "Player")
        {
            SetEnemyLastKnowPosition(playerDetectionInfo.position);
            stateMachine.ChangeState(new TankChaseState(this)); // Chase player if within range
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
            navigation.MoveToTargetLocation();
    }

    public void StopMoving()
    {
        navigation.ClearMovementQueue();
    }

    public void SetEnemyLastKnowPosition(Vector2 newPosition)
    {
        // Check the distance between the current last known position and the new position
        if (Vector2.Distance(enemyLastKnowPosition, newPosition) > lastKnowPositionOffset && newPosition != Vector2.zero)
        {
            // Update the last known position if the new position is far enough
            enemyLastKnowPosition = newPosition;
        }
    }

    public Vector2 GetEnemyLastKnowPosition()
    {
        return enemyLastKnowPosition;
    }

}
public class TankPatrolState : TankState
{
    public TankPatrolState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter()
    {
        if(tankAI.GetEnemyLastKnowPosition() != Vector2.zero)
        {
            Debug.Log("Tank is patrolling last seen");
            tankAI.SetTargetLoaction(tankAI.GetEnemyLastKnowPosition());
        }
        // Custom patrol behavior for tanks
        Debug.Log("Tank is patrolling");
        // Add movement or other tank-specific logic here
    }
    public override void Execute()
    {
        // Custom patrol behavior for tanks
        Debug.Log("Tank is checking");
        tankAI.ChaseEnemy();
        // Add movement or other tank-specific logic here
    }
}

public class TankChaseState : TankState
{
  public TankChaseState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter()
    {
        Debug.Log("Tank started chasing");
        tankAI.SetTargetLoaction(tankAI.GetPlayerDetectionInfo().position);
    }

    public override void Execute()
    {
        // Custom patrol behavior for tanks
        Debug.Log("Tank is chasing");
        tankAI.ChaseEnemy();
        // Add movement or other tank-specific logic here
    }

    public override void Exit()
    {
        tankAI.StopMoving();
    }
}


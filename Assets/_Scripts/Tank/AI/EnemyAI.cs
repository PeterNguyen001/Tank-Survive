using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAI : TankSubComponent
{

    protected AISensor sensor;
    protected AIStateMachine stateMachine;



    void Start()
    {
        //stateMachine = new AIStateMachine();
        //stateMachine.ChangeState(new IdleState(this));
    }

    void Update()
    {
        //stateMachine.Update();

        //// Transition logic example
        //if (Vector3.Distance(transform.position, player.position) < detectionRange)
        //{
        //    stateMachine.ChangeState(new ChaseState(this));
        //}
        //else
        //{
        //    stateMachine.ChangeState(new PatrolState(this));
        //}
    }


}

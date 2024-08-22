using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private AIStateMachine stateMachine;
    public Transform player;
    public float detectionRange;
    public float attackRange;

    void Start()
    {
        stateMachine = new AIStateMachine();
        //stateMachine.ChangeState(new IdleState(this));
    }

    void Update()
    {
        stateMachine.Update();

        // Transition logic example
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            //stateMachine.ChangeState(new ChaseState(this));
        }
        else
        {
            //stateMachine.ChangeState(new PatrolState(this));
        }
    }
}

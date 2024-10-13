public abstract class State
{
    protected EnemyAI enemyAI;

    public State(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}

public class TankState : State
{
    protected EnemyTankAI tankAI;

    public TankState(EnemyAI enemyAI) : base(enemyAI)
    {
        // Downcast to EnemyTankAI to access tank-specific methods
        tankAI = enemyAI as EnemyTankAI;
    }
    public override void Enter() { }
    public override void Execute() { }
    public override void Exit() { }
}

public class AIStateMachine
{
    private State currentState;

    public void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;

            if (currentState != null)
            {
                currentState.Enter();
            }
        }
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
}

public class IdleState : State
{
    public IdleState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter() { }
    public override void Execute() { }
    public override void Exit() { }
}

public class PatrolState : State
{
    public PatrolState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter() { }
    public override void Execute() { }
    public override void Exit() { }
}

public class ChaseState : State
{
    public ChaseState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void Enter() { }
    public override void Execute() { }
    public override void Exit() { }
}


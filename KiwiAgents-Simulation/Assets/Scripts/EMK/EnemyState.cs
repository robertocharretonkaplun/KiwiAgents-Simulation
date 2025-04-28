public abstract class EnemyState
{
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyStateMachine enemyStateMachine)
    {
        this.enemyStateMachine = enemyStateMachine;
    }

    public abstract void Execute();
}

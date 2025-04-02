public class IdleState : EnemyState
{
    public IdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) {}

    public override void Execute()
    {
        
        if (enemyStateMachine.IsPlayerInSight())
        {
            enemyStateMachine.ChangeState(enemyStateMachine.detectingState);
        }
    }
}

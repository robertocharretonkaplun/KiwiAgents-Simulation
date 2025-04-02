public class DetectingState : EnemyState
{
    public DetectingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) {}

    public override void Execute()
    {
        
        if (enemyStateMachine.IsPlayerInSight())
        {
            enemyStateMachine.ChangeState(enemyStateMachine.revealedState);
        }
        else
        {
            enemyStateMachine.ChangeState(enemyStateMachine.idleState);
        }
    }
}

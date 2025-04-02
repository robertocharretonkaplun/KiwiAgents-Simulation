public class RevealedState : EnemyState
{
    public RevealedState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) {}

    public override void Execute()
    {
        
        enemyStateMachine.RevealEnemy();
    }
}

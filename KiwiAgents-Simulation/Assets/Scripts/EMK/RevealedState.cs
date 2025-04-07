public class RevealedState : EnemyState
{
    private bool yaRevelado = false;

    public RevealedState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Execute()
    {
        if (!yaRevelado)
        {
            //Debug.Log("🔔 Entrando a RevealedState");
            enemyStateMachine.RevealEnemy();
            yaRevelado = true;
        }
    }
}

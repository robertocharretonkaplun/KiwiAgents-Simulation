using UnityEngine;
public class IdleState : EnemyState
{
    public IdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Execute()
    {
        //Debug.Log("IdleState activo");

        if (enemyStateMachine.IsPlayerInSight() ||
            (enemyStateMachine.detectionZone != null && enemyStateMachine.detectionZone.hasDetectPlayer))
        {
           // Debug.Log("Cambiando a DetectingState");
            enemyStateMachine.ChangeState(enemyStateMachine.detectingState);
        }

    }

}

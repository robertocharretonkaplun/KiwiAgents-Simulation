using UnityEngine;
public class DetectingState : EnemyState
{
    private bool señalActivada = false;

    public DetectingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Execute()
    {
        Debug.Log("DetectingState ejecutado");

        if (enemyStateMachine.IsPlayerInSight() ||
            (enemyStateMachine.detectionZone != null && enemyStateMachine.detectionZone.hasDetectPlayer))
        {
            if (!enemyStateMachine.activarTemporizador)
            {
                enemyStateMachine.activarTemporizador = true;
                Debug.Log("⏱️ Temporizador activado desde estado Detecting");
            }

            if (!señalActivada)
            {
                enemyStateMachine.ShowExclamation();
                señalActivada = true;
            }
        }
        else
        {
            // Solo volver a Idle si aún no fue revelado
            if (!enemyStateMachine.IsRevealed())
            {
                enemyStateMachine.activarTemporizador = false;
                enemyStateMachine.HideExclamation();
                señalActivada = false;
                enemyStateMachine.ChangeState(enemyStateMachine.idleState);
            }
        }
    }
}

using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public bool hasDetectPlayer = false;
    public GameObject detectPlayerRef;
    public EnemyStateMachine enemyFSM;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasDetectPlayer && other.CompareTag("Player"))
        {
            hasDetectPlayer = true;
            detectPlayerRef = other.gameObject;
            Debug.Log("🚪 Jugador entró en la zona de detección");

            if (enemyFSM != null && !enemyFSM.IsRevealed())
            {
                enemyFSM.activarTemporizador = true;
                enemyFSM.ShowExclamation();
                Debug.Log("⏱️ Temporizador activado por colisión");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && detectPlayerRef == other.gameObject)
        {
            if (enemyFSM != null && enemyFSM.IsRevealed())
            {
                Debug.Log("⚠️ Jugador salió pero enemigo ya revelado, no ocultamos señal");
                return;
            }

            hasDetectPlayer = false;
            detectPlayerRef = null;
            Debug.Log("🧹 Limpiando referencia de jugador (salida)");
            enemyFSM.HideExclamation(); // ❌ elimine esta línea si la tiene aquí
        }
    }

}

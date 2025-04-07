using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState currentState;
    public EnemyState idleState;
    public EnemyState detectingState;
    public EnemyState revealedState;

    public GameObject modeloEnemigo;
    public GameObject exclamacionUI;
    public Transform player;

    [Header("Control de Escala y Visibilidad")]
    public Vector3 escalaOriginal = new Vector3(40.64f, 40.64f, 40.64f); // ✅

    [Header("Temporizador de Revelación")]
    public float tiempoAntesDeRevelar = 10f;
    public bool activarTemporizador = false;

    [Header("Detección")]
    public DetectionZone detectionZone;
    public float rangoVision = 10f;
    public float anguloVision = 60f;

    private float tiempoDetectando = 0f;
    private bool revelado = false;

    void Start()
    {
        idleState = new IdleState(this);
        detectingState = new DetectingState(this);
        revealedState = new RevealedState(this);

        currentState = idleState;

        if (modeloEnemigo != null)
            modeloEnemigo.transform.localScale = Vector3.zero; // Inicia invisible

        if (detectionZone != null)
            detectionZone.enemyFSM = this;

        HideExclamation();
    }

    void Update()
    {
        if (currentState != null)
            currentState.Execute();

        if (activarTemporizador && !revelado)
        {
            tiempoDetectando += Time.deltaTime;
            Debug.Log($"⏱️ Esperando para revelar: {tiempoDetectando}");

            if (tiempoDetectando >= tiempoAntesDeRevelar)
            {
                Debug.Log("💥 Tiempo cumplido: revelando");
                RevealEnemy();
                ChangeState(revealedState);
                activarTemporizador = false;
                tiempoDetectando = 0f;
            }
        }
    }

    public void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    public void RevealEnemy()
    {
        revelado = true;
        HideExclamation();

        if (modeloEnemigo != null)
        {
            modeloEnemigo.transform.localScale = escalaOriginal;
            Debug.Log($"📏 Escala restaurada a: {escalaOriginal}");
        }
    }

    public void ShowExclamation()
    {
        if (exclamacionUI != null) exclamacionUI.SetActive(true);
    }

    public void HideExclamation()
    {
        if (exclamacionUI != null) exclamacionUI.SetActive(false);
    }

    public bool IsPlayerInSight()
    {
        if (player == null) return false;

        Vector3 dirJugador = player.position - transform.position;
        float distancia = dirJugador.magnitude;

        if (distancia < rangoVision)
        {
            float angulo = Vector3.Angle(transform.forward, dirJugador);
            if (angulo < anguloVision * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, dirJugador.normalized, out hit, rangoVision))
                {
                    if (hit.collider.CompareTag("Player"))
                        return true;
                }
            }
        }
        return false;
    }

    public bool IsRevealed()
    {
        return revelado;
    }
}

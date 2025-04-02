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
    public float rangoVision = 10f;
    public float anguloVision = 60f;
    public float tiempoAntesDeRevelar = 1.5f;

    private bool revelado = false;

    void Start()
    {
        
        idleState = new IdleState(this);
        detectingState = new DetectingState(this);
        revealedState = new RevealedState(this);

       
        currentState = idleState;

        modeloEnemigo.SetActive(false);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    public void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    
    public void RevealEnemy()
    {
        revelado = true;
        modeloEnemigo.SetActive(true);
        exclamacionUI.SetActive(false);
    }

    public void ShowExclamation()
    {
        exclamacionUI.SetActive(true);
    }

    public void HideExclamation()
    {
        exclamacionUI.SetActive(false);
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
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}

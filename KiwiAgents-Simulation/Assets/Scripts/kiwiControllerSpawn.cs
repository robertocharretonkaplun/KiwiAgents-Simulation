using UnityEngine;
using UnityEngine.AI;

public class kiwiControllerSpawn : MonoBehaviour
{
    public Transform ObjectFollow; // Referencia al jugador
    private NavMeshAgent agent; // Referencia al NavMeshAgent
    [SerializeField] public float stopDistance = 1.0f; // Distancia mínima para detenerse
    private bool avoidPoop = false;

    public DetectionZone detectionZone;
    public EnemyStateMachine enemyStateMachine; // MODIFICADO: acceso a la FSM

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stopDistance;

        PoopController.OnPoopStatusChanged += HandlePoopStatus;
    }

    void Update()
    {
        // MODIFICADO: Solo asigna objetivo si el enemigo está revelado
        if (enemyStateMachine != null && enemyStateMachine.IsRevealed())
        {
            if (detectionZone.hasDetectPlayer)
            {
                ObjectFollow = detectionZone.detectPlayerRef.transform;
            }
            else
            {
                ObjectFollow = null;
            }
        }
        else
        {
            ObjectFollow = null; // Si no está revelado, no hace nada
        }

        if (avoidPoop)
        {
            MoveAwayFromPoop();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        if (ObjectFollow)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, ObjectFollow.position);

            if (distanceToPlayer > stopDistance)
            {
                agent.SetDestination(ObjectFollow.position);
            }
            else
            {
                agent.ResetPath();
            }
        }
    }

    void MoveAwayFromPoop()
    {
        if (ObjectFollow)
        {
            Vector3 direction = (transform.position - ObjectFollow.position).normalized;
            Vector3 newPos = transform.position + direction * 3f; // Distancia de alejamiento
            agent.SetDestination(newPos);
        }
    }

    void HandlePoopStatus(bool hasPoop)
    {
        avoidPoop = hasPoop;
    }

    void OnDestroy()
    {
        PoopController.OnPoopStatusChanged -= HandlePoopStatus;
    }
}


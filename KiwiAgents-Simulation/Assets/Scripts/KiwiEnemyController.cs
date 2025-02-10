using UnityEngine;
using UnityEngine.AI;

public 
class KiwiEnemyController : MonoBehaviour{

    public Transform ObjectFollow;  // Referencia al jugador
    private NavMeshAgent agent;     // Referencia al NavMeshAgent

    [SerializeField]
    public float stopDistance = 1.0f;  // Distancia mínima para detenerse

    void 
    Start() {
        agent = GetComponent<NavMeshAgent>();

        if (ObjectFollow == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                ObjectFollow = player.transform;
            }
        }

        agent.stoppingDistance = stopDistance;
    }

    void 
    Update() {
        if (ObjectFollow == null){
            Debug.LogWarning("Jugador no encontrado. Asegúrate de que el objeto tiene la etiqueta 'Player'.");
            return;
        }
        MoveTowardsPlayer();
    }

    void 
    MoveTowardsPlayer() {
        float distanceToPlayer = Vector3.Distance(transform.position, ObjectFollow.position);

        if (distanceToPlayer > stopDistance) {
            agent.SetDestination(ObjectFollow.position);
        }
        else {
            agent.ResetPath();
        }
    }
}

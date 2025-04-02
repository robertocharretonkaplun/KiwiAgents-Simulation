using UnityEngine;
using UnityEngine.AI;

public class KiwiEnemyController : MonoBehaviour
{
  public Transform ObjectFollow; // Referencia al jugador
  private NavMeshAgent agent; // Referencia al NavMeshAgent
  [SerializeField] public float stopDistance = 1.0f; // Distancia mínima para detenerse
  private bool avoidPoop = false;
  public DetectionZone detectionZone;
  void
  Start()
  {
    agent = GetComponent<NavMeshAgent>();

    agent.stoppingDistance = stopDistance;
    PoopController.OnPoopStatusChanged += HandlePoopStatus;
  }

  void
  Update()
  {
    if (detectionZone.hasDetectPlayer)
    {
      ObjectFollow = detectionZone.detectPlayerRef.transform;
    }
    else
    {
      ObjectFollow = null;
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
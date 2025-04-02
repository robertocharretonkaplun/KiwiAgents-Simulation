using UnityEngine;

public class WormPatrol : MonoBehaviour
{
    [Header("Configuración de patrulla")]
    public Transform waypointA; // Primer waypoint
    public Transform waypointB; // Segundo waypoint
    public float speed = 2f; // Velocidad de movimiento

    private Transform targetWaypoint; // Waypoint actual al que se dirige

    private void Start()
    {
        targetWaypoint = waypointA; // Comienza dirigiéndose al primer waypoint
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (targetWaypoint == null) return;

        // Mueve el worm hacia el waypoint actual
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Si llega al waypoint, cambia al otro
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            targetWaypoint = targetWaypoint == waypointA ? waypointB : waypointA;
        }
    }
}

using UnityEngine;

public class KiwiTropas : MonoBehaviour
{
    [Header("Configuración general")]
    public bool isAlly = false;            // Si es aliado, sigue al jugador
    public Transform player;               // Referencia al jugador

    [Header("Movimiento de seguimiento")]
    public float followSpeed = 5f;
    public float stoppingDistance = 2f;

    [Header("Movimiento aleatorio (enemigo)")]
    public float wanderRadius = 10f;
    public float wanderSpeed = 3f;
    public float waitTime = 3f;

    private Vector3 wanderTarget;
    private float wanderTimer;

    void Start()
    {
        wanderTarget = transform.position;
        wanderTimer = waitTime;
    }

    void Update()
    {
        if (isAlly)
        {
            FollowPlayer();
        }
        else
        {
            WanderRandomly();
        }
    }

    void FollowPlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * followSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    void WanderRandomly()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= waitTime || Vector3.Distance(transform.position, wanderTarget) < 1f)
        {
            // Elige una nueva posición aleatoria dentro del radio
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            wanderTarget = new Vector3(randomCircle.x, transform.position.y, randomCircle.y) + transform.position;
            wanderTimer = 0;
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;
        transform.position += direction * wanderSpeed * Time.deltaTime;
        transform.LookAt(new Vector3(wanderTarget.x, transform.position.y, wanderTarget.z));
    }
}
using UnityEngine;

public class KiwiTropas : MonoBehaviour
{
    [Header("Configuración general")]
    public bool isAlly = false;
    public Transform player;

    [Header("Follow Player")]
    public float followSpeed = 5f;
    public float stoppingDistance = 2f;

    [Header("Wander Random")]
    public float wanderRadius = 10f;
    public float wanderSpeed = 3f;
    public float waitTime = 3f;

    [Header("Interacción")]
    public float detectionRadius = 3f;  // Radio para detectar al jugador
    private bool isPlayerNear = false;
    public GameObject interactionUI;  //canvas
    private PlayerInventory playerInventory;
    [Header("Feedback")]
    public GameObject indicadorNeutral;
    public GameObject indicadorAliado;
    //  public AudioSource interactionSound;


    private Vector3 wanderTarget;
    private float wanderTimer;

    void Start()
    {
        indicadorAliado.SetActive(false);
        indicadorNeutral.SetActive(true);
        wanderTarget = transform.position;
        wanderTimer = waitTime;

        if (player != null)
            playerInventory = player.GetComponent<PlayerInventory>();
    }

    void Update()
    {
        if (isAlly)
        {
            FollowPlayer();
        }
        else
        {
            DetectPlayer();

            if (isPlayerNear)
            {
                // Espera la interacción
                if (Input.GetKeyDown(KeyCode.E))
                {
                    TryConvertToAlly();
                }
                // No se mueve si el jugador está cerca
                return;
            }

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
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            wanderTarget = new Vector3(randomCircle.x, transform.position.y, randomCircle.y) + transform.position;
            wanderTimer = 0;
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;
        transform.position += direction * wanderSpeed * Time.deltaTime;
        transform.LookAt(new Vector3(wanderTarget.x, transform.position.y, wanderTarget.z));
    }

    void DetectPlayer()
    {
        if (player == null)
        {
            isPlayerNear = false;
            if (interactionUI != null) interactionUI.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        isPlayerNear = distance <= detectionRadius;

        if (interactionUI != null)
        {
            interactionUI.SetActive(isPlayerNear && !isAlly);
        }
    }
    void TryConvertToAlly()
    {
        if (isAlly || playerInventory == null) return;

        if (playerInventory.UseSeed())
        {
            isAlly = true;
            if (interactionUI != null) interactionUI.SetActive(false);
         //   if (interactionSound != null) interactionSound.Play();
            Debug.Log($"{gameObject.name} se ha convertido en aliado.");
            indicadorNeutral.SetActive(false);
            indicadorAliado.SetActive(true);
        }
        else
        {
            Debug.Log("Necesitas una semilla para convertirlo en aliado.");
            // Aquí podrías mostrar un mensaje visual si quieres
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class TraditionalControllerIv : MonoBehaviour
{
    public static TraditionalControllerIv instance;

    [Header("Agente")]
    CustomAccion input;
    public NavMeshAgent agent;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEfect;
    [SerializeField] LayerMask clickableLayer;

    [Header("Animator By Bruwwu")]
    public Rigidbody kiwiRb;
    public Animator kiwiAnimator;
    float lookRotationSpeed = 8f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    private bool isJumping = false;

    // 🔹 Variables para detectar doble clic y cambiar velocidad
    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;
    private float normalSpeed;
    [SerializeField] private float sprintSpeed = 10f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            return;
        }
        instance = this;
        input = new CustomAccion();
        kiwiRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Agent was null, check for component.");
        }
        normalSpeed = agent.speed; // Guardamos la velocidad normal
        AssingInputs();
    }

    private void Update()
    {
        FaceTarget();
        UpdateBlendTree();

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }
    }

  void OnEnable()
{
    if (input == null) input = new CustomAccion(); // 🔹 Asegurar que `input` no es null
    input.Enable();
}

void OnDisable()
{
    if (input != null) input.Disable(); // 🔹 Evitar deshabilitar si ya es null
}
    void AssingInputs()
    {
        input.Main.Move.performed += ctx => ClicKToMove();
    }

    void ClicKToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayer))
        {
            if (agent != null && !isJumping)
            {
                float timeSinceLastClick = Time.time - lastClickTime;
                lastClickTime = Time.time;

                if (timeSinceLastClick <= doubleClickThreshold)
                {
                    agent.speed = sprintSpeed;
                    CancelInvoke(nameof(ResetSpeed));
                    Invoke(nameof(ResetSpeed), 1.5f);
                }

                agent.isStopped = false;
                agent.SetDestination(hit.point);
            }

            if (clickEfect != null)
            {
                ParticleSystem Effect = Instantiate(clickEfect, hit.point + new Vector3(0, 0.1f, 0), clickEfect.transform.rotation);
                Destroy(Effect.gameObject, Effect.main.duration);
            }
        }
    }

    void ResetSpeed()
    {
        agent.speed = normalSpeed;
    }

    void FaceTarget()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            Vector3 direccion = (agent.destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }

    void UpdateBlendTree()
    {
        if (kiwiAnimator != null)
        {
            kiwiAnimator.SetFloat("Velocity", agent.velocity.magnitude);
        }
    }

    void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            agent.enabled = false;
            kiwiRb.linearVelocity = new Vector3(kiwiRb.linearVelocity.x, jumpForce, kiwiRb.linearVelocity.z);

            if (kiwiAnimator != null)
            {
                // kiwiAnimator.SetTrigger("Jump");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            agent.enabled = true;
        }
    }
}

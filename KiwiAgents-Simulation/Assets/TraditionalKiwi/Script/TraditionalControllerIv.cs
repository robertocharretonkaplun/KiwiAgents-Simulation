
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
    [SerializeField] private float jumpForce = 5f; // Ajusta qué tan alto salta
    private bool isJumping = false;

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
        AssingInputs();
    }

    private void Update()
    {
        FaceTarget();
        UpdateBlendTree();

        // Salto con la tecla Space
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
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
            if (agent != null && !isJumping) // Solo permitir moverse si no está saltando
            {
                agent.isStopped = false; // Asegurar que el NavMeshAgent sigue funcionando
                agent.SetDestination(hit.point);
            }
            if (clickEfect != null)
            {
                ParticleSystem Effect = Instantiate(clickEfect, hit.point + new Vector3(0, 0.1f, 0), clickEfect.transform.rotation);
                Destroy(Effect.gameObject, Effect.main.duration);
            }
        }
    }

    void FaceTarget()
    {
        if (agent.velocity.magnitude > 0.1f) // Solo rotar si se está moviendo
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

    // ----------------- LÓGICA DE SALTO ----------------- //

    void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            agent.enabled = false; // Desactiva el NavMeshAgent SOLO mientras dura el salto
            kiwiRb.linearVelocity = new Vector3(kiwiRb.linearVelocity.x, jumpForce, kiwiRb.linearVelocity.z);

            // Aquí puedes agregar la animación de salto cuando la tengas
            if (kiwiAnimator != null)
            {
                // kiwiAnimator.SetTrigger("Jump");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Detectar si aterriza
        {
            isJumping = false;
            agent.enabled = true; // Reactivar el NavMeshAgent para caminar normal
        }
    }
}



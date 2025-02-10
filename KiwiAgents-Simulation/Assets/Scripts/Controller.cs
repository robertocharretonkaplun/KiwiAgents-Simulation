using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    public static Controller instance;

    [Header("Agente")]
    CustomAccion input;
    public NavMeshAgent agent;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEfect;
    [SerializeField] LayerMask clickableLayer;

    float lookRotationSpeed = 8f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            return;
        }

        instance = this;

        // Assign a custom action in our input action
        input = new CustomAccion();
    }

    private void Start()
    {
        // Assign the NavMeshAgent reference and check that it is not null
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Agent was null, check for component.");
        }

        // Assign user inputs
        AssingInputs();
    }

    private void Update()
    {
        FaceTarget();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    /// <summary>
    /// Configures the inputs necessary to control the agent's movement and
    /// assigns an event to the click input that calls the move agent method.
    /// </summary>
    void AssingInputs()
    {
        input.Main.Move.performed += ctx => ClicKToMove();
    }

    /// <summary>
    /// A raycast is created from the mouse position to move the agent.
    /// If a surface is detected in the selected part, the agent will move, 
    /// towards that point, generating particles where it was clicked.
    /// </summary>
    void ClicKToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayer))
        {
            agent.destination = hit.point;
            if (clickEfect != null)
            {
                ParticleSystem Effect = Instantiate(clickEfect, hit.point += new Vector3(0, 0.1f, 0), clickEfect.transform.rotation);
                Destroy(Effect.gameObject, Effect.main.duration);
            }
        }
    }

    /// <summary>
    /// Rotates the agent in the direction of the destination. 
    /// This is done by interpolation for smooth movement.
    /// </summary>
    void FaceTarget()
    {
        // Calculates the direction to the destination
        Vector3 direccion = (agent.destination - transform.position).normalized;

        // Calculates the rotation that the object must have to face the direction of rotation
        Quaternion lookRotation = Quaternion.LookRotation(direccion);

        // Smoothly interpolates the current rotation to the new rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }
}

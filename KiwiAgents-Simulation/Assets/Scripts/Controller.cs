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

        // Asignar una accion personalizada en nuestra input action
        input = new CustomAccion();
    }

    private void Start()
    {
        // Asignar la referencia del NavMeshAgent y comprobar que no sea nulo
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Agent was null, check for component.");
        }

        // Asignar inputs de usuario
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
    /// Configura los inputs necesarios para el control del movimiento del agente y
    /// asigna un evento al input de clic que llama al m�todo mueve al agente.
    /// </summary>
    void AssingInputs()
    {
        input.Main.Move.performed += ctx => ClicKToMove();
    }

    /// <summary>
    /// Se crea un raycast desde la posici�n del mpouse para mover el agente.
    /// Si se detecta una superficie en la parte seleccionada, el agente se mover� 
    /// hacia ese punto, generando unas part�culas en donde se dio click.
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
    /// Rota al agente hacia la direcci�n del destino. 
    /// Se hace mediante una interpolaci�n para que el movimiento sea suave.
    /// </summary>
    void FaceTarget()
    {
        // Calcula la direcci�n hacia el destino
        Vector3 direccion = (agent.destination - transform.position).normalized;

        // Calcula la rotaci�n que debe tener el objeto para mirar hacia la direcci�n
        Quaternion lookRotation = Quaternion.LookRotation(direccion);

        // Interpola suavemente la rotaci�n actual hacia la nueva rotaci�n
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }
}

/*using Unity.VisualScripting;
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
    [SerializeField] GameObject objectToInstantiate; // Objeto a instanciar
    [SerializeField] Transform spawnPoint; // Punto de spawn del objeto

    float lookRotationSpeed = 8f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            return;
        }

        instance = this;
        input = new CustomAccion();
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
        input.Main.SecondaryAction.performed += ctx => InstantiateObject(); // Asigna acción secundaria
    }

    void ClicKToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayer))
        {
            agent.destination = hit.point;
            if (clickEfect != null)
            {
                ParticleSystem Effect = Instantiate(clickEfect, hit.point + new Vector3(0, 0.1f, 0), clickEfect.transform.rotation);
                Destroy(Effect.gameObject, Effect.main.duration);
            }
        }
    }

    void InstantiateObject()
    {
        if (objectToInstantiate != null && spawnPoint != null)
        {
            Instantiate(objectToInstantiate, spawnPoint.position, spawnPoint.rotation);
        }
    }

    void FaceTarget()
    {
        Vector3 direccion = (agent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }
}
*/
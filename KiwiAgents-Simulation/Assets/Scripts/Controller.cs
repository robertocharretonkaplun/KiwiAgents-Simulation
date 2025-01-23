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
    }

    private void Start()
    {
        // Asignar la referencia del NavMeshAgent
        if (agent = GetComponent<NavMeshAgent>())
        {
            Debug.LogError("Agent was null, check for component.");
        }

        // Asignar una accion personalizada en nuestra input action
        input = new CustomAccion();

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
    /// asigna un evento al input de clic que llama al método mueve al agente.
    /// </summary>
    void AssingInputs()
    {
        input.Main.Move.performed += ctx => ClicKToMove();
    }

    /// <summary>
    /// Se crea un raycast desde la posición del mpouse para mover el agente.
    /// Si se detecta una superficie en la parte seleccionada, el agente se moverá 
    /// hacia ese punto, generando unas partículas en donde se dio click.
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
    /// Rota al agente hacia la dirección del destino. 
    /// Se hace mediante una interpolación para que el movimiento sea suave.
    /// </summary>
    void FaceTarget()
    {
        // Calcula la dirección hacia el destino
        Vector3 direccion = (agent.destination - transform.position).normalized;

        // Calcula la rotación que debe tener el objeto para mirar hacia la dirección
        Quaternion lookRotation = Quaternion.LookRotation(direccion);

        // Interpola suavemente la rotación actual hacia la nueva rotación
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }
}

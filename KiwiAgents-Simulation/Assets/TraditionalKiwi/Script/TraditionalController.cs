using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TraditionalController : MonoBehaviour
{
    public static TraditionalController instance;

    [Header("Agente")]
    CustomAccion input;
    public NavMeshAgent agent;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEfect;
    [SerializeField] LayerMask clickableLayer;

    [Header("Animator By Bruwwu")] 
    public Rigidbody kiwiRb;
    public Animator kiwiAnimator;
    public int kiwiSpeed;
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

        //Encontrar componentes necesarios para animaciones (Bruwu)
        kiwiAnimator = GetComponent<Animator>();
        kiwiRb = GetComponent<Rigidbody>();
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
            //Verificacion del animator
              if (kiwiAnimator != null){

                kiwiAnimator.SetTrigger("KiwiRun"); //Trigger para pasar a la animacion de correr
            }

            if(agent != null){ //He implementado una verificacion del NavMeshAgent (Bruwu)

                agent.destination = hit.point;
            }
            
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


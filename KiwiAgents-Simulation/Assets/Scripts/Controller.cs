using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    

    CustomAccion input;
    public NavMeshAgent agent;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEfect;
    [SerializeField] LayerMask clickableLayer;

    float lookRotationSpeed = 8f;

  


    public static Controller instance;

    private void Awake()
    {
        instance = this;
        agent = GetComponent<NavMeshAgent>();
        input= new CustomAccion();
        AssingInputs();

    }
    private void Update()
    {
        FaceTarget();
        
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
            agent.destination = hit.point;
            if (clickEfect != null)
            {
                ParticleSystem Effect = Instantiate(clickEfect, hit.point += new Vector3(0, 0.1f, 0), clickEfect.transform.rotation);
                Destroy(Effect.gameObject, Effect.main.duration);
            }
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

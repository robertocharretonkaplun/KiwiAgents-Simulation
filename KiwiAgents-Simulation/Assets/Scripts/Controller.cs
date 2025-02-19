using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller instance;

    [Header("Agente")]
    CustomAccion input;
    public Rigidbody rb;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayer;
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    float lookRotationSpeed = 8f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isGrounded;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        input = new CustomAccion();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        AssingInputs();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            FaceTarget();
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

    /// <summary>
    /// Configura los inputs necesarios para el movimiento del personaje.
    /// </summary>
    void AssingInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
        input.Main.Jump.performed += ctx => Jump();
    }

    /// <summary>
    /// Se usa un Raycast para detectar la posici�n donde se hizo clic y mover al personaje.
    /// </summary>
    void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayer))
        {
            targetPosition = hit.point;
            isMoving = true;
            Debug.Log("Destino actualizado a: " + targetPosition);
        }
        else
        {
            Debug.Log("No se detectó ningún objeto al hacer clic.");
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, clickableLayer);

        if (isMoving)
        {
            MoveToTarget();
        }
    }


    /// <summary>
    /// Mueve al personaje usando f�sica (MovePosition).
    /// </summary>
    void MoveToTarget()
    {
        Debug.Log("Moviendo a: " + targetPosition);
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }


    /// <summary>
    /// Aplica un impulso en el eje Y para simular un salto.
    /// </summary>
    void Jump()
    {
        if (isGrounded)
        {
            Debug.Log("Salto");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("No puede saltar, no está en el suelo.");
        }
    }

    /// <summary>
    /// Rota al personaje en direcci�n al objetivo.
    /// </summary>
    void FaceTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }
}

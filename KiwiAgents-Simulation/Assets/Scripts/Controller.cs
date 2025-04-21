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
    public float airControlFactor = 0.5f; // Control en el aire
    float lookRotationSpeed = 8f;

    [Header("Render Objects")]
    public Material[] targetMaterial;
    public Material fullScreenMaterial;
    public string propertyName = "_Alpha"; // Nombre de la propiedad del shader
    public float VHSShaderTime = 5.0f; // Tiempo de transición del shader VHS
    public float vignettePower;
    public float vignetteIntensity;


    private Vector3 targetPosition;
    private Vector3 moveDirection; // Nueva variable para almacenar dirección de movimiento
    public bool isMoving = false;
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Bloquea la rotación del Rigidbody
        AssingInputs();
        targetPosition = transform.position;

        // Inicializa los materiales con el valor de la propiedad
        foreach (var material in targetMaterial)
        {
            material.SetFloat("_Alpha", 0.0f); // Cambia el valor de la propiedad
        }

        // Inicializa la propiedad del material de pantalla completa
        fullScreenMaterial.SetFloat("_VignettePower", 0.0f);
        fullScreenMaterial.SetFloat("_VignetteIntensity", 0.0f);

    }

    private void Update()
    {
        // Asegura que el personaje siempre mire hacia donde se mueve
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
        input.Main.Scanner.performed += ctx => ChangeMaterialProperty();
    }

    /// <summary>
    /// Cambia la propiedad del material al hacer clic en el botón de escáner.
    /// </summary>
    void ChangeMaterialProperty()
    {
            Debug.Log("Cambiando propiedad del material");
           
            foreach (var material in targetMaterial)
            {
                material.SetFloat("_Alpha", 1.0f); // Cambia el valor de la propiedad
            }

            // Cambia la propiedad del material de pantalla completa
            fullScreenMaterial.SetFloat("_VignettePower", vignettePower);
            fullScreenMaterial.SetFloat("_VignetteIntensity", vignetteIntensity);


            Invoke("ResetMaterialProperty", VHSShaderTime); // Resetea la propiedad después de 1 segundo
    }

    

    /// <summary>
    /// Resetea la propiedad del material después de un tiempo.
    /// </summary>
    void ResetMaterialProperty()
    {
            Debug.Log("Reseteando propiedad del material");


            foreach (var material in targetMaterial)
            {
                material.SetFloat("_Alpha", 0.0f); // Cambia el valor de la propiedad
            }

            // Resetea la propiedad del material de pantalla completa
            fullScreenMaterial.SetFloat("_VignettePower", 0.0f);
            fullScreenMaterial.SetFloat("_VignetteIntensity", 0.0f);
    }


    /// <summary>
    /// Se usa un Raycast para detectar la posición donde se hizo clic y mover al personaje.
    /// </summary>
    void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayer))
        {
            targetPosition = hit.point;
            moveDirection = (targetPosition - transform.position).normalized; // Guarda la dirección
            isMoving = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, clickableLayer);

        if (isGrounded)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }

        if (isMoving)
        {
            MoveCharacter();
        }
    }

    /// <summary>
    /// Mueve al personaje en el suelo o en el aire.
    /// </summary>
    void MoveCharacter()
    {
        if (isGrounded)
        {
            // Movimiento normal en el suelo
            Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
        else
        {
            // Movimiento en el aire con menor control
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed * airControlFactor, rb.linearVelocity.y, moveDirection.z * moveSpeed * airControlFactor);
        }

        // Si ya llegó al objetivo, detiene el movimiento
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
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Rota al personaje en dirección al objetivo.
    /// </summary>
    void FaceTarget()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }
}

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
    
    [Tooltip("Fuerza del salto del Kiwi.")]
    public float jumpForce = 5f;

    [Tooltip("Velocidad normal de movimiento del Kiwi.")]
    public float moveSpeed = 5f;

    [Tooltip("Factor de control cuando el Kiwi está en el aire.")]
    public float airControlFactor = 0.5f;

    float lookRotationSpeed = 8f;

    [Header("Sprint Settings")]
    /// <summary>
/// Velocidad máxima que el kiwi alcanzará cuando se detecte un doble clic.
/// Por defecto es mayor que moveSpeed, simulando una "carrera".
/// </summary>
    [Tooltip("Velocidad máxima al hacer doble clic.")]
    public float sprintSpeed = 10f;
    /// <summary>
/// Tiempo en segundos que durará el sprint una vez activado.
/// Después de este tiempo, el kiwi regresará a su velocidad normal.
/// </summary>

    [Tooltip("Duración en segundos del sprint.")]
    public float sprintDuration = 1.5f;

    private float normalSpeed;
    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    [Header("SFX")]
    [SerializeField] private float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    [Header("Render Objects")]
    public Material[] targetMaterial;
    public string propertyName = "_Alpha";

    private Vector3 targetPosition;
    private Vector3 moveDirection;
    public bool isMoving = false;
    private bool isGrounded;
    private Vector3 lastPosition;

    [Tooltip("Distancia mínima para que se detecte movimiento y se activen sonidos.")]
    [SerializeField] private float movementDeltaLimit = 0.05f;

    /// <summary>
    /// Configura la instancia y los inputs.
    /// </summary>
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

    /// <summary>
    /// Inicializa variables, configura inputs y prepara los materiales.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        AssingInputs();
        targetPosition = transform.position;
        lastPosition = transform.position;
        normalSpeed = moveSpeed;

        foreach (var material in targetMaterial)
        {
            material.SetFloat("_Alpha", 0.0f);
        }
    }

    /// <summary>
    /// Llama cada frame para ajustar la dirección visual del personaje.
    /// </summary>
    private void Update()
    {
        if (isMoving)
        {
            FaceTarget();
        }
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    /// <summary>
    /// Cambia temporalmente el alpha de los materiales (modo scanner).
    /// </summary>
    void ChangeMaterialProperty()
    {
        foreach (var material in targetMaterial)
        {
            material.SetFloat("_Alpha", 1.0f);
        }

        Invoke("ResetMaterialProperty", 5.0f);
    }

    void ResetMaterialProperty()
    {
        foreach (var material in targetMaterial)
        {
            material.SetFloat("_Alpha", 0.0f);
        }
    }

    /// <summary>
    /// Asigna los inputs a sus respectivas acciones.
    /// </summary>
    void AssingInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
        input.Main.Jump.performed += ctx => Jump();
        input.Main.Scanner.performed += ctx => ChangeMaterialProperty();
    }

    /// <summary>
    /// Detecta el punto de clic y mueve al Kiwi. Si se detecta doble clic, se activa el sprint.
    /// </summary>
    void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayer))
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            lastClickTime = Time.time;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                moveSpeed = sprintSpeed;
                CancelInvoke(nameof(ResetSpeed));
                Invoke(nameof(ResetSpeed), sprintDuration);
            }

            targetPosition = hit.point;
            moveDirection = (targetPosition - transform.position).normalized;
            isMoving = true;

            if (clickEffect != null)
            {
                ParticleSystem effect = Instantiate(clickEffect, hit.point + Vector3.up * 0.1f, clickEffect.transform.rotation);
                Destroy(effect.gameObject, effect.main.duration);
            }
        }
    }

    /// <summary>
    /// Restaura la velocidad normal del Kiwi después del sprint.
    /// </summary>
    void ResetSpeed()
    {
        moveSpeed = normalSpeed;
    }

    /// <summary>
    /// Control principal del movimiento y del sonido de pasos.
    /// </summary>
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

            float movementDelta = (transform.position - lastPosition).magnitude;

            if (isGrounded && movementDelta > movementDeltaLimit)
            {
                footstepTimer += Time.fixedDeltaTime;
                if (footstepTimer >= footstepInterval)
                {
                    AudioManager.instance.PlayFootstep();
                    footstepTimer = 0f;
                }
            }
            else
            {
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        lastPosition = transform.position;

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// Lógica de movimiento con control aéreo si el Kiwi está en el aire.
    /// </summary>
    void MoveCharacter()
    {
        if (isGrounded)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
        else
        {
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed * airControlFactor, rb.linearVelocity.y, moveDirection.z * moveSpeed * airControlFactor);
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// Aplica impulso hacia arriba para que el Kiwi salte.
    /// </summary>
    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Gira al personaje en dirección al destino de movimiento.
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

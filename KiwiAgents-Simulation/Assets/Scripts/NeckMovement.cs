using UnityEngine;
using System.Collections;

/// <summary>
/// Movimiento procedural del cuello con transiciones suaves usando corrutinas.
/// Idle: balanceo lateral (con bounce tipo respiración por rotación en X).
/// Caminata: movimiento vertical e inclinación lateral.
/// Si el personaje está en Idle prolongado, el movimiento se relaja pero mantiene algo de movimiento.
/// </summary>
public class NeckMovement : MonoBehaviour
{
    public Transform neckBone;
    public Transform jawBone;
    public Transform playerTransform;

    [Header("Idle Settings")]
    public float idleRotationSpeed = 15f; // Velocidad de rotacion lateral del idle
    public float idleRotationAngle = 30f; // Velocidad de rotacion lateral del idle
    public float idleBounceHeight = 10f; //¨Altura del rebote para simular la respiracion
    public float idleBounceSpeed = 2.5f; // Velocidad del rebote que simula la respiracion

    [Header("Walk Settings")]
    public float walkBounceSpeed = 3f; // Velocidad del rebote al caminar
    public float walkBounceHeight = 6f; // Altura del rebote al caminar
    public float walkTiltSpeed = 1.5f; // Velocidad de inclinacion del cuello al caminar
    public float walkTiltAngle = 4f; //¨Angulo de inclinacion del cuello al caminar
    public float movementThreshold = 0.01f; // Límite para detectar el movimiento

    [Header("Relax Settings")]
    public float relaxT = 0; // Valor del estado de calma (de 0 a 1)
    public float idleRelaxDelay = 5f; // Tiempo antes de que idle comience a calmarse
    public float relaxedSpeedFactor = 0.2f; // Reduccion de velocidad al estar calmado
    public float relaxedAngleFactor = 0.3f; // Reduccion del angulo al estar calmado
    public float relaxedBounceFactor = 0.3f; // Reduccion del rebote al estar calmado

    private Coroutine currentRoutine;
    private bool isMoving;
    private Vector3 lastPosition;

    private float idleAngle = 0f;
    private float walkOffset = 0f;
    private float timeStill = 0f;
    private float bounceTimer = 0f;
    private bool movingRight = true;
    private bool isIdle = true;

    /// <summary>
    /// Inicializamos la ultima posicion como la posicón del jugador y se aplica una rotación inicial de rebote.
    /// </summary>
    void Start()
    {
        lastPosition = playerTransform.position;
        //currentRoutine = StartCoroutine(IdleRoutine());

        // Aplicar rotación inicial para que se vea el bounce desde el inicio
        float initBounceX = Mathf.Sin(0f) * idleBounceHeight;
        Quaternion initRotation = Quaternion.Euler(initBounceX, 0f, 0f);
        neckBone.localRotation = initRotation;
    }

    /// <summary>
    /// Como su nombre lo dice, este método se ejecuta luego de Update y lo usamos para 
    /// asegurar que todas las transformaciones anteriores se hayan aplicado. 
    /// Además, detecta si el jugador se movio y gestiona la transicion entre idle y caminata.
    /// </summary>
    void LateUpdate()
    {
        if (neckBone == null || jawBone == null || playerTransform == null) return;

        isMoving = (playerTransform.position - lastPosition).sqrMagnitude > movementThreshold;
        lastPosition = playerTransform.position;

        if (isMoving && isIdle)
        {
            isIdle = false;
            ResetIdleState();

            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
            }
            currentRoutine = StartCoroutine(WalkRoutine());
        }
        else if (!isMoving && !isIdle)
        {
            isIdle = true;
            timeStill = 0f;
            StopCoroutine(currentRoutine);
        }

        jawBone.localRotation = neckBone.localRotation;
    }

    /// <summary>
    /// Restablecemos los parámetros del movimiento en idle al cambiar de caminata a idle.
    /// </summary>
    void ResetIdleState()
    {
        idleAngle = 0f;
        movingRight = true;
        timeStill = 0f;
        bounceTimer = 0f;
    }

    /// <summary>
    /// A diferencia de Update, FixedUpdate se ejecuta en intervalos fijos, por lo que la
    /// actualización de sus elementos no depende de la tasa de actualización de cada dispositivo,
    /// evitando problemas donde funcione bien en algunas laptops y no en otras.
    /// Manejar el movimiento en idle con mayor precisión, controla el balanceo y rebote 
    /// cuando el kiwi anda inactivo, y se encarga de que aumente su relajación.
    /// </summary>
    void FixedUpdate()
    {
        if (isIdle)
        {
            timeStill += Time.deltaTime;
            bounceTimer += Time.deltaTime * idleBounceSpeed;

            relaxT = Mathf.Clamp01((timeStill - idleRelaxDelay) / 3f);
            float currentSpeed = Mathf.Lerp(idleRotationSpeed, idleRotationSpeed * relaxedSpeedFactor, relaxT);
            float currentAngle = Mathf.Lerp(idleRotationAngle, idleRotationAngle * relaxedAngleFactor, relaxT);
            float bounceFactor = Mathf.Lerp(1f, relaxedBounceFactor, relaxT);
            float bounceX = Mathf.Sin(bounceTimer) * idleBounceHeight * bounceFactor;

            idleAngle += movingRight ? currentSpeed * Time.deltaTime : -currentSpeed * Time.deltaTime;

            if (idleAngle > currentAngle) movingRight = false;
            else if (idleAngle < -currentAngle) movingRight = true;

            Quaternion targetRotation = Quaternion.Euler(bounceX, idleAngle, 0);
            neckBone.localRotation = Quaternion.Slerp(neckBone.localRotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    /// <summary>
    /// Esta corrutina maneja el movimiento procedural del cuello durante la caminata.
    /// Se genera un rebote y una inclinación lateral suave usando interpolación (lerp).
    /// </summary>
    IEnumerator WalkRoutine()
    {
        walkOffset = 0f;
        float currentBounce = 0f;
        float currentTilt = 0f;

        while (true)
        {
            walkOffset += Time.deltaTime;

            float targetBounce = Mathf.Sin(walkOffset * walkBounceSpeed) * walkBounceHeight;
            float targetTilt = Mathf.Cos(walkOffset * walkTiltSpeed) * walkTiltAngle;

            currentBounce = Mathf.Lerp(currentBounce, targetBounce, Time.deltaTime * 5f);
            currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * 5f);

            Quaternion targetRotation = Quaternion.Euler(currentBounce, 0, currentTilt);
            neckBone.localRotation = Quaternion.Slerp(neckBone.localRotation, targetRotation, Time.deltaTime * 5f);

            yield return null;
        }
    }
}
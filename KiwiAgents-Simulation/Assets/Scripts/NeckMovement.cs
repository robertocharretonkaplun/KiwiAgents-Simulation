using UnityEngine;

public class NeckMovement : MonoBehaviour
{
    public Transform neckBone;
    public Transform jawBone;
    public Transform playerTransform;

    public float idleRotationSpeed = 15f;
    public float idleRotationAngle = 30f;

    public float walkBounceSpeed = 2f;
    public float walkBounceHeight = 1.2f;
    public float walkTiltSpeed = 1.5f;
    public float walkTiltAngle = 4f;
    public float movementThreshold = 0.01f;

    private float idleAngle = 0f;
    private bool movingRight = true;
    private bool isMoving = false;
    private float walkOffset = 0f;
    private float currentBounce = 0f;
    private float currentTilt = 0f;

    void Start()
    {
        // Inicializa en Idle cuando la escena empieza
        idleAngle = 0f;
        movingRight = true;
        isMoving = false;
    }

    void LateUpdate()
    {
        if (neckBone == null || jawBone == null || playerTransform == null) return;

        // Detectar si el jugador se está moviendo
        isMoving = (playerTransform.position - neckBone.position).sqrMagnitude > movementThreshold;

        if (isMoving)
        {
            // 🔹 Resetear valores del Idle cuando empieza a caminar
            idleAngle = 0f;
            movingRight = true;
            UpdateNeckWalking();
        }
        else
        {
            // 🔹 Resetear Walk cuando se detiene (limpia los valores acumulados)
            walkOffset = 0f;
            currentBounce = 0f;
            currentTilt = 0f;
            UpdateNeckIdle();
        }

        // Sincroniza la mandíbula con el cuello
        jawBone.localRotation = neckBone.localRotation;
    }

    /// <summary>
    /// Movimiento del cuello en Idle (ahora ya no tiene rastros de Walk)
    /// </summary>
    void UpdateNeckIdle()
    {
        // Asegura que no empiece con Walk, incluso si se cambia de estado
        idleAngle += movingRight ? idleRotationSpeed * Time.deltaTime : -idleRotationSpeed * Time.deltaTime;

        if (idleAngle > idleRotationAngle) movingRight = false;
        else if (idleAngle < -idleRotationAngle) movingRight = true;

        // 🔹 Se asegura que no hay valores del Walk activos
        neckBone.localRotation = Quaternion.Euler(0, idleAngle, 0);
    }

    /// <summary>
    /// Movimiento del cuello en Caminata (sin afectar el Idle después)
    /// </summary>
    void UpdateNeckWalking()
    {
        walkOffset += Time.deltaTime * walkBounceSpeed;

        float targetBounce = Mathf.Sin(walkOffset) * walkBounceHeight;
        float targetTilt = Mathf.Cos(walkOffset) * walkTiltAngle;

        currentBounce = Mathf.Lerp(currentBounce, targetBounce, Time.deltaTime * 10f);
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * 10f);

        // 🔹 Ya no tiene valores acumulados de Idle
        neckBone.localRotation = Quaternion.Euler(currentBounce, 0, currentTilt);
    }
}

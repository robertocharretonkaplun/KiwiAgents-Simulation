using UnityEngine;

public class NeckMovement : MonoBehaviour
{
    public Transform neckBone;
    public Transform jawBone;
    public Transform playerTransform;
    private Vector3 lastPosition;

    // Parámetros de Idle
    public float idleRotationSpeed = 15f;
    public float idleRotationAngle = 30f;

    // Parámetros de Caminata (valores ajustados para reducir velocidad)
    public float walkBounceSpeed = 2f;  // 🔹 Reducido para evitar movimientos bruscos
    public float walkBounceHeight = 1f; // 🔹 Menos amplitud vertical
    public float walkTiltSpeed = 1f;    // 🔹 Movimiento lateral más controlado
    public float walkTiltAngle = 2f;    // 🔹 Menos inclinación lateral
    public float movementThreshold = 0.01f;

    private float idleAngle = 0f;
    private bool movingRight = true;
    private bool isMoving = false;
    private float walkOffset = 0f;
    private float currentVertical = 0f;
    private float currentTilt = 0f;

    void LateUpdate()
    {
        if (neckBone == null || jawBone == null || playerTransform == null) return;

        isMoving = (playerTransform.position - lastPosition).sqrMagnitude > movementThreshold;
        lastPosition = playerTransform.position;

        if (isMoving)
            UpdateNeckWalking();
        else
            UpdateNeckIdle();

        jawBone.localRotation = neckBone.localRotation;
    }

    /// <summary>
    /// Movimiento del cuello en Idle (solo balanceo lateral)
    /// </summary>
    void UpdateNeckIdle()
    {
        idleAngle = movingRight ? idleAngle + idleRotationSpeed * Time.deltaTime : idleAngle - idleRotationSpeed * Time.deltaTime;

        if (idleAngle > idleRotationAngle) movingRight = false;
        else if (idleAngle < -idleRotationAngle) movingRight = true;

        neckBone.localRotation = Quaternion.Euler(0, idleAngle, 0);
    }

    /// <summary>
    /// Movimiento del cuello en Caminata (sube y baja + leve inclinación lateral, pero más suave)
    /// </summary>
    void UpdateNeckWalking()
    {
        walkOffset += Time.deltaTime * walkBounceSpeed;

        float targetVertical = Mathf.Sin(walkOffset) * walkBounceHeight;
        float targetTilt = Mathf.Cos(walkOffset) * walkTiltAngle;

        // 🔹 Suavizamos la transición en la caminata
        currentVertical = Mathf.Lerp(currentVertical, targetVertical, Time.deltaTime * 3f);
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * 3f);

        neckBone.localRotation = Quaternion.Euler(currentVertical, currentTilt, 0);
    }
}

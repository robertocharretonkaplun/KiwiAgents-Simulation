using UnityEngine;

public class NeckMovement : MonoBehaviour
{
    public Transform neckBone;
    public Transform jawBone;
    public Transform playerTransform; // Ahora detectamos movimiento con Transform
    private Vector3 lastPosition; // Última posición para detectar movimiento

    // Parámetros de Idle
    public float idleRotationSpeed = 15f;
    public float idleRotationAngle = 30f;

    // Parámetros de Caminata
    public float walkBounceSpeed = 5f;
    public float walkBounceHeight = 2f;
    public float walkTiltSpeed = 3f;
    public float walkTiltAngle = 5f;
    public float movementThreshold = 0.01f; // Más sensible al movimiento

    private float idleAngle = 0f;
    private bool movingRight = true;
    private bool isMoving = false;
    private float walkOffset = 0f;

    void LateUpdate()
    {
        if (neckBone == null || jawBone == null || playerTransform == null) return;

        // ✅ Nueva detección de movimiento con Transform
        isMoving = (playerTransform.position - lastPosition).sqrMagnitude > movementThreshold;
        lastPosition = playerTransform.position; // Guardar última posición

        // 🔍 Debug para verificar si ahora detecta el movimiento correctamente
        Debug.Log("Kiwi en movimiento: " + isMoving);

        // Aplicar la animación según el estado
        if (isMoving)
            UpdateNeckWalking();
        else
            UpdateNeckIdle();

        // Sincronizar mandíbula con el cuello
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
    /// Movimiento del cuello en Caminata (sube y baja + leve inclinación lateral)
    /// </summary>
    void UpdateNeckWalking()
    {
        walkOffset += Time.deltaTime * walkBounceSpeed;

        float verticalMovement = Mathf.Sin(walkOffset) * walkBounceHeight;
        float sideTilt = Mathf.Cos(walkOffset * walkTiltSpeed) * walkTiltAngle;

        neckBone.localRotation = Quaternion.Euler(verticalMovement, sideTilt, 0);
    }
}

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
    public float idleRotationSpeed = 15f;
    public float idleRotationAngle = 30f;
    public float idleBounceHeight = 10f;
    public float idleBounceSpeed = 2.5f;

    [Header("Walk Settings")]
    public float walkBounceSpeed = 3f;
    public float walkBounceHeight = 6f;
    public float walkTiltSpeed = 1.5f;
    public float walkTiltAngle = 4f;
    public float movementThreshold = 0.01f;

    [Header("Relax Settings")]
    public float idleRelaxDelay = 5f;
    public float relaxedSpeedFactor = 0.2f;
    public float relaxedAngleFactor = 0.3f;
    public float relaxedBounceFactor = 0.3f;

    private Coroutine currentRoutine;
    private bool isMoving;
    private Vector3 lastPosition;

    private float idleAngle = 0f;
    private bool movingRight = true;
    private float walkOffset = 0f;
    private float timeStill = 0f;
    private float bounceTimer = 0f;

    void Start()
    {
        lastPosition = playerTransform.position;
        currentRoutine = StartCoroutine(IdleRoutine());

        // Aplicar rotación inicial para que se vea el bounce desde el inicio
        float initBounceX = Mathf.Sin(0f) * idleBounceHeight;
        Quaternion initRotation = Quaternion.Euler(initBounceX, 0f, 0f);
        neckBone.localRotation = initRotation;
    }

    void LateUpdate()
    {
        if (neckBone == null || jawBone == null || playerTransform == null) return;

        isMoving = (playerTransform.position - lastPosition).sqrMagnitude > movementThreshold;
        lastPosition = playerTransform.position;

        if (isMoving && currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            ResetIdleState();
            currentRoutine = StartCoroutine(WalkRoutine());
        }
        else if (!isMoving && currentRoutine != null && currentRoutine.ToString() != "IdleRoutine")
        {
            StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(IdleRoutine());
        }

        jawBone.localRotation = neckBone.localRotation;
    }

    void ResetIdleState()
    {
        idleAngle = 0f;
        movingRight = true;
        timeStill = 0f;
        bounceTimer = 0f;
    }

    IEnumerator IdleRoutine()
    {
        bounceTimer = 0f;

        while (true)
        {
            timeStill += Time.deltaTime;
            bounceTimer += Time.deltaTime * idleBounceSpeed;

            float relaxT = Mathf.Clamp01((timeStill - idleRelaxDelay) / 3f);
            float currentSpeed = Mathf.Lerp(idleRotationSpeed, idleRotationSpeed * relaxedSpeedFactor, relaxT);
            float currentAngle = Mathf.Lerp(idleRotationAngle, idleRotationAngle * relaxedAngleFactor, relaxT);
            float bounceFactor = Mathf.Lerp(1f, relaxedBounceFactor, relaxT);
            float bounceX = Mathf.Sin(bounceTimer) * idleBounceHeight * bounceFactor;

            idleAngle += movingRight ? currentSpeed * Time.deltaTime : -currentSpeed * Time.deltaTime;

            if (idleAngle > currentAngle) movingRight = false;
            else if (idleAngle < -currentAngle) movingRight = true;

            Quaternion targetRotation = Quaternion.Euler(bounceX, idleAngle, 0);
            neckBone.localRotation = Quaternion.Slerp(neckBone.localRotation, targetRotation, Time.deltaTime * 5f);

            yield return null;
        }
    }

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
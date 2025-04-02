using UnityEngine;

/// <summary>
/// Sistema de Inverse Kinematics (IK) para el movimiento de los pies en un personaje.
/// Controla la posición de los pies al caminar en terrenos irregulares.
/// </summary>
public class IKSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer; // Capa del terreno para detección de colisiones
    [SerializeField] private Transform body; // Referencia al hueso base del cuerpo
    [SerializeField] private IKSolver otherFoot; // Referencia al otro pie para sincronización
    [SerializeField] private float speed = 5f; // Velocidad del movimiento del pie
    [SerializeField] private float stepDistance = 3.5f; // Distancia máxima antes de realizar un paso
    [SerializeField] private float stepLength = 3.5f; // Longitud del paso
    [SerializeField] private float stepHeight = 1f; // Altura máxima de cada paso
    [SerializeField] private Vector3 footOffset = Vector3.zero; // Ajuste de la posición del pie
    [SerializeField] private float raycastDistance = 5f; // Distancia del raycast para detección del terreno
    [SerializeField] private float minFootSpacing = 0.5f; // Distancia mínima entre los pies

    private float footSpacing; // Separación entre los pies
    private float lerp; // Valor de interpolación para el movimiento del pie

    private Vector3 oldPosition, currentPosition, newPosition; // Posiciones del pie
    private Vector3 oldNormal, currentNormal, newNormal; // Normales para la rotación del pie
    private Vector3 lastBodyPosition; // Calcular la dirección de movimiento

    /// <summary>
    /// Inicialización de variables y configuración inicial del pie.
    /// </summary>
    private void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1f; // El pie está en reposo al inicio

        lastBodyPosition = body.position; // Guarda la posición inicial
    }

    /// <summary>
    /// Actualiza la posición del pie en cada frame.
    /// </summary>
    private void Update()
    {
        // Actualiza la posición y orientación del pie
        transform.position = currentPosition;
        transform.up = currentNormal;

        // Calcula el movimiento del cuerpo desde el último frame
        Vector3 bodyMovement = body.position - lastBodyPosition;
        lastBodyPosition = body.position;

        // ✅ Nueva validación: solo considerar pasos si el personaje se está moviendo de verdad
        if (Controller.instance != null && !Controller.instance.isMoving)
        {
            return;
        }

        // Lanzar un raycast desde la posición del cuerpo hacia abajo para detectar el terreno
        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, raycastDistance, terrainLayer.value))
        {
            float footDistance = Vector3.Distance(currentPosition, info.point);

            // Si el pie está demasiado lejos o el cuerpo se movió lo suficiente, y el otro pie está quieto, iniciar un paso
            if ((footDistance > stepDistance || bodyMovement.sqrMagnitude > 0.001f) &&
                !otherFoot.IsMoving() && lerp >= 1f)
            {
                lerp = 0f;

                float dynamicStepLength = stepLength + bodyMovement.magnitude * 1.5f;

                // Calcular nueva posición del pie
                Vector3 tentativeNewPosition = info.point + footOffset +
                    (body.forward * dynamicStepLength) + (body.right * footSpacing);

                // Verificar que los pies no estén demasiado separados
                float forwardDistance = Mathf.Abs(tentativeNewPosition.z - otherFoot.currentPosition.z);
                if (forwardDistance < minFootSpacing)
                {
                    float direction = Mathf.Sign(footSpacing);
                    float adjustment = (minFootSpacing - forwardDistance) * 0.3f;
                    adjustment = Mathf.Clamp(adjustment, 0, minFootSpacing * 100.5f);
                    tentativeNewPosition += body.forward * direction * adjustment;
                }

                newPosition = tentativeNewPosition;
                newNormal = info.normal;

                Debug.Log($"Nuevo paso en: {newPosition}");
            }
        }

        // Interpolación del movimiento del pie
        if (lerp < 1f)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = currentPosition = newPosition;
            oldNormal = currentNormal = newNormal;
        }
    }

    /// <summary>
    /// Dibuja Gizmos en la escena para visualizar la posición del pie y el raycast.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(currentPosition, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newPosition, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, newPosition);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(body.position + (body.right * footSpacing), Vector3.down * raycastDistance);
    }

    /// <summary>
    /// Verifica si el pie se está moviendo.
    /// </summary>
    /// <returns>True si el pie está en movimiento, False si está en reposo.</returns>
    public bool IsMoving()
    {
        return lerp < 1f;
    }
}

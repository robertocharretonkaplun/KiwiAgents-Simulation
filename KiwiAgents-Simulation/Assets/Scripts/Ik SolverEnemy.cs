using UnityEngine;

public class IkSolverEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer; // Capa del terreno para detección de colisiones
    [SerializeField] private Transform body; // Referencia al hueso base del cuerpo
    [SerializeField] private IkSolverEnemy otherFoot; // Referencia al otro pie para sincronización
    [SerializeField] private float speed = 5f; // Velocidad del movimiento del pie
    [SerializeField] private float stepDistance = 3.5f; // Distancia máxima antes de realizar un paso
    [SerializeField] private float stepLength = 3.5f; // Longitud del paso
    [SerializeField] private float stepHeight = 1f; // Altura máxima de cada paso
    [SerializeField] private Vector3 footOffset = Vector3.zero; // Ajuste de la posición del pie
    [SerializeField] private float raycastDistance = 5f; // Distancia del raycast para detección del terreno
    [SerializeField] private float minFootSpacing = 0.5f; // Distancia mínima entre los pies

    private float footSpacing; // Separación entre los pies
    private float lerp; // Valor de interpolación para el movimiento del pie

    private Vector3 oldPos, currentPos, newPos; // Posiciones del pie
    private Vector3 oldNormal, currentNormal, newNormal; // Normales para la rotación del pie
    private Vector3 lastBodyPosition; // Calcular la dirección de movimiento

    /// <summary>
    /// Inicialización de variables y configuración inicial del pie.
    /// </summary>
    private void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPos = newPos = oldPos = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1f; // El pie está en reposo al inicio

        lastBodyPosition = body.position; // Guarda la posición inicial
    }

    /// <summary>
    /// Actualiza la posición del pie en cada frame.
    /// </summary>
    private void Update()
    {
        Vector3 bodyMovement = body.position - lastBodyPosition;
        lastBodyPosition = body.position;

        transform.position = currentPos;
        transform.up = currentNormal;

        // Lanzar un raycast desde la posición del cuerpo hacia abajo para detectar el terreno
        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, raycastDistance, terrainLayer.value))
        {
            float footDistance = Vector3.Distance(currentPos, info.point);

            // Si el pie está demasiado lejos y el otro pie no se está moviendo, iniciar un paso
            if ((footDistance > stepDistance || bodyMovement.magnitude > 0.01f) &&
                 (otherFoot == null || !otherFoot.IsMoving()) && lerp >= 1f)
            {
                lerp = 0f;

                float dynamicStepLength = stepLength + bodyMovement.magnitude * 1.5f;

                // Calcular nueva posición del pie
                Vector3 tentativeNewPosition = info.point + footOffset +
                        (body.forward * dynamicStepLength) + (body.right * footSpacing);

                // Verificar que los pies no estén demasiado separados
                if (otherFoot != null)
                {
                    float forwardDistance = Mathf.Abs(tentativeNewPosition.z - otherFoot.currentPos.z);
                    if (forwardDistance < minFootSpacing)
                    {
                        float direction = Mathf.Sign(footSpacing);
                        float adjustment = (minFootSpacing - forwardDistance) * 0.3f;
                        adjustment = Mathf.Clamp(adjustment, 0, minFootSpacing * 100.5f);
                        tentativeNewPosition += body.forward * direction * adjustment;
                    }
                }

                newPos = tentativeNewPosition;
                newNormal = info.normal;
                //Debug.Log($"Nuevo paso en: {newPos}");
            }
        }

        // Interpolación del movimiento del pie
        if (lerp < 1f)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPos, newPos, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPos = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPos = currentPos = newPos;
            oldNormal = currentNormal = newNormal;
        }
    }

    /// <summary>
    /// Dibuja Gizmos en la escena para visualizar la posición del pie y el raycast.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(currentPos, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newPos, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, newPos);

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

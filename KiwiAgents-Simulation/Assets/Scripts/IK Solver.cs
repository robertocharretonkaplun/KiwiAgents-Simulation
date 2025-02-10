using UnityEngine;

public class IKSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default; // Capa del terreno
    [SerializeField] Transform body = default; // Referencia al hueso base del cuerpo
    [SerializeField] IKSolver otherFoot = default; // Referencia al otro pie
    [SerializeField] float speed = 1; // Velocidad del movimiento del pie
    [SerializeField] float stepDistance = 4; // Distancia máxima antes de realizar un paso
    [SerializeField] float stepLength = 4; // Longitud del paso de adelante a atrás
    [SerializeField] float stepHeight = 1; // Altura máxima de cada paso
    [SerializeField] Vector3 footOffset = default;

    float footSpacing; // Qué tan separados están ambos pies entre sí
    float lerp; // Valor de interpolación

    Vector3 oldPosition, currentPosition, newPosition; // Posiciones del pie
    Vector3 oldNormal, currentNormal, newNormal; // Valores para la rotación del pie

    /// <summary>
    /// Se inicializan las variables de posición y rotación del pie
    /// </summary>
    private void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1; // Se le indica que el pie no está moviensose al inicio
    }

    /// <summary>
    /// Controla el movimiento del pie y su ajuste al terreno en cada frame,
    /// utilizando un raycast para encontrar el terreno al que el pie debe ir.
    /// </summary>
    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        // Raycast desde el cuerpo hacia abajo para detectar la superficie del terreno
        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            // Comprobar si el pie debe moverse
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = info.point + (body.forward * stepLength * direction) + footOffset;
                newNormal = info.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    /// <summary>
    /// Se dibuja una esfera roja en la posición objetivo del pie como si fuera un raycast.
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.2f);
    }

    /// <summary>
    /// Comprobar si el pie está actualmente en movimiento.
    /// </summary>
    public bool IsMoving()
    {
        return lerp < 1;
    }



}

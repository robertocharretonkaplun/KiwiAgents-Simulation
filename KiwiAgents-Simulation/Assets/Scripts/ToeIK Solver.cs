using UnityEngine;

public class ToeIKSolver : MonoBehaviour
{
    [SerializeField] private Transform footTarget; // Target de IK del pie principal
    [SerializeField] private Transform[] toeTargets; // Targets de IK de los dedos
    [SerializeField] private LayerMask terrainLayer; // Capa del terreno para detección con Raycast
    [SerializeField] private float raycastDistance = 2f; // Distancia máxima del raycast
    [SerializeField] private float movementSmoothness = 5f; // Suavidad del movimiento de los dedos

    private Vector3[] initialOffsets;

    private void Start()
    {
        // Almacenar las posiciones iniciales de los targets de los dedos respecto al target del pie
        initialOffsets = new Vector3[toeTargets.Length];
        for (int i = 0; i < toeTargets.Length; i++)
        {
            initialOffsets[i] = toeTargets[i].position - footTarget.position;
        }
    }

    private void Update()
    {
        AdjustToePositions();
    }

    /// <summary>
    /// Ajusta la posición de los targets de los dedos para que sigan el movimiento del pie de forma natural y se adapten al terreno.
    /// </summary>
    private void AdjustToePositions()
    {
        for (int i = 0; i < toeTargets.Length; i++)
        {
            Vector3 targetPosition = footTarget.position + initialOffsets[i];
            if (Physics.Raycast(targetPosition + Vector3.up * raycastDistance, Vector3.down, out RaycastHit hit, raycastDistance * 2, terrainLayer))
            {
                targetPosition = hit.point; // Ajustar la posición del dedo al suelo
            }
            toeTargets[i].position = Vector3.Lerp(toeTargets[i].position, targetPosition, Time.deltaTime * movementSmoothness);
        }
    }
}

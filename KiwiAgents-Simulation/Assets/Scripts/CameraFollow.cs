using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
 [Header("Target Settings")]
    public string targetName = "MainCharacter"; //  Nombre del personaje a seguir
    private Transform target; // Transform del personaje

    [Header("Camera Settings")]
    public Vector3 cameraOffset = new Vector3(0, 15, -10); // Posición relativa al personaje
    public float followSpeed = 10f; // ?? Velocidad de seguimiento

    private void Start()
    {
        // ?? Buscar solo UNA VEZ al personaje en la jerarquía
        GameObject targetObject = GameObject.Find(targetName);

        if (targetObject != null)
        {
            target = targetObject.transform;
            transform.position = target.position + cameraOffset; //  Poner la cámara en su posición inicial
        }
        else
        {
            Debug.LogError($"? No se encontró un GameObject llamado '{targetName}'.");
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        //  SEGUIR AL PERSONAJE SIN TOCAR SU MOVIMIENTO
        Vector3 desiredPosition = target.position + cameraOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class FloatingUI : MonoBehaviour
{
    public Camera mainCamera;
    public Transform playerTransform;
    public float maxDistance = 10f;
    public float minScale = 0.5f;
    public float maxScale = 1f;
    public Canvas canvasToShow;
    // Referencia al RectTransform de la imagen que deseas escalar.
    public RectTransform imageToScale;

    private Transform canvasTransform;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Asigna el canvas si no se ha definido en el Inspector.
        if (canvasToShow == null)
            canvasToShow = GetComponentInChildren<Canvas>();

        canvasTransform = canvasToShow.transform;

        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void LateUpdate()
    {
        if (mainCamera == null || playerTransform == null || canvasToShow == null || imageToScale == null)
            return;

        // Se orienta el canvas para que mire directamente hacia la posición de la cámara.
        canvasTransform.LookAt(mainCamera.transform.position, mainCamera.transform.up);

        // Calcular la distancia entre el jugador y el contenedor (por ejemplo, el cubo)
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= maxDistance)
        {
            canvasToShow.enabled = true;

            // Calcula la escala para la imagen: 
            // Cuando estés cerca (distance ~ 0), t es 0 ? se usa minScale.
            // Cuando te alejas (distance ~ maxDistance), t se acerca a 1 ? se usa maxScale.
            float t = distance / maxDistance;
            float scale = Mathf.Lerp(minScale, maxScale, t);

            // Aplica la escala solo a la imagen, sin modificar la escala global del canvas.
            imageToScale.localScale = Vector3.one * scale;
        }
        else
        {
            canvasToShow.enabled = false;
        }
    }
}

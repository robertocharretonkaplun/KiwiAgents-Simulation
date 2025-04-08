using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public Camera mainCamera;
    public Transform playerTransform;
    public float maxDistance = 10f;
    public float minScale = 0.5f;
    public float maxScale = 1f;
    public Canvas canvasToShow;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (canvasToShow == null)
            canvasToShow = GetComponent<Canvas>();

        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void LateUpdate()
    {
        if (mainCamera == null || playerTransform == null || canvasToShow == null) return;

        // Siempre mirar a la cámara
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);

        // Distancia entre el jugador y este objeto
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= maxDistance)
        {
            canvasToShow.enabled = true;

            // Escala dinámica opcional
            float t = 1f - (distance / maxDistance);
            float scale = Mathf.Lerp(minScale, maxScale, t);
            transform.localScale = Vector3.one * scale;
        }
        else
        {
            canvasToShow.enabled = false;
        }
    }
}

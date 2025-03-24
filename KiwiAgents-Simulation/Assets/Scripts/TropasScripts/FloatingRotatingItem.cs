using UnityEngine;

public class FloatingRotatingItem : MonoBehaviour
{
    [Header("Rotación")]
    public float rotationSpeed = 50f;

    [Header("Flotación")]
    public float floatAmplitude = 0.5f;  // Altura del movimiento hacia arriba y abajo
    public float floatFrequency = 1f;    // Velocidad del movimiento oscilatorio

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotar sobre su eje Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Movimiento flotante hacia arriba y abajo (eje Y)
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }
}
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    [Header("Movimiento de Cámara")]
    public float moveSpeed = 20f;
    public float borderThickness = 10f;
    public Vector2 xLimits = new Vector2(-50f, 50f);
    public Vector2 zLimits = new Vector2(-50f, 50f);

    [Header("Zoom")]
    public float scrollSpeed = 500f;
    public float minY = 10f;
    public float maxY = 60f;

    void Update()
    {
        Vector3 pos = transform.position;

        // Movimiento con bordes de pantalla
        if (Input.mousePosition.y >= Screen.height - borderThickness)
        {
            pos += Vector3.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= borderThickness)
        {
            pos += Vector3.back * moveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - borderThickness)
        {
            pos += Vector3.right * moveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= borderThickness)
        {
            pos += Vector3.left * moveSpeed * Time.deltaTime;
        }

        // Zoom con scroll del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * Time.deltaTime;

        // Limitar zoom (altura Y)
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Limitar movimiento en X y Z
        pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);

        transform.position = pos;
    }
}
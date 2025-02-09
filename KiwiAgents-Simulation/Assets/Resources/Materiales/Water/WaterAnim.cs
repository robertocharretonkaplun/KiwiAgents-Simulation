using UnityEngine;

public class WaterAnim : MonoBehaviour
{
    public Material waterMaterial; // Material del agua
    public float speedX = 0.00000000001f;    // Velocidad horizontal del desplazamiento
    public float speedY = 0.00000000001f;    // Velocidad vertical del desplazamiento

    void Update()
    {
        // Desplazar la textura principal (Base Map)
        float offsetX = Time.time * speedX;
        float offsetY = Time.time * speedY;

        // Aplicar el desplazamiento al Base Map
        waterMaterial.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}




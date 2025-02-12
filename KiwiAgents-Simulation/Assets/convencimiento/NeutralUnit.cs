using UnityEngine;

public class NeutralUnit : MonoBehaviour
{
    public int requiredItems = 5; // Número de objetos necesarios
    public string playerUnitLayer = "Clickable"; // Layer al que se cambiará
    public int currentItems = 0;
    public GameObject NeutralIndicator;

    private bool isConverted = false; // Evita múltiples conversiones

    void Update()
    {
        // Comprueba si el número de objetos es suficiente y la unidad aún no ha sido convertida
        if (currentItems >= requiredItems && !isConverted)
        {
            ConvertToAlly();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es un item válido
        if (other.CompareTag("Item"))
        {
            currentItems++;
            Destroy(other.gameObject); // Elimina el objeto entregado
        }
    }

    void ConvertToAlly()
    {
        isConverted = true;
        gameObject.layer = LayerMask.NameToLayer(playerUnitLayer);
        Debug.Log("Unidad convertida en aliada");

        if (NeutralIndicator != null)
        {
            NeutralIndicator.SetActive(false);
        }
    }
}

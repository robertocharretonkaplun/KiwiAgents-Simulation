using UnityEngine;

public class NeutralUnit : MonoBehaviour
{
    public int requiredItems = 5; // Número de objetos necesarios
    public string playerUnitLayer = "Clickable"; // Layer al que se cambiará
    public int currentItems = 0;
    public GameObject NeutralIndicator;

    private bool isConverted = false; // Evita múltiples conversiones
    private bool unidadAliadaCerca = false; // Indica si una unidad aliada está cerca

    void Update()
    {
        // Si la unidad ya se convirtió, no hace nada
        if (isConverted) return;

        // Comprueba si el número de objetos es suficiente y la unidad aún no ha sido convertida
        if (currentItems >= requiredItems)
        {
            ConvertToAlly();
        }

        // Si hay una unidad aliada cerca y se hace clic derecho, intenta entregar un objeto
        if (unidadAliadaCerca && Input.GetMouseButtonDown(1)) // 1 = clic derecho
        {
            TryDeliverItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si la unidad ya se convirtió, no hace nada
        if (isConverted) return;

        // Verifica si el objeto que entra es una unidad aliada (usando el tag "Player")
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = true;
            Debug.Log("Unidad aliada cerca. Haz clic derecho para entregar un objeto.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si la unidad ya se convirtió, no hace nada
        if (isConverted) return;

        // Verifica si el objeto que sale es una unidad aliada (usando el tag "Player")
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = false;
            Debug.Log("Unidad aliada se ha alejado.");
        }
    }

    private void TryDeliverItem()
    {
        // Si la unidad ya se convirtió, no hace nada
        if (isConverted) return;

        // Verifica si el jugador tiene objetos para entregar
        if (GameManager.Instance.semillasRecolectadas > 0)
        {
            GameManager.Instance.semillasRecolectadas--; // Reduce la cantidad de objetos en el GameManager
            currentItems++; // Aumenta la cantidad de objetos entregados
            Debug.Log("Objeto entregado. Objetos actuales: " + currentItems);
        }
        else
        {
            Debug.Log("No tienes objetos para entregar.");
        }
    }

    void ConvertToAlly()
    {
        isConverted = true; // Marca la unidad como convertida
        gameObject.layer = LayerMask.NameToLayer(playerUnitLayer); // Cambia la layer
        gameObject.tag = "Player"; // Cambia el tag a "Player"
        Debug.Log("Unidad convertida en aliada. Tag cambiado a 'Player'.");

        if (NeutralIndicator != null)
        {
            NeutralIndicator.SetActive(false); // Desactiva el indicador visual
        }
    }
}
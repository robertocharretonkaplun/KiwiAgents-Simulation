using UnityEngine;

public class NeutralUnit : MonoBehaviour
{
    public int requiredItems = 3; // Número de objetos necesarios
    public string playerUnitLayer = "Clickable"; // Layer al que se cambiará
    public int currentItems = 0;
    public GameObject NeutralIndicator;

    private bool isConverted = false; // Evita múltiples conversiones
    private bool unidadAliadaCerca = false; // Indica si una unidad aliada está cerca

    void Update()
    {
        // Comprueba si el número de objetos es suficiente y la unidad aún no ha sido convertida
        if (currentItems >= requiredItems && !isConverted)
        {
            ConvertToAlly();
        }

        // Si hay una unidad aliada cerca, no está convertida y se hace clic izquierdo, intenta entregar un objeto
        if (unidadAliadaCerca && !isConverted && Input.GetMouseButtonDown(0)) // 0 = clic izquierdo
        {
            TryDeliverItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es una unidad aliada
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = true;
            if (!isConverted)
            {
                Debug.Log("Unidad aliada cerca. Haz clic izquierdo para entregar un objeto.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto que sale es una unidad aliada
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = false;
            Debug.Log("Unidad aliada se ha alejado.");
        }
    }

    private void TryDeliverItem()
    {
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
        isConverted = true;
        gameObject.layer = LayerMask.NameToLayer(playerUnitLayer); // Cambia la layer
        gameObject.tag = "Player"; // Cambia el tag a "Player"
        Debug.Log("Unidad convertida en aliada.");

        if (NeutralIndicator != null)
        {
            NeutralIndicator.SetActive(false); // Desactiva el indicador visual
        }

        // Desactiva la capacidad de recibir más objetos
        unidadAliadaCerca = false; // Ya no necesita estar cerca de una unidad aliada
        Debug.Log("La unidad neutral ya no acepta más objetos.");
    }
}
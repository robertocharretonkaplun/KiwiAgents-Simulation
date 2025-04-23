using UnityEngine;
using UnityEngine.UI;

public class NeutralUnit : MonoBehaviour
{
    public int requiredItems = 3; // Número de objetos necesarios
    public string playerUnitLayer = "Clickable"; // Layer al que se cambiará
    public int currentItems = 0;
    public GameObject NeutralIndicator;

    // Referencias a los textos del Canvas
    public Text textoUnidadCerca;
    public Text textoUnidadAlejada;
    public Text textoObjetoEntregado;
    public Text textoNoObjetos;
    public Text textoUnidadConvertida;
    public Text textoNoAceptaMasObjetos;

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
                // Desactiva todos los textos antes de activar uno nuevo
                DesactivarTodosLosTextos();
                // Mostrar el texto en el Canvas
                textoUnidadCerca.gameObject.SetActive(true);
                Debug.Log("Unidad neutral cerca. Haz clic izquierdo para entregar un objeto.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto que sale es una unidad aliada
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = false;
            // Desactiva todos los textos antes de activar uno nuevo
            DesactivarTodosLosTextos();
            // Mostrar el texto de que el jugador se ha alejado
            textoUnidadAlejada.gameObject.SetActive(true);
            Debug.Log("Te has alejado de la unidad neutral.");
            // Ocultar el texto después de un tiempo
            Invoke("OcultarTextoUnidadAlejada", 2f); // 2 segundos
        }
    }

    private void TryDeliverItem()
    {
        // Verifica si el jugador tiene objetos para entregar
        if (GameManager.Instance.semillasRecolectadas > 0)
        {
            GameManager.Instance.semillasRecolectadas--; // Reduce la cantidad de objetos en el GameManager
            currentItems++; // Aumenta la cantidad de objetos entregados
            // Desactiva todos los textos antes de activar uno nuevo
            DesactivarTodosLosTextos();
            // Mostrar el texto de objeto entregado
            textoObjetoEntregado.text = "Objeto entregado. Objetos actuales: " + currentItems;
            textoObjetoEntregado.gameObject.SetActive(true);
            Debug.Log("Objeto entregado. Objetos actuales: " + currentItems);
            // Ocultar el texto después de un tiempo
            Invoke("OcultarTextoObjetoEntregado", 2f); // 2 segundos
        }
        else
        {
            // Desactiva todos los textos antes de activar uno nuevo
            DesactivarTodosLosTextos();
            // Mostrar el texto de no tienes objetos
            textoNoObjetos.gameObject.SetActive(true);
            Debug.Log("No tienes objetos para entregar.");
            // Ocultar el texto después de un tiempo
            Invoke("OcultarTextoNoObjetos", 2f); // 2 segundos
        }
    }

    void ConvertToAlly()
    {
        isConverted = true;
        gameObject.layer = LayerMask.NameToLayer(playerUnitLayer); // Cambia la layer
        gameObject.tag = "Player"; // Cambia el tag a "Player"
        // Desactiva todos los textos antes de activar uno nuevo
        DesactivarTodosLosTextos();
        // Mostrar el texto de unidad convertida
        textoUnidadConvertida.gameObject.SetActive(true);
        Debug.Log("Unidad convertida en aliada.");
        // Ocultar el texto después de un tiempo
        Invoke("OcultarTextoUnidadConvertida", 2f); // 2 segundos

        if (NeutralIndicator != null)
        {
            NeutralIndicator.SetActive(false); // Desactiva el indicador visual
        }

        // Desactiva la capacidad de recibir más objetos
        unidadAliadaCerca = false; // Ya no necesita estar cerca de una unidad aliada
        // Mostrar el texto de que no acepta más objetos
        textoNoAceptaMasObjetos.gameObject.SetActive(true);
        Debug.Log("La unidad neutral ya no acepta más objetos.");
        // Ocultar el texto después de un tiempo
        Invoke("OcultarTextoNoAceptaMasObjetos", 2f); // 2 segundos
    }

    // Métodos para ocultar los textos después de un tiempo
    private void OcultarTextoUnidadAlejada()
    {
        textoUnidadAlejada.gameObject.SetActive(false);
    }

    private void OcultarTextoObjetoEntregado()
    {
        textoObjetoEntregado.gameObject.SetActive(false);
    }

    private void OcultarTextoNoObjetos()
    {
        textoNoObjetos.gameObject.SetActive(false);
    }

    private void OcultarTextoUnidadConvertida()
    {
        textoUnidadConvertida.gameObject.SetActive(false);
    }

    private void OcultarTextoNoAceptaMasObjetos()
    {
        textoNoAceptaMasObjetos.gameObject.SetActive(false);
    }

    private void DesactivarTodosLosTextos()
    {
        // Desactiva todos los textos
        textoUnidadCerca.gameObject.SetActive(false);
        textoUnidadAlejada.gameObject.SetActive(false);
        textoObjetoEntregado.gameObject.SetActive(false);
        textoNoObjetos.gameObject.SetActive(false);
        textoUnidadConvertida.gameObject.SetActive(false);
        textoNoAceptaMasObjetos.gameObject.SetActive(false);
        // Cancela cualquier Invoke pendiente
        CancelInvoke("OcultarTextoUnidadAlejada");
        CancelInvoke("OcultarTextoObjetoEntregado");
        CancelInvoke("OcultarTextoNoObjetos");
        CancelInvoke("OcultarTextoUnidadConvertida");
        CancelInvoke("OcultarTextoNoAceptaMasObjetos");
    }
}
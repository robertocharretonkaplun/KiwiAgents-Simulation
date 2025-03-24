using UnityEngine;
using UnityEngine.UI;

public class Semilla : MonoBehaviour
{
    public float distanciaRecoleccion = 2f; // Rango de recolección
    private bool unidadAliadaCerca = false;

    // Referencias a los textos del Canvas
    public Text textoUnidadCerca;
    public Text textoUnidadAlejada;
    public Text textoSemillaRecolectada;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es una unidad aliada
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = true;
            // Desactiva todos los textos antes de activar uno nuevo
            DesactivarTodosLosTextos();
            // Mostrar el texto en el Canvas
            textoUnidadCerca.gameObject.SetActive(true);
            Debug.Log("Unidad aliada cerca. Haz clic izquierdo para recolectar la semilla.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto que sale del trigger es una unidad aliada
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = false;
            // Desactiva todos los textos antes de activar uno nuevo
            DesactivarTodosLosTextos();
            // Mostrar el texto de que la unidad se ha alejado
            textoUnidadAlejada.gameObject.SetActive(true);
            Debug.Log("Unidad aliada se ha alejado.");
            // Ocultar el texto después de un tiempo
            Invoke("OcultarTextoUnidadAlejada", 2f); // 2 segundos
        }
    }

    private void Update()
    {
        // Si hay una unidad aliada cerca y se hace clic izquierdo, recolecta la semilla
        if (unidadAliadaCerca && Input.GetMouseButtonDown(0)) // 0 = clic izquierdo
        {
            Recolectar();
        }
    }

    private void Recolectar()
    {
        GameManager.Instance.RecolectarSemilla();
        // Desactiva todos los textos antes de activar uno nuevo
        DesactivarTodosLosTextos();
        // Mostrar el texto de semilla recolectada
        textoSemillaRecolectada.gameObject.SetActive(true);
        Debug.Log("Semilla recolectada.");
        // Ocultar el texto después de un tiempo
        Invoke("OcultarTextoSemillaRecolectada", 0.6f); // 2 segundos
        Invoke("destruirSemilla", 0.6f);// Destruye la semilla después de recolectarla
    }


    private void destruirSemilla()
    {
        Destroy(gameObject);
    }
    private void OcultarTextoUnidadAlejada()
    {
        textoUnidadAlejada.gameObject.SetActive(false);
    }

    private void OcultarTextoSemillaRecolectada()
    {
        textoSemillaRecolectada.gameObject.SetActive(false);
    }

    private void DesactivarTodosLosTextos()
    {
        // Desactiva todos los textos
        textoUnidadCerca.gameObject.SetActive(false);
        textoUnidadAlejada.gameObject.SetActive(false);
        textoSemillaRecolectada.gameObject.SetActive(false);
        // Cancela cualquier Invoke pendiente
        CancelInvoke("OcultarTextoUnidadAlejada");
        CancelInvoke("OcultarTextoSemillaRecolectada");
    }
}
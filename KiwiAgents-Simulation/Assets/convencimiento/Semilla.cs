using UnityEngine;

public class Semilla : MonoBehaviour
{
   
    private bool unidadAliadaCerca = false;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es una unidad aliada
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = true;
            Debug.Log("Unidad aliada cerca. Haz clic derecho para recolectar la semilla.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica si el objeto que sale del trigger es una unidad aliada
        if (other.CompareTag("Player"))
        {
            unidadAliadaCerca = false;
            Debug.Log("Unidad aliada se ha alejado.");
        }
    }

    private void Update()
    {
        // Si hay una unidad aliada cerca y se hace clic derecho, recolecta la semilla
        if (unidadAliadaCerca && Input.GetMouseButtonDown(1)) // 1 = clic derecho
        {
            Recolectar();
        }
    }

    private void Recolectar()
    {
        GameManager.Instance.RecolectarSemilla();
        Debug.Log("Semilla recolectada.");
        Destroy(gameObject); // Destruye la semilla después de recolectarla
    }
}
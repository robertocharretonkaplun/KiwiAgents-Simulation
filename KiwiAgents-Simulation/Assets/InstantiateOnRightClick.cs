using UnityEngine;
using UnityEngine.InputSystem;

public class InstantiateOnRightClick : MonoBehaviour
{
    [SerializeField] private GameObject objectToInstantiate; // Objeto a instanciar
    [SerializeField] private Transform spawnPoint; // Punto de spawn del objeto

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame) // Detecta clic derecho
        {
            InstantiateObject();
        }
    }

    private void InstantiateObject()
    {
        if (objectToInstantiate != null && spawnPoint != null)
        {
            Instantiate(objectToInstantiate, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Falta asignar el objeto a instanciar o el spawnPoint.");
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InstantiateOnRightClick : MonoBehaviour
{
    [SerializeField] private GameObject objectToInstantiate; // Objeto a instanciar
    [SerializeField] private Transform spawnPoint; // Punto de spawn del objeto
    
    private void Start()
    {
        StartCoroutine(DropPoopRandomly()); // Iniciar la rutina de drop aleatorio
    }

    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame) // Detecta la tecla C
        {
            InstantiateObject();
        }
    }

    private IEnumerator DropPoopRandomly()
    {
        while (true)
        {
            float waitTime = Random.Range(10f, 30f); // Tiempo aleatorio entre 10 y 30 segundos
            yield return new WaitForSeconds(waitTime);
            InstantiateObject();
        }
    }

    private void InstantiateObject()
    {
        if (objectToInstantiate != null && spawnPoint != null)
        {
            Instantiate(objectToInstantiate, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Popó soltada en: " + spawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Falta asignar el objeto a instanciar o el spawnPoint.");
        }
    }
}

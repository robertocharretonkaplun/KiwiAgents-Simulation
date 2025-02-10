using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopController : MonoBehaviour
{
    public static PoopController instance;

    [Header("Agente Popo")]
    public GameObject Poop;
    public GameObject PoopPosition;
    CustomAccion input;

    [Header("Limitar Prefabs")]
    public int maxPoops = 3;
    private List<GameObject> activePoops = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            return;
        }

        instance = this;

        // Asignar una accion personalizada en nuestra input action
        input = new CustomAccion();
    }

    private void Start()
    {
        // Asignar inputs de usuario
        AssingInputs();
    }

    /// <summary>
    /// Configura los inputs necesarios para el Poop control del y
    /// asigna un evento al input de clic derecho que llama al metodo que droppea popo.
    /// </summary>

    void AssingInputs()
    {
        input.Main.Poop.performed += ctx => ClicKToPoop();
    }

    /// <summary>
    /// Crea un GameObject temporal que instancia el objeto popo en cuestion con la
    /// posicion de un objeto vacio. Luego de 5 segundos, se destruye
    /// </summary>

    void ClicKToPoop()
    {
        if(activePoops.Count < maxPoops)
        {
            GameObject PoopTemporal = Instantiate(Poop, PoopPosition.transform.position, PoopPosition.transform.rotation);
            activePoops.Add(PoopTemporal);
            Destroy(PoopTemporal, 5f);
            StartCoroutine(RemovePoopFromList(PoopTemporal, 5f));
        }
    }
    private IEnumerator RemovePoopFromList(GameObject poop, float delay)
    {
        yield return new WaitForSeconds(delay);
        activePoops.Remove(poop);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }
}

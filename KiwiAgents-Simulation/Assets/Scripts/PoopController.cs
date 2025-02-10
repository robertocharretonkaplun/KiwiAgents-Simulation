using UnityEngine;

public class PoopController : MonoBehaviour
{
    public static PoopController instance;

    [Header("Agente Popo")]
    public GameObject Poop;
    public GameObject PoopPosition;
    CustomAccion input;
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
        GameObject PoopTemporal = Instantiate(Poop, PoopPosition.transform.position, PoopPosition.transform.rotation) as GameObject;
        Destroy(PoopTemporal, 5f);
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

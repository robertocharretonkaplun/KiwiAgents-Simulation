using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int semillasRecolectadas = 0;
    public Text textoContadorSemillas; // Referencia al texto del Canvas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Recomendable para persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ActualizarTextoSemillas(); // Actualizar texto al iniciar
    }

    private void Update()
    {
        ActualizarTextoSemillas();
    }
    public void RecolectarSemilla()
    {
        semillasRecolectadas += 2;
        ActualizarTextoSemillas(); // Actualizar UI al recolectar
        Debug.Log("Semillas recolectadas: " + semillasRecolectadas);
    }

    // Método para actualizar el texto del contador
    private void ActualizarTextoSemillas()
    {
        if (textoContadorSemillas != null)
        {
            textoContadorSemillas.text = $"Semillas: {semillasRecolectadas}";
        }
    }
}
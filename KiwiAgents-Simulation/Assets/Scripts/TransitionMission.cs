using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class TransitionMission : MonoBehaviour
{
    public string nombreEscenaDestino; // Por ejemplo: "Escena2"
    public TransitionSettings configuracionTransicion;
    public float duracionTransicion = 1f;

    public void CambiarEscena()
    {
        TransitionManager.Instance().Transition(nombreEscenaDestino, configuracionTransicion, duracionTransicion);
    }

    // Pausa el juego
    public void PausarJuego()
    {
        Time.timeScale = 0f;  // Detiene el tiempo
        Debug.Log("Juego pausado");
        // Aquí puedes activar el menú de pausa si tienes uno
    }

    // Reanuda el juego
    public void ReanudarJuego()
    {
        Time.timeScale = 1f;  // Restaura el tiempo a la velocidad normal
        Debug.Log("Juego reanudado");
        // Aquí puedes desactivar el menú de pausa si tienes uno
    }
}

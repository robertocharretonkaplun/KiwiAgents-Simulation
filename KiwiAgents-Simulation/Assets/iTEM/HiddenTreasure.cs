using UnityEngine;

/// <summary>
/// Este script controla la lógica de excavar y revelar un tesoro oculto.
/// Permite al jugador activar el evento al presionar 'F' dentro de un rango,
/// y muestra partículas, sonido y el objeto del tesoro.
/// </summary>
public class HiddenTreasure : MonoBehaviour
{
    [Header("Configuración del Tesoro")]

    /// <summary>
    /// Objeto que representa el tesoro oculto.
    /// Este objeto se activa cuando la excavación termina.
    /// </summary>
    public GameObject treasureObject;

    [Header("Efectos Visuales y Sonoros")]

    /// <summary>
    /// Sistema de partículas que se activa al excavar.
    /// Simula tierra volando durante la excavación.
    /// </summary>
    public ParticleSystem digParticles;

    /// <summary>
    /// Sonido reproducido mientras se excava.
    /// </summary>
    public AudioSource digSound;

    [Header("Parámetros de Excavación")]

    /// <summary>
    /// Rango máximo desde el jugador para poder excavar.
    /// </summary>
    public float digRange = 3.5f;

    /// <summary>
    /// Tiempo que dura la excavación antes de revelar el tesoro.
    /// </summary>
    public float digDuration = 3f;

    [Header("Referencia al Jugador")]

    /// <summary>
    /// Transform del jugador. Se debe arrastrar manualmente desde la jerarquía.
    /// </summary>
    public Transform player;

    /// <summary>
    /// Booleano que indica si el tesoro ya fue excavado.
    /// </summary>
    private bool isDug = false;

    /// <summary>
    /// Booleano que indica si se está excavando actualmente.
    /// Previene excavaciones múltiples al mismo tiempo.
    /// </summary>
    private bool isDigging = false;

    /// <summary>
    /// Se ejecuta cada frame. Verifica si el jugador puede excavar,
    /// y si presiona la tecla correcta dentro del rango establecido.
    /// </summary>
    void Update()
    {
        // Si ya se ha excavado este punto, no hacer nada
        if (isDug)
        {
            Debug.Log("Ya se ha excavado aquí.");
            return;
        }

        // Si ya está en proceso de excavación, prevenir nuevas
        if (isDigging)
        {
            Debug.Log("Ya estás excavando...");
            return;
        }

        // Si el jugador presiona la tecla F
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Validar que el jugador esté asignado
            if (player == null)
            {
                Debug.LogWarning("No se asignó el jugador en el Inspector.");
                return;
            }

            // Calcular la distancia entre el jugador y el objeto
            float distance = Vector2.Distance(transform.position, player.position);

            // Si el jugador está dentro del rango permitido
            if (distance < digRange)
            {
                Debug.Log("Estás cerca. Iniciando excavación...");
                StartCoroutine(DigUpCoroutine()); // Iniciar excavación
            }
            else
            {
                Debug.Log("Estás demasiado lejos para excavar.");
            }
        }
    }

    /// <summary>
    /// Corrutina que simula el proceso de excavación:
    /// Reproduce efectos, espera un tiempo, y luego activa el tesoro.
    /// </summary>
    /// <returns>IEnumerator necesario para usar WaitForSeconds</returns>
    System.Collections.IEnumerator DigUpCoroutine()
    {
        // Bloquear nuevas excavaciones mientras esta está en curso
        isDigging = true;

        // Reproducir el sonido si está asignado
        if (digSound != null) digSound.Play();

        // Reproducir partículas si están asignadas
        if (digParticles != null)
        {
            digParticles.Stop(); // Asegura que reinicia
            digParticles.Play();
        }

        // Esperar el tiempo que toma excavar
        yield return new WaitForSeconds(digDuration);

        // Activar el objeto del tesoro
        if (treasureObject != null)
        {
            treasureObject.SetActive(true);
            Debug.Log("¡Tesoro desenterrado!");
        }

        // Marcar como excavado para evitar repetir
        isDug = true;
        isDigging = false;
    }

    /// <summary>
    /// Dibuja un gizmo en el editor que representa el rango de excavación.
    /// Solo visible cuando el objeto está seleccionado.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Color amarillo para distinguir el rango
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, digRange);
    }
}

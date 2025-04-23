using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    // Singleton
    public static MusicManager Instance { get; private set; }

    [Header("Configuración de Música")]
    public AudioClip[] songs;  // Array de canciones a asignar desde el Inspector

    [Header("Parámetros de Probabilidad")]
    [SerializeField] private float ProbabilidadReproducir = 0.5f;        // Probabilidad para iniciar una canción cuando no se esté reproduciendo
    [SerializeField] private float ProbabilidadEnReproduccion = 0.7f;    // Probabilidad para controlar la canción en reproducción
    [SerializeField] private float ProbabilidadDePausa = 0.5f;           // Probabilidad de pausar en lugar de detener cuando se controla la canción

    [Header("Intervalos de Tiempo")]
    [SerializeField] private float TiempoMinEspera = 5f;  // Tiempo mínimo de espera entre canciones
    [SerializeField] private float TiempoMaxEspera = 15f; // Tiempo máximo de espera entre canciones    

    [Header("Transiciones de Volumen")]
    [SerializeField] private float fadeInDuration = 2f;   // Duración del efecto Fade In
    [SerializeField] private float fadeOutDuration = 2f;  // Duración del efecto Fade Out

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Obtener el componente AudioSource y comprobar que existe.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se encontró AudioSource en el GameObject");
            return;
        }

        // Iniciar la repetición periódica del método RandomMusicRoutine con un tiempo aleatorio entre las canciones.
        InvokeRepeating("RandomMusicRoutine", 0f, Random.Range(TiempoMinEspera, TiempoMaxEspera));
    }

    void RandomMusicRoutine()
    {
        // Verifica que el array de canciones esté asignado y contenga elementos.
        if (songs == null || songs.Length == 0)
        {
            // Si no hay canciones asignadas, muestra un mensaje de advertencia.
            Debug.LogWarning("No hay canciones asignadas en MusicManager");
            // Finaliza la ejecución actual y vuelve a invocar el método con el siguiente intervalo.
            return;
        }

        // Comprueba si el AudioSource no está reproduciendo ninguna canción.
        if (!audioSource.isPlaying)
        {
            // Con la probabilidad definida, decide si iniciar una nueva canción.
            if (Random.value > ProbabilidadReproducir)
            {
                // Selecciona una canción aleatoria del array y la reproduce.
                int index = Random.Range(0, songs.Length);
                // Asigna la canción seleccionada al AudioSource.
                audioSource.clip = songs[index];

                // Inicia Fade In si ya había otra corutina activa
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }

                // Reproduce la canción con fade in.
                fadeCoroutine = StartCoroutine(FadeIn(fadeInDuration));
            }
        }
        else
        {
            // Si ya se está reproduciendo, con la probabilidad definida, decide pausar o detener la canción.
            if (Random.value > ProbabilidadEnReproduccion)
            {
                // Con la probabilidad definida, decide si pausar o detener la canción.
                if (Random.value > ProbabilidadDePausa)
                {
                    // Pausa la canción para reanudarla más tarde.
                    if (fadeCoroutine != null)
                    {
                        StopCoroutine(fadeCoroutine);
                    }

                    fadeCoroutine = StartCoroutine(FadeOut(fadeOutDuration, true));
                }
                else
                {
                    // Detiene la canción, reiniciando su reproducción en la siguiente ejecución.
                    if (fadeCoroutine != null)
                    {
                        StopCoroutine(fadeCoroutine);
                    }

                    fadeCoroutine = StartCoroutine(FadeOut(fadeOutDuration, false));
                }
            }
        }

        // Establece el siguiente intervalo de espera entre canciones.
        CancelInvoke("RandomMusicRoutine"); // Cancelar la llamada previa
        InvokeRepeating("RandomMusicRoutine", Random.Range(TiempoMinEspera, TiempoMaxEspera), Random.Range(TiempoMinEspera, TiempoMaxEspera));
    }

    // Corutina para reproducir una canción con fade in.
    IEnumerator FadeIn(float duration)
    {
        float targetVolume = 1f;  // Volumen objetivo al final del fade in
        audioSource.volume = 0f;  // Comienza con el volumen en 0
        audioSource.Play();       // Inicia la reproducción de la canción

        // Espera un segundo antes de iniciar el fade in
        float elapsed = 0f;
        while (elapsed < duration) // Espera hasta que el tiempo de fade in se complete
        {
            // Interpolación del volumen desde 0 hasta el volumen objetivo
            audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
            // Aumenta el tiempo transcurrido
            elapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    // Corutina para pausar o detener la canción con fade out.
    IEnumerator FadeOut(float duration, bool pause)
    {
        // Si la canción no se está reproduciendo, no hace nada.
        float startVolume = audioSource.volume; // Volumen inicial
        float elapsed = 0f;                    

        while (elapsed < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Al finalizar el fade out, establece el volumen a 0.
        audioSource.volume = 0f;

        if (pause)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Stop();
        }
    }
}

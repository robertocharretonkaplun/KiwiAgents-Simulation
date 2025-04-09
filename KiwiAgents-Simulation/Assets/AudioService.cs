using UnityEngine;

public class AudioService : MonoBehaviour
{
    // Singleton
    public static AudioService Instance { get; private set; }

    // Referencias a MusicManager y AudioManager
    public MusicManager musicManager;
    public AudioManager audioManager;

    private void Awake()
    {
        // Implementación del Singleton para AudioService
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

        // Buscar los componentes MusicManager y AudioManager en los hijos si no están asignados
        if (musicManager == null)
        {
            musicManager = GetComponentInChildren<MusicManager>();
        }

        if (audioManager == null)
        {
            audioManager = GetComponentInChildren<AudioManager>();
        }

        // Asegurar que los managers sean referenciados correctamente
        if (musicManager == null || audioManager == null)
        {
            Debug.LogError("Faltan referencias a MusicManager o AudioManager en AudioService");
        }
    }
}
